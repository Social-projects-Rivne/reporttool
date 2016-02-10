using ReportingTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IniParser;
using IniParser.Model;
using System.Web.Hosting;

using System.Data.Entity;
using Newtonsoft.Json;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Controllers
{
    public class TeamsController : Controller
    {
        public enum Answer { Exists, Created };
        /*------------------------  It should be moved to some JiraHelper ---------------------------------*/
        private string FILE_NAME = HostingEnvironment.MapPath("~/Configurations.ini");
        private const string SECTION = "GeneralConfiguration";
        private const string PROJECT_NAME_KEY = "ProjectName";
        /*-------------------------------------------------------------------------------------------------*/
        /// <summary>
        /// look for members without teams and [ isactive=>false ]
        /// </summary>
        public static HttpStatusCodeResult MemberCheck()
        {
            using (var ctx = new DB2())
            {
                try
                {
                    var query = from m in ctx.members.Include("teams")
                                orderby m.id
                                select m;

                    List<member> memberList = query.ToList<member>();

                    foreach (member memberVar in memberList)
                    {
                        if (memberVar.teams.Count == 0)
                        {
                            memberVar.isactive = false;
                            ctx.SaveChanges();
                        }
                    }
                    return new HttpStatusCodeResult(HttpStatusCode.OK, "Member table checked successfully");
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotModified, "Member table is not checked ");
                }
            }
        }

        /// <summary>
        /// Get all teams for the specified project
        /// </summary>
        /// <param name="projectKey">name of a project -  string</param>
        /// <returns>A serialized list of teams with their members included</returns>
        [HttpGet]
        public string GetAll(string projectKey = "projectkey1")
        {
            List<team> teamList = new List<team>();

            using (var ctx = new DB2())
            {
                var query = from t in ctx.teams.Include("members")
                            orderby t.projectkey, t.name
                            where t.isactive == true && t.projectkey == projectKey
                            select t;

                teamList = query.ToList();
            }

            //  works
            string ouputJSON = JsonConvert.SerializeObject(teamList, Formatting.Indented);
            //string ouputJSON = JsonConvert.SerializeObject(teamList);
            return ouputJSON;
        }

        [HttpPost]
        public ActionResult AddNewTeam(TempTeamModel newTeam)
        {
            Answer answer;

            FileIniDataParser fileIniData = new FileIniDataParser();
            IniData parsedData = fileIniData.ReadFile(FILE_NAME);
            var project = parsedData[SECTION][PROJECT_NAME_KEY];

            using (var db = new DB2())
            {

                if (db.Teams.Any(p => p.Name == newTeam.teamName & p.ProjectKey == project & p.IsActive))
                {
                    answer = Answer.Exists;
                }
                else
                {
                    var team = new Team { Name = newTeam.teamName, ProjectKey = project, IsActive = true };

                    foreach (var member in newTeam.Members)
                    {
                        var newMember = db.Members.SingleOrDefault(p => p.Username == member.userName);
                        if (newMember != null)
                        {
                            if (newMember.IsActive)
                            {
                                team.Members.Add(newMember);
                            }
                            else
                            {
                                newMember.IsActive = true;
                                team.Members.Add(newMember);
                            }
                        }
                        else
                        {
                            newMember = new Member { Username = member.userName, Fullname = member.fullName, IsActive = true };
                            team.Members.Add(newMember);
                        }
                    }
                    db.Teams.Add(team);
                    db.SaveChanges();
                    answer = Answer.Created;
                }

            }

            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
        }

        /// <summary>
        /// Edit the specified team : add/delete some members
        /// </summary>
        /// <param name="teamFromJSON">A serialized team with a current list of members</param>
        /// <returns>HttpStatusCode for the client</returns>
        [HttpPut]
        public HttpStatusCode Edit([ModelBinder(typeof(JsonNetModelBinder))] team teamFromJSON)
        {
            team teamForUpdate = new team();

            using (var ctx = new DB2())
            {

                //  CHECK :   is a team with the specified projectkey and name present in DB ?
                teamForUpdate = ctx.teams.Include("members")
                    .SingleOrDefault<team>(t => t.name == teamFromJSON.name && t.projectkey == teamFromJSON.projectkey && t.isactive == true);

                //  CHECK RESULT  : No  ---> send a NotFound error response + exit
                if (teamForUpdate == null)
                {
                    return HttpStatusCode.NotFound;
                    //return null;    //  OK
                }

                //  CHECK RESULT  : Yes  ---> keep running

                //  the team in DB -> active
                teamForUpdate.isactive = true;

                try
                {
                    #region  1st run from DB thru JSON - members of the team from DB which are not present in JSON must be deleted
                    bool deleteMember = true;
                    member[] memberArrayDelete = teamForUpdate.members.ToArray<member>();
                    int index = 0;

                    for (int i = memberArrayDelete.GetLowerBound(0), upper = memberArrayDelete.GetUpperBound(0); i <= upper; i++)
                    {
                        deleteMember = true;
                        foreach (var itemFromJSON in teamFromJSON.members)
                        {
                            // found in JSON ?
                            if (memberArrayDelete[i].username == itemFromJSON.username)
                            {
                                deleteMember = false;
                                break;
                            }
                        }
                        // the member from DB is not found in JSON ---> delete 
                        if (deleteMember == true)
                        {
                            teamForUpdate.members.Remove(memberArrayDelete[i]);
                            ctx.SaveChanges();
                        }
                    }

                    #endregion

                    #region 2nd run from JSON thru DB - members of the team from JSON which are not present in DB must be added
                    bool addMember = true;
                    member[] memberArrayAdd = new member[teamFromJSON.members.Count];
                    int idx = 0;

                    // add members present in JSON and missing in DB to add_array
                    foreach (var itemFromJSON in teamFromJSON.members)
                    {
                        addMember = true;
                        foreach (var itemFromDB in teamForUpdate.members)
                        {
                            // already present in DB -> not added to DB
                            if (itemFromDB.username == itemFromJSON.username)
                            {
                                addMember = false;
                                break;
                            }
                        }

                        // the member from JSON is not found in DB ---> add to add_array
                        if (addMember == true)
                        {
                            memberArrayAdd[idx++] = itemFromJSON;
                        }
                    }
                    //  -----
                    // members from add_array are added
                    for (int k = 0; k < teamFromJSON.members.Count; k++)
                    {
                        // **** check if members to add are already present in DB ***********************
                        // 
                        if (memberArrayAdd[k] != null)
                        {
                            member memberTmp = memberArrayAdd[k];

                            member memberDup = ctx.members.
                                SingleOrDefault<member>(m => m.username == memberTmp.username);

                            // if a member with the same name exists he is activated
                            if (memberDup != null)
                            {
                                memberDup.isactive = true;

                                //  Insert Raw SQLcommand for team_member DB table
                                string SqlCommand = "insert into team_member(team_id, member_id) values(" +
                                    teamForUpdate.id + ", " +
                                    memberDup.id + ")";

                                int noOfRowInserted = ctx.Database.ExecuteSqlCommand(SqlCommand);

                                continue;
                            }
                        }
                        // ***********************************************************************************************

                        if (memberArrayAdd[k] != null)
                        {
                            teamForUpdate.members.Add(memberArrayAdd[k]);
                            ctx.SaveChanges();
                        }
                    }

                    #endregion

                    ctx.SaveChanges();
                }
                catch
                {
                    return HttpStatusCode.NotModified;
                }

            }

            MemberCheck();

            //return Json("TEAM UPDATED");    //  OK
            return HttpStatusCode.OK;
        }

        /// <summary>
        /// Delete a team with the specified id
        /// </summary>
        /// <param name="id">team id</param>
        /// <returns>HttpStatusCode for the client</returns>
        [HttpDelete]
        public HttpStatusCodeResult Delete(int id)
        {
            using (var ctx = new DB2())
            {
                team teamDelete = ctx.teams.Include("members")
                .FirstOrDefault<team>(t => t.id == id);

                if (teamDelete == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Team is not found");
                    //return null;    //  OK
                }

                try
                {
                    ctx.teams.Remove(teamDelete);
                    ctx.teams.Attach(teamDelete);
                    ctx.Entry(teamDelete).State = EntityState.Deleted;
                    ctx.SaveChanges();

                    // now we add the deleted team to DB with isactive = false
                    teamDelete.isactive = false;
                    ctx.teams.Add(teamDelete);
                    ctx.SaveChanges();
                }
                catch
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotModified, "Team is not deleted");
                }
            }

            MemberCheck();
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Team deleted successfully");
        }
    }
}

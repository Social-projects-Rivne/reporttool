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
                    var query = from m in ctx.Members.Include("Teams")
                                orderby m.Id
                                select m;

                    List<Member> memberList = query.ToList<Member>();

                    foreach (Member memberVar in memberList)
                    {
                        if (memberVar.Teams.Count == 0)
                        {
                            memberVar.IsActive = false;
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
            List<Team> teamList = new List<Team>();

            using (var ctx = new DB2())
            {
                var query = from t in ctx.Teams.Include("members")
                            orderby t.ProjectKey, t.Name
                            where t.IsActive == true && t.ProjectKey == projectKey
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
        public HttpStatusCode Edit([ModelBinder(typeof(JsonNetModelBinder))] Team teamFromJSON)
        {
            Team teamForUpdate = new Team();

            using (var ctx = new DB2())
            {

                //  CHECK :   is a team with the specified projectkey and name present in DB ?
                teamForUpdate = ctx.Teams.Include("members")
                    .SingleOrDefault<Team>(t => t.Name == teamFromJSON.Name && t.ProjectKey == teamFromJSON.ProjectKey && t.IsActive == true);

                //  CHECK RESULT  : No  ---> send a NotFound error response + exit
                if (teamForUpdate == null)
                {
                    return HttpStatusCode.NotFound;
                    //return null;    //  OK
                }

                //  CHECK RESULT  : Yes  ---> keep running

                //  the team in DB -> active
                teamForUpdate.IsActive = true;

                try
                {
                    #region  1st run from DB thru JSON - members of the team from DB which are not present in JSON must be deleted
                    bool deleteMember = true;
                    Member[] memberArrayDelete = teamForUpdate.Members.ToArray<Member>();
                    int index = 0;

                    for (int i = memberArrayDelete.GetLowerBound(0), upper = memberArrayDelete.GetUpperBound(0); i <= upper; i++)
                    {
                        deleteMember = true;
                        foreach (var itemFromJSON in teamFromJSON.Members)
                        {
                            // found in JSON ?
                            if (memberArrayDelete[i].Username == itemFromJSON.Username)
                            {
                                deleteMember = false;
                                break;
                            }
                        }
                        // the member from DB is not found in JSON ---> delete 
                        if (deleteMember == true)
                        {
                            teamForUpdate.Members.Remove(memberArrayDelete[i]);
                            ctx.SaveChanges();
                        }
                    }

                    #endregion

                    #region 2nd run from JSON thru DB - members of the team from JSON which are not present in DB must be added
                    bool addMember = true;
                    Member[] memberArrayAdd = new Member[teamFromJSON.Members.Count];
                    int idx = 0;

                    // add members present in JSON and missing in DB to add_array
                    foreach (var itemFromJSON in teamFromJSON.Members)
                    {
                        addMember = true;
                        foreach (var itemFromDB in teamForUpdate.Members)
                        {
                            // already present in DB -> not added to DB
                            if (itemFromDB.Username == itemFromJSON.Username)
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
                    for (int k = 0; k < teamFromJSON.Members.Count; k++)
                    {
                        // **** check if members to add are already present in DB ***********************
                        // 
                        if (memberArrayAdd[k] != null)
                        {
                            Member memberTmp = memberArrayAdd[k];

                            Member memberDup = ctx.Members.
                                SingleOrDefault<Member>(m => m.Username == memberTmp.Username);

                            // if a member with the same name exists he is activated
                            if (memberDup != null)
                            {
                                memberDup.IsActive = true;

                                //  Insert Raw SQLcommand for team_member DB table
                                string SqlCommand = "insert into team_member(team_id, member_id) values(" +
                                    teamForUpdate.Id  + ", " +
                                    memberDup.Id + ")";

                                int noOfRowInserted = ctx.Database.ExecuteSqlCommand(SqlCommand);

                                continue;
                            }
                        }
                        // ***********************************************************************************************

                        if (memberArrayAdd[k] != null)
                        {
                            teamForUpdate.Members.Add(memberArrayAdd[k]);
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
                Team teamDelete = ctx.Teams.Include("members")
                .FirstOrDefault<Team>(t => t.Id == id);

                if (teamDelete == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Team is not found");
                    //return null;    //  OK
                }

                try
                {
                    ctx.Teams.Remove(teamDelete);
                    ctx.Teams.Attach(teamDelete);
                    ctx.Entry(teamDelete).State = EntityState.Deleted;
                    ctx.SaveChanges();

                    // now we add the deleted team to DB with isactive = false
                    teamDelete.IsActive = false;
                    ctx.Teams.Add(teamDelete);
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

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

using System.IO;
using System.Data.Entity;
using Newtonsoft.Json;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Controllers
{
    public class TeamsController : Controller
    {
        public enum Answer { NotExists, IsEmpty, NotValid, Exists, Created, NotCreated, NotDeleted, Deleted, NotFound, NotModified, Modified };
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
                        }

                        if (memberVar.Teams.Count > 0)
                        {
                            memberVar.IsActive = true;
                        }
                        ctx.SaveChanges();
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
        public string GetAllTeams()
        {
            FileIniDataParser fileIniData = new FileIniDataParser();
            IniData parsedData = fileIniData.ReadFile(FILE_NAME);
            string projectKey = parsedData[SECTION][PROJECT_NAME_KEY];

            List<Team> teamList = new List<Team>();

            using (var ctx = new DB2())
            {
                //  1
                //var query = from t in ctx.Teams.Include("Members")
                //            orderby t.Name
                //            where t.IsActive == true && t.ProjectKey == projectKey
                //            select t;
                //teamList = query.ToList();

                //  3
                teamList = ctx.Teams
                     .Include(t => t.Members)
                    .OrderBy(t => t.Name)
                     .Where(t => (t.IsActive == true) && (t.ProjectKey == projectKey))
                .ToList();

            }

            //  works
            string ouputJSON = JsonConvert.SerializeObject(teamList, Formatting.Indented);
            //string ouputJSON = JsonConvert.SerializeObject(teamList);
            return ouputJSON;
        }

        [HttpPost]
        public ActionResult AddNewTeam([ModelBinder(typeof(JsonNetModelBinder))] Team newTeam)
        {
            Answer answer;

            FileIniDataParser fileIniData = new FileIniDataParser();
            IniData parsedData = fileIniData.ReadFile(FILE_NAME);
            var project = parsedData[SECTION][PROJECT_NAME_KEY];

            using (var db = new DB2())
            {
                if (db.Teams.Any(p => p.Name == newTeam.Name & p.ProjectKey == project & p.IsActive))
                {
                    answer = Answer.Exists;
                }
                else
                {
                    var team = new Team { Name = newTeam.Name, ProjectKey = project, IsActive = true };

                    foreach (var member in newTeam.Members)
                    {
                        var newMember = db.Members.SingleOrDefault(p => p.UserName == member.UserName);
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
                            newMember = new Member { UserName = member.UserName, FullName = member.FullName, IsActive = true };
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
        //public HttpStatusCode EditTeam([ModelBinder(typeof(JsonNetModelBinder))] Team teamFromJSON)
        public ActionResult EditTeam([ModelBinder(typeof(JsonNetModelBinder))] Team teamFromJSON)
        {
            Answer answer;

            Team teamForUpdate = new Team();

            //  projectKey from.INI file
            FileIniDataParser fileIniData = new FileIniDataParser();
            IniData parsedData = fileIniData.ReadFile(FILE_NAME);
            var ProjectKey = parsedData[SECTION][PROJECT_NAME_KEY];
            //

            using (var ctx = new DB2())
            {

                //  CHECK :   is a team with the specified projectkey and name present in DB ?

                //  CHECK RESULT  : No  ---> send a NotFound error response + exit
                //if (ctx.Teams.Any(t => t.Name == teamFromJSON.Name && t.ProjectKey == ProjectKey && t.IsActive == true) == false)  ----- and if I have changed teamName ??
                if (ctx.Teams.Any(t => t.Id == teamFromJSON.Id && t.ProjectKey == ProjectKey && t.IsActive == true) == false)
                {
                    //return HttpStatusCode.NotFound;
                    answer = Answer.NotFound;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
                }

                //  CHECK RESULT  : Yes  ---> keep running
                teamForUpdate = ctx.Teams.Include("Members")
                  // .SingleOrDefault<Team>(t => t.Name == teamFromJSON.Name && t.ProjectKey == ProjectKey && t.IsActive == true);       ----- the same
                  .SingleOrDefault<Team>(t => t.Id == teamFromJSON.Id && t.ProjectKey == ProjectKey && t.IsActive == true); 
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
                            if (memberArrayDelete[i].UserName == itemFromJSON.UserName)
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
                            if (itemFromDB.UserName == itemFromJSON.UserName)
                            {
                                addMember = false;
                                itemFromDB.IsActive = true;
                                ctx.SaveChanges();
                                break;
                            }
                        }

                        // the member from JSON is not found in DB ---> add to add_array
                        if (addMember == true)
                        {
                            itemFromJSON.IsActive = true;
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
                                SingleOrDefault<Member>(m => m.UserName == memberTmp.UserName);

                            // if a member with the same name exists he is activated
                            if (memberDup != null)
                            {
                                memberDup.IsActive = true;

                                //  Insert Raw SQLcommand for team_member DB table :
                                // INSERT INTO public."TeamMembers"("Team_Id", "Member_Id") VALUES (?, ?);
                                string SqlCommand = "INSERT INTO public.\"TeamMembers\"(\"Team_Id\", \"Member_Id\") VALUES (" +
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
                    answer = Answer.NotModified;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
                }

            }

            MemberCheck();

            //return Json("TEAM UPDATED");    //  OK
            answer = Answer.Modified;
            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete a team with the specified id
        /// </summary>
        /// <param name="id">team id</param>
        /// <returns>HttpStatusCode for the client</returns>
        [HttpDelete]
        //public HttpStatusCodeResult DeleteTeam(int id)
        public ActionResult DeleteTeam(int id)
        {
            Answer answer;
            using (var ctx = new DB2())
            {
                Team teamDelete = ctx.Teams.Include("Members")
                .FirstOrDefault<Team>(t => t.Id == id);

                if (teamDelete == null)
                {
                    answer = Answer.NotFound;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
                    //return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Team is not found");
                }

                try
                {
                    //  Insert Raw SQLcommand for team_member DB table :
                    //  DELETE FROM public."TeamMembers" WHERE "Team_Id" = 1;

                    string SqlCommand = "DELETE FROM public.\"TeamMembers\" WHERE \"Team_Id\" = " + teamDelete.Id + ";";
                    int noOfRowInserted = ctx.Database.ExecuteSqlCommand(SqlCommand);
                
                    //ctx.Teams.Remove(teamDelete);
                    teamDelete.IsActive = false;
                    ctx.Teams.Attach(teamDelete);
                    ctx.Entry(teamDelete).State = EntityState.Modified;

                    ctx.SaveChanges();

                    // now we add the deleted team to DB with isactive = false
                    //teamDelete.IsActive = false;
                    //ctx.Teams.Add(teamDelete);
                    //ctx.SaveChanges();
                }
                catch
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.NotModified, "Team is not deleted");
                    answer = Answer.NotDeleted;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
                }
            }

            MemberCheck();
            //return new HttpStatusCodeResult(HttpStatusCode.OK, "Team deleted successfully");
            answer = Answer.Deleted;
            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);

        }
    }
}

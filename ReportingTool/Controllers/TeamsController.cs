using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using ReportingTool.DAL.Entities;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.Core.Models;

namespace ReportingTool.Controllers
{
    public class TeamsController : Controller
    {
        private readonly IDB2 _db;

        public TeamsController(IDB2 db)
        {
            _db = db;
        }

        public TeamsController() : this(new DB2()) { }

        public enum Answer { NotExists, IsEmpty, NotValid, Exists, Created, NotCreated, NotDeleted, Deleted, NotFound, NotModified, Modified };

        /// <summary>
        /// look for members without teams and [ isactive=>false ]
        /// </summary>
        public static HttpStatusCodeResult MemberCheck()
        {
            using (var ctx = new DB2())
            {
                try
                {
                    var query = from m in ctx.Members.Include("Teams") orderby m.Id select m;
                    var memberList = query.ToList();

                    foreach (var memberVar in memberList)
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
            var projectKey = Session["projectKey"] as string;
            List<Team> teamList;

            using (var ctx = new DB2())
            {
                teamList = ctx.Teams.Include(t => t.Members).OrderBy(t => t.Name)
                    .Where(t => t.IsActive && (t.ProjectKey == projectKey)).ToList();
            }
            return JsonConvert.SerializeObject(teamList, Formatting.Indented);
        }

        [HttpPost]
        public ActionResult AddNewTeam([ModelBinder(typeof(JsonNetModelBinder))] Team newTeam)
        {
            Answer answer;
            var projectKey = Session["projectKey"] as string;

            using (var db = new DB2())
            {
                if (db.Teams.Any(t => t.Name == newTeam.Name & t.ProjectKey == projectKey & t.IsActive))
                {
                    answer = Answer.Exists;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                }

                var team = new Team { Name = newTeam.Name, ProjectKey = projectKey, IsActive = true };

                foreach (var member in newTeam.Members)
                {
                    var newMember = db.Members.SingleOrDefault(m => m.UserName == member.UserName);
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
            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
        }

        /// <summary>
        /// Edit the specified team : add/delete some members
        /// </summary>
        /// <param name="teamFromJSON">A serialized team with a current list of members</param>
        /// <returns>HttpStatusCode for the client</returns>
        [HttpPut]
        public ActionResult EditTeam([ModelBinder(typeof(JsonNetModelBinder))] Team teamFromJSON)
        {
            Answer answer;
            Team teamForUpdate;
            var projectKey = Session["projectKey"] as string;

            using (var ctx = new DB2())
            {

                //  CHECK :   is a team with the specified projectkey and name present in DB ?

                //  CHECK RESULT  : No  ---> send a NotFound error response + exit
                if (ctx.Teams.Any(t => t.Id == teamFromJSON.Id && t.ProjectKey == projectKey && t.IsActive) == false)
                {
                    answer = Answer.NotFound;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
                }

                //  CHECK RESULT  : Yes  ---> keep running
                teamForUpdate = ctx.Teams.Include("Members")
                  .SingleOrDefault(t => t.Id == teamFromJSON.Id && t.ProjectKey == projectKey && t.IsActive);
                //  the team in DB -> active
                teamForUpdate.IsActive = true;

                try
                {
                    #region  1st run from DB thru JSON - members of the team from DB which are not present in JSON must be deleted
                    bool deleteMember;
                    var memberArrayDelete = teamForUpdate.Members.ToArray();

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
                        if (deleteMember)
                        {
                            teamForUpdate.Members.Remove(memberArrayDelete[i]);
                            ctx.SaveChanges();
                        }
                    }

                    #endregion

                    #region 2nd run from JSON thru DB - members of the team from JSON which are not present in DB must be added

                    var memberArrayAdd = new Member[teamFromJSON.Members.Count];
                    var idx = 0;

                    // add members present in JSON and missing in DB to add_array
                    foreach (var itemFromJSON in teamFromJSON.Members)
                    {
                        var addMember = true;
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
                        if (addMember)
                        {
                            itemFromJSON.IsActive = true;
                            memberArrayAdd[idx++] = itemFromJSON;
                        }
                    }
                    //  -----
                    // members from add_array are added
                    for (var k = 0; k < teamFromJSON.Members.Count; k++)
                    {
                        // **** check if members to add are already present in DB ***********************
                        // 
                        if (memberArrayAdd[k] != null)
                        {
                            var memberTmp = memberArrayAdd[k];

                            var memberDup = ctx.Members.SingleOrDefault(m => m.UserName == memberTmp.UserName);

                            // if a member with the same name exists he is activated
                            if (memberDup != null)
                            {
                                memberDup.IsActive = true;

                                //  Insert Raw SQLcommand for team_member DB table :
                                // INSERT INTO public."TeamMembers"("Team_Id", "Member_Id") VALUES (?, ?);
                                var SqlCommand = "INSERT INTO public.\"TeamMembers\"(\"Team_Id\", \"Member_Id\") VALUES (" +
                                    teamForUpdate.Id + ", " + memberDup.Id + ")";

                                ctx.Database.ExecuteSqlCommand(SqlCommand);

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

                    teamForUpdate.Name = teamFromJSON.Name;
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
        public ActionResult DeleteTeam(int id)
        {
            Answer answer;
            using (var ctx = new DB2())
            {
                var teamDelete = ctx.Teams.Include("Members").FirstOrDefault(t => t.Id == id);

                if (teamDelete == null)
                {
                    answer = Answer.NotFound;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    //  Insert Raw SQLcommand for team_member DB table :
                    //  DELETE FROM public."TeamMembers" WHERE "Team_Id" = 1;

                    var SqlCommand = "DELETE FROM public.\"TeamMembers\" WHERE \"Team_Id\" = " + teamDelete.Id + ";";
                    ctx.Database.ExecuteSqlCommand(SqlCommand);

                    teamDelete.IsActive = false;
                    ctx.Teams.Attach(teamDelete);
                    ctx.Entry(teamDelete).State = EntityState.Modified;

                    ctx.SaveChanges();
                }
                catch
                {
                    answer = Answer.NotDeleted;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
                }
            }
            MemberCheck();
            answer = Answer.Deleted;
            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// Get members of team with specific id
        /// </summary>
        /// <param name="id">Team id</param>
        /// <returns>List of team members</returns>
        [HttpGet]
        public ActionResult GetMembersByTeamId(int id)
        {
            Answer answer;
            List<MemberModel> members = new List<MemberModel>();

            var team = _db.Teams.SingleOrDefault(t => t.Id == id);
            if (team == null)
            {
                answer = Answer.NotFound;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
            }

            foreach (var teamMember in team.Members)
            {
                var member = new MemberModel
                {
                    userName = teamMember.UserName,
                    fullName = teamMember.FullName
                };

                members.Add(member);
            }
            return Json(members, JsonRequestBehavior.AllowGet);
        }
    }
}

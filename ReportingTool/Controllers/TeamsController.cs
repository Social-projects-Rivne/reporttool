using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web.Mvc;
using Newtonsoft.Json;
using ReportingTool.Core.Validation;
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

        public enum Answer
        {
            NotExists, IsEmpty, NotValid, Exists, Created, NotCreated, NotDeleted, Deleted,
            NotFound, NotModified, Modified
        };

        /// <summary>
        /// look for members without teams and [ isactive=>false ]
        /// </summary>
        public static HttpStatusCodeResult MemberCheck()
        {
            using (var ctx = new DB2())
            {
                try
                {
                    var memberList = ctx.Members.Include("Teams").ToList();

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
                teamList = ctx.Teams.AsNoTracking().Include(t => t.Members)
                    .Where(t => t.IsActive && (t.ProjectKey == projectKey)).ToList();
            }
            return JsonConvert.SerializeObject(teamList, Formatting.Indented);
        }

        [HttpPost]
        public ActionResult AddNewTeam([ModelBinder(typeof(JsonNetModelBinder))] Team newTeam)
        {
            var projectKey = Session["projectKey"] as string;

            var validation = newTeam.TeamValid();
            if (validation != null) return Json(new { Answer = validation });

            using (var db = new DB2())
            {
                var team = db.Teams.FirstOrDefault(t => t.Name == newTeam.Name & t.ProjectKey == projectKey);
                if (team == null)
                {
                    team = new Team { Name = newTeam.Name, ProjectKey = projectKey, IsActive = true };
                    db.Teams.Add(team);
                }
                else if (team.IsActive)
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.Exists) });
                else if (!team.IsActive) team.IsActive = true;

                foreach (var member in newTeam.Members)
                {
                    var newMember = db.Members.FirstOrDefault(m => m.UserName == member.UserName);
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

                db.SaveChanges();
            }
            return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.Created) });
        }

        /// <summary>
        /// Edit the specified team : add/delete some members
        /// </summary>
        /// <param name="teamFromJSON">A serialized team with a current list of members</param>
        /// <returns>HttpStatusCode for the client</returns>
        [HttpPut]
        public ActionResult EditTeam([ModelBinder(typeof(JsonNetModelBinder))] Team teamFromJSON)
        {
            var projectKey = Session["projectKey"] as string;
            var validation = teamFromJSON.TeamValid();
            if (validation != null) return Json(new { Answer = validation });

            using (var ctx = new DB2())
            {
                //  CHECK :   is a team with the specified projectkey and name present in DB ?
                var teamForUpdate = ctx.Teams.Include("Members")
                    .SingleOrDefault(t => t.Id == teamFromJSON.Id && t.ProjectKey == projectKey && t.IsActive);
                //  CHECK RESULT  : No  ---> send a NotFound error response + exit
                if (teamForUpdate == null)
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.NotFound) }, JsonRequestBehavior.AllowGet);
                //  CHECK :   is a teamFromJSON has the same name as other team
                if (ctx.Teams.Any(t => t.Id != teamFromJSON.Id && t.Name == teamFromJSON.Name && t.ProjectKey == projectKey && t.IsActive))
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.Exists) }, JsonRequestBehavior.AllowGet);
                //  CHECK RESULT  : Yes  ---> keep running
                try
                {
                    teamForUpdate.Members.Clear();

                    foreach (var member in teamFromJSON.Members)
                    {
                        var newMember = ctx.Members.FirstOrDefault(m => m.UserName == member.UserName);
                        if (newMember != null)
                        {
                            if (newMember.IsActive)
                            {
                                teamForUpdate.Members.Add(newMember);
                            }
                            else
                            {
                                newMember.IsActive = true;
                                teamForUpdate.Members.Add(newMember);
                            }
                        }
                        else
                        {
                            newMember = new Member { UserName = member.UserName, FullName = member.FullName, IsActive = true };
                            teamForUpdate.Members.Add(newMember);
                        }
                    }
                    
                    teamForUpdate.Name = teamFromJSON.Name;
                    ctx.SaveChanges();
                }
                catch
                {
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.NotModified) }, JsonRequestBehavior.AllowGet);
                }
            }
            MemberCheck();

            //return Json("TEAM UPDATED");    //  OK
            return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.Modified) }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Delete a team with the specified id
        /// </summary>
        /// <param name="id">team id</param>
        /// <returns>HttpStatusCode for the client</returns>
        [HttpDelete]
        public ActionResult DeleteTeam(int id)
        {
            using (var ctx = new DB2())
            {
                var teamDelete = ctx.Teams.Include("Members").FirstOrDefault(t => t.Id == id);

                if (teamDelete == null)
                {
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.NotFound) }, JsonRequestBehavior.AllowGet);
                }

                try
                {
                    teamDelete.Members.Clear();
                    teamDelete.IsActive = false;
                    ctx.SaveChanges();
                }
                catch
                {
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.NotDeleted) }, JsonRequestBehavior.AllowGet);
                }
            }
            MemberCheck();
            return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.Deleted) }, JsonRequestBehavior.AllowGet);
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

using ReportingTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using IniParser;
using IniParser.Model;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Controllers
{
    public class TeamsController : Controller
    {

        private string FILE_NAME = @"E:\programming\studing\softserve\newdev\reporttool\Configurations.ini";
        private const string SECTION = "GeneralConfiguration";
        private const string PROJECT_NAME_KEY = "ProjectName";

        [HttpGet]
        public JsonResult GetAllTeams() {
            var temp = new List<TempTeamModel>();
           TempTeamModel TempTeam = new TempTeamModel();
            TempTeam.teamID = 5;
            TempTeam.teamName= "Test Team";
            TempTeam.members.Add(new TempMemberModel( "feodtc", "Feol"));
            TempTeam.members.Add(new TempMemberModel( "loginName_1", "fullName_1"));
            TempTeam.members.Add(new TempMemberModel( "loginName_2", "fullName_2"));
            temp.Add(TempTeam);
            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddNewTeam(/*Some teamModel object*/)
        {
            Answer answer;

            FileIniDataParser fileIniData = new FileIniDataParser();
            IniData parsedData = fileIniData.ReadFile(FILE_NAME);
            var projectkey = parsedData[SECTION][PROJECT_NAME_KEY];

            using (var db = new DB2())
            {
                var allmembers = new List<Member>
                {
                    new Member {Username = "ssund", Fullname = "sergiy"},
                    new Member {Username = "oldv", Fullname = "oldv"},
                    new Member {Username = "newv", Fullname = "newv"}
                };

                string teamname = "newtea";
                string project = projectkey;

                if (db.Teams.Any(p => p.Name == teamname & p.ProjectKey == project & p.IsActive))
                {
                    answer = Answer.Exists;
                }
                else
                {
                    var team = new Team { Name = teamname, ProjectKey = project, IsActive = true };
                    db.Teams.Add(team);
                    db.SaveChanges();

                    foreach (var memb in allmembers)
                    {
                        var member = db.Members.SingleOrDefault(p => p.Username == memb.Username);
                        if (member != null)
                        {
                            if (member.IsActive)
                            {
                                CreateConnection(team.Id, member.Id);
                            }
                            else
                            {
                                member.IsActive = true;
                                CreateConnection(team.Id, member.Id);
                            }
                        }
                        else
                        {
                            member = new Member { Username = memb.Username, Fullname = memb.Fullname, IsActive = true };
                            db.Members.Add(member);
                            db.SaveChanges();
                            CreateConnection(team.Id, member.Id);
                        }
                    }
                    answer = Answer.Created;
                }

            }

            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
        }

        [HttpPut]
        public HttpStatusCode EditTeam(/*Some teamModel object*/)
        {
            return HttpStatusCode.OK;
        }

        [HttpDelete]
        public HttpStatusCodeResult DeleteTeam(string teamID)
        {
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Team deleted successfully");
        }
        public void CreateConnection(int teamid, int memberid)
        {
            using (var db = new DB2())
            {
                var connection = new TeamMember { TeamId = teamid, MemberId = memberid };
                db.TeamMembers.Add(connection);
                db.SaveChanges();
            }
        }
    }
}
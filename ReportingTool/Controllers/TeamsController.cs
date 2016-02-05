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
using System.Web.Hosting;

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
        [HttpGet]
        public JsonResult GetAllTeams()
        {
            var temp = new List<TempTeamModel>();
            TempTeamModel TempTeam = new TempTeamModel();
            TempTeam.teamID = 5;
            TempTeam.teamName = "Test Team 1";
            TempTeam.members.Add(new TempMemberModel("1_feodtc", "1_Feol"));
            TempTeam.members.Add(new TempMemberModel("1_loginName_1", "1_fullName_1"));
            TempTeam.members.Add(new TempMemberModel("1_loginName_2", "1_fullName_2"));
            temp.Add(TempTeam);

            TempTeam = new TempTeamModel("7", "Test Team 2");
            TempTeam.members.Add(new TempMemberModel("2_loginName_1", "2_fullName_1"));
            TempTeam.members.Add(new TempMemberModel("2_loginName_2", "2_fullName_2"));
            TempTeam.members.Add(new TempMemberModel("2_loginName_3", "2_fullName_3"));
            TempTeam.members.Add(new TempMemberModel("2_loginName_4", "2_fullName_4"));
            temp.Add(TempTeam);

            TempTeam = new TempTeamModel("11", "Test Team 3");
            TempTeam.members.Add(new TempMemberModel("3_loginName_1", "3_fullName_1"));
            TempTeam.members.Add(new TempMemberModel("3_loginName_2", "3_fullName_2"));
            temp.Add(TempTeam);

            return Json(temp, JsonRequestBehavior.AllowGet);
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
                db.Database.Log = Console.WriteLine;
                var allmembers = new List<member>
                {
                    new member {username = "ssund", fullname = "sergiy"},
                    new member {username = "oldv", fullname = "oldv"},
                    new member {username = "newv", fullname = "newv"}
                };

                string teamname = "newteama";

                if (db.teams.Any(p => p.name == teamname & p.projectkey == project & p.isactive))
                {
                    answer = Answer.Exists;
                }
                else
                {
                    var newteam = new team { name = teamname, projectkey = project, isactive = true };

                    foreach (var memb in allmembers)
                    {
                        var newmember = db.members.SingleOrDefault(p => p.username == memb.username);
                        if (newmember != null)
                        {
                            if (newmember.isactive)
                            {
                                newteam.members.Add(newmember);
                            }
                            else
                            {
                                newmember.isactive = true;
                                newteam.members.Add(newmember);
                            }
                        }
                        else
                        {
                            newmember = new member { username = memb.username, fullname = memb.fullname, isactive = true };
                            newteam.members.Add(newmember);
                        }
                    }
                    db.teams.Add(newteam);
                    db.SaveChanges();
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
    }
}

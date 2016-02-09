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

                if (db.Teams.Any(p => p.Name == newTeam.teamName & p.ProjectKey == project & p.IsActive))
                {
                    answer = Answer.Exists;
                }
                else
                {
                    var team = new Team { Name = newTeam.teamName, ProjectKey = project, IsActive = true };

                    foreach (var member in newTeam.members)
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

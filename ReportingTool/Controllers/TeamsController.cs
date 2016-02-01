using ReportingTool.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace ReportingTool.Controllers
{
    public class TeamsController : Controller
    {
        [HttpGet]
        public JsonResult GetAllTeams() {
            var temp = new List<TempTeamModel>();
           TempTeamModel TempTeam = new TempTeamModel();
            TempTeam.teamID = 5;
            TempTeam.teamName= "Test Team 1";
            TempTeam.members.Add(new TempMemberModel("1_feodtc", "1_Feol"));
            TempTeam.members.Add(new TempMemberModel("1_loginName_1", "1_fullName_1"));
            TempTeam.members.Add(new TempMemberModel("1_loginName_2", "1_fullName_2"));
            temp.Add(TempTeam);

            TempTeam = new TempTeamModel(7, "Test Team 2");
            TempTeam.members.Add(new TempMemberModel("2_loginName_1", "2_fullName_1"));
            TempTeam.members.Add(new TempMemberModel("2_loginName_2", "2_fullName_2"));
            TempTeam.members.Add(new TempMemberModel("2_loginName_3", "2_fullName_3"));
            TempTeam.members.Add(new TempMemberModel("2_loginName_4", "2_fullName_4"));
            temp.Add(TempTeam);
            
            TempTeam = new TempTeamModel(11, "Test Team 3");
            TempTeam.members.Add(new TempMemberModel("3_loginName_1", "3_fullName_1"));
            TempTeam.members.Add(new TempMemberModel("3_loginName_2", "3_fullName_2"));
            temp.Add(TempTeam);

            return Json(temp, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public HttpStatusCode AddNewTeam(/*Some teamModel object*/)
        {
            return HttpStatusCode.OK;
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
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
            TempTeam.teamName= "Test Team";
            TempTeam.members.Add(new TempMemberModel( "feodtc", "Feol"));
            TempTeam.members.Add(new TempMemberModel( "loginName_1", "fullName_1"));
            TempTeam.members.Add(new TempMemberModel( "loginName_2", "fullName_2"));
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
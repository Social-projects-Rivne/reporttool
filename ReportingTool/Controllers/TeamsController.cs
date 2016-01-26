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
            return Json("");
        }

        [HttpPost]
        public HttpStatusCode AddNewTeam(/*Some teamModel object*/) {
            return HttpStatusCode.OK;
        }

        [HttpPut]
        public HttpStatusCode EditTeam(/*Some teamModel object*/)
        {
            return HttpStatusCode.OK;
        }

        [HttpDelete]
        public HttpStatusCodeResult DeleteTeam(string teamID) {
            return new HttpStatusCodeResult(HttpStatusCode.OK, "Team deleted successfully");
        }
    }
}
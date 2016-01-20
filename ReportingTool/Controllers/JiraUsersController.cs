using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportingTool.DAL.DataAccessLayer.Jira;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Controllers
{
    public class JiraUsersController : Controller
    {
        // GET: JiraUsers
        [HttpGet]
        public ActionResult GetAllUsers(string project, string username, string password)
        {
            JiraClient client = new JiraClient("http://ssu-jira.softserveinc.com", username, password);
            List<JiraUser> users = client.GetAllUsers(project);
            return new JsonResult { Data = users, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
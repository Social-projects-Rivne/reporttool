using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.DAL.Entities;
using ReportingTool.Models;

namespace ReportingTool.Controllers
{
    public class JiraUsersController : Controller
    {
        // GET: JiraUsers
        [HttpGet]
        public JsonResult GetAllUsers(string username, string password)
        {
            /*---------------------- Remove hardcode!!! ---------------------------------*/

            JiraClient client = new JiraClient("http://ssu-jira.softserveinc.com", "ofeodtc", "jss-em}t");
            var JiraUsers = client.GetAllUsers("RVNETJAN").ToList();
            List<Member> members = new List<Member>();
            foreach (var user in JiraUsers)
            {
                members.Add(new Member(user.name, user.displayName));
            }
            return new JsonResult { Data = members, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
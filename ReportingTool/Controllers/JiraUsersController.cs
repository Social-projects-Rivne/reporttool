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
            List<JiraUser> users = client.GetAllUsers("RVNETJAN");
            List<TempMemberModel> members = new List<TempMemberModel>();
            foreach(JiraUser user in users){
                members.Add(new TempMemberModel(user.name, user.displayName));
            }
            return new JsonResult { Data = members, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using ReportingTool.Core.Services;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.DAL.Entities;
using ReportingTool.Core.Models;

namespace ReportingTool.Controllers
{
    public class JiraUsersController : Controller
    {
        private static List<JiraUser> UsersStorage { get; set; }

        // GET: JiraUsers
        [HttpGet]
        public JsonResult GetAllUsers(string searchValue)
        {
            List<JiraUserModel> members = new List<JiraUserModel>();
            if (!string.IsNullOrEmpty(searchValue))
            {
                IEnumerable<JiraUserModel> temp =  UsersStorage.Where(user => user.displayName.ToLower().Contains(searchValue))
                    .Select(user => JiraUserService.CreateModelFromEntity(user));
                members = temp.ToList();
            }

            return new JsonResult { Data = members, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpGet]
        public HttpStatusCodeResult CreateBackendStorage()
        {
            try
            {
                JiraClient client = Session["jiraClient"] as JiraClient;
                var projectKey = Session["projectKey"] as string;
                if (client == null || projectKey == null)
                {
                    return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Wrong credentials");
                }
                UsersStorage = client.GetAllUsers(projectKey).ToList();
                return new HttpStatusCodeResult(HttpStatusCode.OK, "Users get from Jira successfully");
            }
            catch (Exception e)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, e.Message);
            }
        }
    }
}
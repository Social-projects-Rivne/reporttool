using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using System.Net;

using ReportingTool.Core.Services;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.Core.Validation;
using ReportingTool.Core.Models;


namespace ReportingTool.Controllers
{
    public class ReportsController : Controller
    {
        IJiraClientService jiraClientService;

        public ReportsController() : this(new JiraClientService()) { }

        public ReportsController(IJiraClientService jiraClientService)
        {
            this.jiraClientService = jiraClientService;
        }

        /// <summary>
        /// Retrieving spent time for specific user for specific period of time
        /// </summary>
        /// <param name="userName">Login of specific user</param>
        /// <param name="dateFrom">Lower boundary of time period</param>
        /// <param name="dateTo">Upper boundary of time period</param>
        /// <returns>Time is seconds</returns>
        [HttpGet]
        public ActionResult GetUserActivity(string userName, string dateFrom, string dateTo)
        {
            int result = 0;
            try
            {
                result = jiraClientService.GetUserActivity(userName, dateFrom, dateTo);            
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            return Json(new { Timespent = result }, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// Retrieveing issues of specific user for specific period of time
        /// </summary>
        /// <param name="userName">Login of specific user</param>
        /// <param name="dateFrom">Lower boundary of time period</param>
        /// <param name="dateTo">Upper boundary of time period</param>
        /// <returns>List of issues</returns>
        [HttpGet]
        public ActionResult GetIssues(string userName, string dateFrom, string dateTo)
        {
            List<IssueModel> result = new List<IssueModel>();
            try
            {
                result = jiraClientService.GetUserIssues(userName, dateFrom, dateTo);
            }
            catch (ArgumentException)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);        
            }
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
	}
}
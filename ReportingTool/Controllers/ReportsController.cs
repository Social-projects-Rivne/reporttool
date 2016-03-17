using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;

using ReportingTool.Core.Services;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.Core.Validation;


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
        /// <param name="userName"></param>
        /// <param name="dateFrom"></param>
        /// <param name="dateTo"></param>
        /// <returns></returns>
        [HttpGet]
        public ActionResult GetUserActivity(string userName, string dateFrom, string dateTo)
        {
           var result = jiraClientService.GetUserActivity(userName, dateFrom, dateTo);

           return (result == -1) ? 
               Json( "Wrong request", JsonRequestBehavior.AllowGet) :
               Json(new { Timespent = result }, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public ActionResult GetIssues(string userName, string dateFrom, string dateTo)
        {
            var result = jiraClientService.GetUserIssues(userName, dateFrom, dateTo);
            return Json(result, JsonRequestBehavior.AllowGet);
        }
        
	}
}
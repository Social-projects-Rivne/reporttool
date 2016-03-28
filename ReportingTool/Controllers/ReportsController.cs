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
using SelectPdf;
using System.IO;


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
            return Json(new { Timespent = result, userNameFromBE = userName }, JsonRequestBehavior.AllowGet);
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
            return Json(new { Issues = result, userNameFromBE = userName }, JsonRequestBehavior.AllowGet);
        }


        public PartialViewResult PreviewReport()
        {
            return PartialView("ReportPreview");
        }

        [HttpPost]
        public ActionResult ExportToPdf(object report)
        {
            // instantiate a html to pdf converter object
            HtmlToPdf converter = new HtmlToPdf();

            // create a new pdf document converting an url
            PdfDocument doc = converter.ConvertHtmlString(RenderRazorViewToString("ReportPreview"));

            // save pdf document
            byte[] pdf = doc.Save();

            // close pdf document
            doc.Close();
            FileResult fileResult = new FileContentResult(pdf, "application/pdf");
            fileResult.FileDownloadName = "Document.pdf";
            return fileResult;
        }

        string RenderRazorViewToString(string viewName)
        {
            using (var sw = new StringWriter())
            {
                var viewResult = ViewEngines.Engines.FindView(ControllerContext, viewName, "_Layout");
                var viewContext = new ViewContext(ControllerContext, viewResult.View,
                                             ViewData, TempData, sw);
                viewResult.View.Render(viewContext, sw);
                viewResult.ViewEngine.ReleaseView(ControllerContext, viewResult.View);
                return sw.GetStringBuilder().ToString();
            }
        }

    }
}
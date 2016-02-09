using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        [HttpGet]
        public ActionResult GetAllTemplates()
        {
            List<string> templates = new List<string>();
            using (var db = new DB2())
            {
                foreach (var template in db.Templates)
                {
                    if (template.Deleted)
                    { }
                    else {
                        templates.Add(template.Name);
                    }
                }
            }
            return new JsonResult { Data = templates, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}
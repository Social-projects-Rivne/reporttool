
﻿using System;
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
            List<Template> templates = new List<Template>();
            using (var db = new DB2())
            {
                foreach (var template in db.Templates)
                {
                    if (template.IsActive)
                    {
                        templates.Add(new Template { Name = template.Name, Id = template.Id, Owner = template.Owner });
                    }
                    
                }
            }
            return new JsonResult { Data = templates, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
    }
}

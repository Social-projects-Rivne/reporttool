
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportingTool.DAL.Entities;
using ReportingTool.Core.Validation;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        private enum Answer { AlreadyExists, WrongName, Added };

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

        [HttpPost]
        public ActionResult AddNewTemplate(Template template)
        {
            Answer answer;

            using (var db = new DB2())
            {
                //Check if template with incoming name already exists in database
                if (db.Templates.Where(t => t.Name == template.Name).Count() > 0)
                {
                    answer = Answer.AlreadyExists;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                }
                else
                    if (!TemplatesValidator.TemplateNameIsCorrect(template.Name))
                    {
                        answer = Answer.WrongName;
                        return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                    }

                db.Templates.Add(template);
                db.SaveChanges();
                answer = Answer.Added;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }
        }
    }
}

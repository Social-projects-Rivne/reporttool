﻿
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportingTool.DAL.Entities;
using ReportingTool.Core.Validation;
﻿using Newtonsoft.Json;


namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        private enum Answer { AlreadyExists, WrongName, WrongOwnerName, Added, IsNull };

        [HttpGet]
        public string GetAllTemplates()
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
            var alltemplates = templates.ToList();
            var outputJSON = JsonConvert.SerializeObject(alltemplates, Formatting.Indented);
            return outputJSON;
        }     

        [HttpPost]
        public ActionResult AddNewTemplate([ModelBinder(typeof(JsonNetModelBinder))] Template template)
        {
            Answer answer;

            if (template == null)
            {
                answer = Answer.IsNull;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }

            if (!TemplatesValidator.TemplateNameIsCorrect(template.Name))
            {
                answer = Answer.WrongName;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }

            if (!TemplatesValidator.TemplateOwnerNameIsCorrect(template.Owner))
            {
                answer = Answer.WrongOwnerName;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }

            using (var db = new DB2())
            {
                //Check if template with incoming name already exists in database
                if (db.Templates.Where(t => t.Name == template.Name).Count() > 0)
                {
                    answer = Answer.AlreadyExists;
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

using System;
using System.Collections.Generic;
﻿using System.Linq;
﻿using System.Web.Mvc;
﻿using Newtonsoft.Json;
﻿using ReportingTool.DAL.Entities;
﻿using ReportingTool.Models;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        public enum Answer { Edited };

        [HttpGet]
        public string GetAllTemplates()
        {
            List<Template> templates = new List<Template>();
            List<FieldsInTemplate> fields = new List<FieldsInTemplate>();
            using (var db = new DB2())
            {
                
                foreach (var template in db.Templates)
                {
                    if (template.IsActive)
                    {
                        templates.Add(new Template {Name = template.Name, Id = template.Id});
                    }
                }
            }
            var outputJSON = JsonConvert.SerializeObject(templates, Formatting.Indented);
            return outputJSON;
        }

        [HttpPut]
        public ActionResult EditTemplate([ModelBinder(typeof(JsonNetModelBinder))] Template editTemplate)
        {
            Answer answer;
            using (var db = new DB2())
            {
                var editedTemplate = db.Templates.SingleOrDefault(p => p.Id == editTemplate.Id);

                editedTemplate.Name = editTemplate.Name;
                
                foreach (var newfield in editTemplate.FieldsInTemplate)
                {
                    foreach (var field in editedTemplate.FieldsInTemplate)
                    {
                        if (field.FieldId == newfield.FieldId)
                        {
                            field.Default = newfield.Default;
                        }
                    }
                } 
                
                db.SaveChanges();
                answer = Answer.Edited;
            }
            
            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
        }
    }
}

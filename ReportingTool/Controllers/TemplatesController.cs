using System;
using System.Collections.Generic;
﻿using System.Linq;
﻿using System.Web.Mvc;
﻿using Newtonsoft.Json;
using ReportingTool.Core.Validation;
using ReportingTool.DAL.Entities;
﻿using ReportingTool.Models;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        public enum Answer { WrongTemplate, WrongName, WrongId, FieldsIsEmpty, FieldIsNotCorrect, Edited };

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

                if (!TemplatesValidator.TemplateIsCorrect(editTemplate))
                {
                    answer = Answer.WrongTemplate;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                }

                if (!TemplatesValidator.IfTemplateIdExists(editTemplate.Id))
                {
                    answer = Answer.WrongId;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                }

                if (!TemplatesValidator.TemplateNameIsCorrect(editTemplate.Name))
                {
                    answer = Answer.WrongName;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                }

                if (!TemplatesValidator.FieldsInTemplateIsNull(editTemplate.FieldsInTemplate))
                {
                    
                    answer = Answer.FieldsIsEmpty;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                }

                if (!TemplatesValidator.FieldInFieldsInTemplateIsCorrect(editTemplate.FieldsInTemplate))
                {
                    answer = Answer.FieldIsNotCorrect;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                }
            
            using (var db = new DB2())
            {
                var editedTemplate = db.Templates.SingleOrDefault(p => p.Id == editTemplate.Id);
                var delete = editedTemplate.FieldsInTemplate.ToArray();

                for (int i = 0; i < delete.Count(); i++)
                {
                    var deletefield = true;
                    foreach (var fields in editTemplate.FieldsInTemplate)
                    {

                        if (delete[i].FieldId == fields.FieldId)
                        {
                            deletefield = false;
                        }
                    }
                    if (deletefield)
                    {
                        db.FieldsInTemplates.Remove(delete[i]);
                    }

                }

                foreach (var fields in editTemplate.FieldsInTemplate)
                {
                    var field = editedTemplate.FieldsInTemplate.SingleOrDefault(p => p.FieldId == fields.FieldId);
                    if (field != null)
                    {
                        field.Default = fields.Default;
                    }
                    else
                    {
                        fields.Field = db.Fields.SingleOrDefault(p => p.Id == fields.FieldId);
                        editedTemplate.FieldsInTemplate.Add(fields);
                    }
                }

                if (editedTemplate.Name != editTemplate.Name)
                {
                    editedTemplate.Name = editTemplate.Name;
                }
                

                db.SaveChanges();
                answer = Answer.Edited;
            }
            
            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
        }
    }
}

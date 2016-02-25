using System;
using System.Collections.Generic;
﻿using System.Linq;
using System.Web.Mvc;
using ReportingTool.DAL.Entities;
using ReportingTool.Core.Validation;
﻿using Newtonsoft.Json;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.Models;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        private enum Answer { WrongTemplate, WrongName, WrongId,
            FieldsIsEmpty, FieldIsNotCorrect, Edited, AlreadyExists, WrongOwnerName, Added, IsNull };

        private readonly IDB2 _db;

        public TemplatesController(IDB2 db)
        {
            _db = db;
        }

        public TemplatesController() : this (new DB2()) {}

        [HttpGet]
        public string GetAllTemplates()
        {
            List<Template> templates = new List<Template>();

            //using (var db = new DB2())
            //{
                foreach (var template in _db.Templates)
                {
                    if (template.IsActive)
                    {
                        templates.Add(new Template {Name = template.Name, Id = template.Id});
                    }
                }
            //}
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
                        field.DefaultValue = fields.DefaultValue;
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

        [HttpGet]
        public string GetTemplateFields(int templateId)
        {
            List<TemplateFieldsDataModel> fields;
            string owner, temaplateName;
            //using (var db = new DB2())
            //{
                var template = _db.Templates.FirstOrDefault(t => t.Id == templateId);
                if (template == null)
                {
                    return JsonConvert.SerializeObject(Json(new { Answer = "False" }));
                }
                owner = template.Owner;
                temaplateName = template.Name;
                var getFields = from filedsInTemplate in _db.FieldsInTemplates
                                join field in _db.Fields on filedsInTemplate.FieldId equals field.Id
                                where filedsInTemplate.TemplateId == templateId
                                select new TemplateFieldsDataModel { FieldName = field.Name, DefaultValue = filedsInTemplate.DefaultValue };
                fields = getFields.ToList();
            //}
            TemplateData templateData = new TemplateData { Fields = fields, Owner = owner, TemplateName = temaplateName };
            return JsonConvert.SerializeObject(templateData, Formatting.Indented);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }
        
    }
}
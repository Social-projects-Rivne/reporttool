using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ReportingTool.DAL.Entities;
using ReportingTool.Core.Validation;
﻿using Newtonsoft.Json;
﻿using ReportingTool.DAL.Entities;
﻿using ReportingTool.Models;

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

        [HttpGet]
        public string GetAllFields() {
            var temp = new List<object>();
            string[] arr1 = new string[]{"value_1", "value_2", "value_3"};
            temp.Add(new { fieldID = 4, fieldName = "Reasons", fieldType = "text", fieldDefaultValue = "qwertyui qwertyuio" });
            temp.Add(new { fieldID = 5, fieldName = "Field_2", fieldType = "date", fieldDefaultValue = "" });
            temp.Add(new { fieldID = 1, fieldName = "Receiver", fieldType = "combobox", fieldDefaultValue =  arr1});
            temp.Add(new { fieldID = 0, fieldName = "Reporter", fieldType = "combobox", fieldDefaultValue = arr1 });
            temp.Add(new { fieldID = 2, fieldName = "Usual Tasks", fieldType = "text", fieldDefaultValue = "wesrdths sdrhtd srghdg" });
            temp.Add(new { fieldID = 3, fieldName = "Risk and Issues", fieldType = "combobox", fieldDefaultValue = arr1 });

            return JsonConvert.SerializeObject(temp, Formatting.Indented);
        }

        [HttpGet]
        public string GetTemplateFields(int templateId)
        {
            List<TemplateFieldsDataModel> fields;
            string temaplateName;
            bool isOwner;
            using (var db = new DB2())
            {
                var template = db.Templates.FirstOrDefault(t => t.Id == templateId);
                if (template == null)
                {
                    return JsonConvert.SerializeObject(Json(new { Answer = "False" }));
                }
                string templateOwner = template.Owner;
                isOwner = CheckIfCurrentUserIsOwnerOfTemplate(templateOwner);
                temaplateName = template.Name;
                var getFields = from filedsInTemplate in db.FieldsInTemplates
                                join field in db.Fields on filedsInTemplate.FieldId equals field.Id
                                where filedsInTemplate.TemplateId == templateId
                                select new TemplateFieldsDataModel { FieldName = field.Name, DefaultValue = filedsInTemplate.DefaultValue };
                fields = getFields.ToList();
            }
            TemplateData templateData = new TemplateData { Fields = fields, IsOwner = isOwner, TemplateName = temaplateName };
            return JsonConvert.SerializeObject(templateData, Formatting.Indented);
        }

        private bool CheckIfCurrentUserIsOwnerOfTemplate(string templateOwner)
        {
            string currentUser = Session["currentUser"] as string;
            if (currentUser == null)
                return false;
            return currentUser.Equals(templateOwner);
        }
    }
}
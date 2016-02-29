using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using ReportingTool.Core.Validation;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.DAL.Entities;
using ReportingTool.Models;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        private enum Answer
        {
            WrongTemplate, WrongName, WrongId,
            FieldsIsEmpty, FieldIsNotCorrect, Edited, AlreadyExists, WrongOwnerName, Added, IsNull
        };

        private readonly IDB2 _db;

        public TemplatesController(IDB2 db)
        {
            _db = db;
        }

        public TemplatesController() : this(new DB2()) { }

        [HttpGet]
        public string GetAllFields()
        {
            var fields = _db.Fields.Select(field => new Field { Name = field.Name, Id = field.Id }).ToList();

            var outputJSON = JsonConvert.SerializeObject(fields, Formatting.Indented);
            return outputJSON;
        }

        [HttpGet]
        public string GetAllTemplates()
        {
            var templates = (from template in _db.Templates where template.IsActive select new Template { Name = template.Name, Id = template.Id }).ToList();

            var outputJSON = JsonConvert.SerializeObject(templates, Formatting.Indented);
            return outputJSON;
        }
        //[HttpPut]
        //public ActionResult EditTemplate([ModelBinder(typeof(JsonNetModelBinder))] Template template)
        //{

        //    Answer answer;
        //    if (!template.IsValid()) return new HttpStatusCodeResult(HttpStatusCode.BadRequest);

        //    if (!TemplatesValidator.FieldsInTemplateIsNull(template.FieldsInTemplate))
        //    {
        //        answer = Answer.FieldsIsEmpty;
        //        return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
        //    }

        //    if (!TemplatesValidator.FieldInFieldsInTemplateIsCorrect(template.FieldsInTemplate))
        //    {
        //        answer = Answer.FieldIsNotCorrect;
        //        return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
        //    }

        //    var templateFromDb = _db.Templates.SingleOrDefault(p => p.Id == template.Id);
        //    if (templateFromDb == null) return HttpNotFound();

        //    var fieldsToRemove = templateFromDb.FieldsInTemplate.ToList();

        //    foreach (var field in fieldsToRemove)
        //    {
        //        var deletefield = true;
        //        foreach (var fields in template.FieldsInTemplate)
        //        {
        //            if (field.FieldId == fields.FieldId) deletefield = false;
        //        }
        //        if (deletefield) _db.FieldsInTemplates.Remove(field);
        //    }

        //    foreach (var fields in template.FieldsInTemplate)
        //    {
        //        var field = templateFromDb.FieldsInTemplate.SingleOrDefault(p => p.FieldId == fields.FieldId);
        //        if (field != null)
        //        {
        //            field.DefaultValue = fields.DefaultValue;
        //        }
        //        else
        //        {
        //            fields.Field = _db.Fields.SingleOrDefault(p => p.Id == fields.FieldId);
        //            templateFromDb.FieldsInTemplate.Add(fields);
        //        }
        //    }

        //    templateFromDb.Name = template.Name;

        //    _db.SaveChanges();
        //    answer = Answer.Edited;

        //    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
        //}

        [HttpPut]
        public ActionResult EditTemplate([ModelBinder(typeof(JsonNetModelBinder))] Template template)
        {
            Answer answer;

            if (!TemplatesValidator.TemplateIsNotNull(template))
            {
                answer = Answer.WrongTemplate;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }

            if (!TemplatesValidator.TemplateNameIsCorrect(template.Name))
            {
                answer = Answer.WrongName;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }

            if (!TemplatesValidator.FieldsInTemplateIsNull(template.FieldsInTemplate))
            {
                answer = Answer.FieldsIsEmpty;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }

            if (!TemplatesValidator.FieldInFieldsInTemplateIsCorrect(template.FieldsInTemplate))
            {
                answer = Answer.FieldIsNotCorrect;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }

            var templateFromDb = _db.Templates.SingleOrDefault(t => t.Id == template.Id);

            if (templateFromDb == null)
            {
                answer = Answer.WrongId;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }

            var fieldsToDelete = templateFromDb.FieldsInTemplate;

            foreach (var fieldToDelete in fieldsToDelete)
            {
                var deleteField = true;
                foreach (var fieldFromTemplate in template.FieldsInTemplate)
                {
                    if (fieldToDelete.FieldId == fieldFromTemplate.FieldId)
                    {
                        deleteField = false;
                    }
                }
                if (deleteField)
                {
                    _db.FieldsInTemplates.Remove(fieldToDelete);
                }
            }

            foreach (var fields in template.FieldsInTemplate)
            {
                if (!_db.Fields.Any(f => f.Id == fields.FieldId))
                {
                    answer = Answer.FieldIsNotCorrect;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                }

                var field = templateFromDb.FieldsInTemplate.SingleOrDefault(f => f.FieldId == fields.FieldId);
                if (field != null)
                {
                    field.DefaultValue = fields.DefaultValue;
                }
                else
                {
                    fields.Field = _db.Fields.SingleOrDefault(f => f.Id == fields.FieldId);
                    templateFromDb.FieldsInTemplate.Add(fields);
                }
            }

            templateFromDb.Name = template.Name;

            _db.SaveChanges();
            answer = Answer.Edited;

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
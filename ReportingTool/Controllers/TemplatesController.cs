using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Newtonsoft.Json;
using ReportingTool.Core.Models;
using ReportingTool.Core.Services;
using ReportingTool.Core.Validation;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.DAL.Entities;
using ReportingTool.Models;
using System.Net;
using System.Web.Hosting;
using System.Data.Entity;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        private enum Answer
        {
            DBConnectionError, WrongTemplate, WrongName, WrongId,
            FieldsIsEmpty, FieldIsNotCorrect, Edited, AlreadyExists, WrongOwnerName, Added, IsNull,
            NotDeleted, Deleted, NotFound
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
            var fields = _db.Fields.AsNoTracking().Select(field => new FieldModel
            {
                fieldID = field.Id,
                fieldName = field.Name,
                fieldType = field.FieldType.Type,
                isSelected = false
            }).ToList();

            return JsonConvert.SerializeObject(fields, Formatting.Indented);
        }

        [HttpGet]
        public string GetAllTemplates()
        {
            var templates = _db.Templates.AsNoTracking().Where(template => template.IsActive)
                .Select(template => new TemplateModel { templateId = template.Id, templateName = template.Name }).ToList();

            return JsonConvert.SerializeObject(templates, Formatting.Indented);
        }

        [HttpPut]
        public ActionResult EditTemplate([ModelBinder(typeof(JsonNetModelBinder))] Template template)
        {
            Answer answer;

            //----- Move all validation to Core.Validation -----//

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

            if (!TemplatesValidator.FieldsInTemplateIsEmpty(template.FieldsInTemplate))
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

            using (var db = new DB2())
            {
                //Check if template with incoming name already exists in database
                if (db.Templates.Where(t => t.Name == template.Name).Count() > 0)
                {
                    answer = Answer.AlreadyExists;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
                }

                var currentUser = this.Session["currentUser"] as string;
                template.Owner = currentUser;
                template.IsActive = true;

                db.Templates.Add(template);
                db.SaveChanges();
                answer = Answer.Added;
                return Json(new { Answer = Enum.GetName(typeof(Answer), answer) });
            }
        }

        public string GetTemplateFields(int templateId)
        {
            List<FieldModel> fields;
            string temaplateName;
            bool isOwner;
            //using (var db = new DB2())
            //{
            var template = _db.Templates.FirstOrDefault(t => t.Id == templateId);
            if (template == null)
            {
                return JsonConvert.SerializeObject(Json(new { Answer = "False" }));
            }
            string templateOwner = template.Owner;
            isOwner = CheckIfCurrentUserIsOwnerOfTemplate(templateOwner);
            temaplateName = template.Name;
            var getFields = from filedsInTemplate in _db.FieldsInTemplates
                            join field in _db.Fields on filedsInTemplate.FieldId equals field.Id
                            where filedsInTemplate.TemplateId == templateId
                            select new FieldModel
                            {
                                fieldName = field.Name,
                                fieldDefaultValue = filedsInTemplate.DefaultValue,
                                fieldID = field.Id,
                                fieldType = field.FieldType.Type,
                                isSelected = true
                            };
            fields = getFields.ToList();
            //}
            TemplateModel templateData = new TemplateModel { fields = fields, IsOwner = isOwner, templateName = temaplateName };
            return JsonConvert.SerializeObject(templateData, Formatting.Indented);
        }

        private bool CheckIfCurrentUserIsOwnerOfTemplate(string templateOwner)
        {
            var currentUser = Session["currentUser"] as string;
            if (currentUser == null)
                return false;
            return currentUser.Equals(templateOwner);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing && _db != null)
            {
                _db.Dispose();
            }
            base.Dispose(disposing);
        }

        /// <summary>
        /// Delete a template with the specified id
        /// </summary>
        /// <param name="id">template id</param>
        /// <returns>result codes from - enum Answer</returns>
        [HttpDelete]
        //public HttpStatusCodeResult Delete(int id)
        public ActionResult DeleteTemplate(int id)
        {
            Answer answer = Answer.NotDeleted;

            using (var ctx = new DB2())
            {
                var item = ctx.Templates
                      .Include(t => t.FieldsInTemplate)
                     .FirstOrDefault<Template>(t => t.Id == id);

                if (item == null)
                {
                    //return new HttpStatusCodeResult(HttpStatusCode.NotFound, "Team is not found");
                    answer = Answer.NotFound;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
                }

                Template templateDelete = (Template)item;

                try
                {
                    FieldsInTemplate[] fitArray = templateDelete.FieldsInTemplate.ToArray<FieldsInTemplate>();

                    for (int i = fitArray.GetLowerBound(0), upper = fitArray.GetUpperBound(0); i <= upper; i++)
                    {
                        if (fitArray[i].TemplateId == id)
                        {
                            ctx.FieldsInTemplates.Remove(fitArray[i]);
                        }
                    }

                    templateDelete.IsActive = false;
                    ctx.SaveChanges();
                }
                catch
                {
                    //  return new HttpStatusCodeResult(HttpStatusCode.NotModified, "Template is not deleted");
                    answer = Answer.NotDeleted;
                    return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
                }
            }

            //  return new HttpStatusCodeResult(HttpStatusCode.OK, "Template deleted successfully");
            answer = Answer.Deleted;
            return Json(new { Answer = Enum.GetName(typeof(Answer), answer) }, JsonRequestBehavior.AllowGet);
        }

    }
}

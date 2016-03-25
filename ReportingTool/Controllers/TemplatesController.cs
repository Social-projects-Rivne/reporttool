using System;
using System.Data.Entity;
using System.Linq;
using System.Web.Mvc;
using Newtonsoft.Json;
using ReportingTool.Core.Models;
using ReportingTool.Core.Validation;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        private enum Answer { WrongId, FieldIsNotCorrect, Edited, AlreadyExists, Added, NotDeleted, Deleted };

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

        [HttpPost]
        public ActionResult AddNewTemplate([ModelBinder(typeof(JsonNetModelBinder))] Template newTemplate)
        {
            var validation = newTemplate.TemplateValidForAdd();
            if (validation != null) return Json(new { Answer = validation });
            if (newTemplate.FieldsInTemplate.Any(field => field.FieldId == 1 && JiraUsersController.UsersStorage.FirstOrDefault(user => user.displayName == field.DefaultValue) == null))
                return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.FieldIsNotCorrect) });
            //if ()
            //return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.FieldIsNotCorrect) });
            using (var db = new DB2())
            {
                var template = db.Templates.FirstOrDefault(t => t.Name == newTemplate.Name);
                if (template == null)
                {
                    template = new Template
                    {
                        Name = newTemplate.Name,
                        Owner = Session["currentUser"] as string
                    };
                    db.Templates.Add(template);
                }
                else if (template.IsActive)
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.AlreadyExists) });

                template.FieldsInTemplate = newTemplate.FieldsInTemplate;
                template.IsActive = true;

                db.SaveChanges();
                return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.Added) });
            }
        }

        [HttpPut]
        public ActionResult EditTemplate([ModelBinder(typeof(JsonNetModelBinder))] Template template)
        {
            var validation = template.TemplateValidForEdit();
            if (validation != null) return Json(new { Answer = validation });

            if (template.FieldsInTemplate.Any(field => field.FieldId == 1 && JiraUsersController.UsersStorage.FirstOrDefault(user => user.displayName == field.DefaultValue) == null))
                return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.FieldIsNotCorrect) });

            var templateFromDb = _db.Templates.FirstOrDefault(t => t.Id == template.Id && t.IsActive);
            if (templateFromDb.TemplateIsNotNull() != null) return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.WrongId) });
            if (_db.Templates.Any(t => t.Id != templateFromDb.Id && t.Name == template.Name && t.IsActive)) return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.AlreadyExists) });

            _db.FieldsInTemplates.RemoveRange(templateFromDb.FieldsInTemplate);

            foreach (var field in template.FieldsInTemplate)
            {
                if (!_db.Fields.Any(f => f.Id == field.FieldId)) { return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.FieldIsNotCorrect) }); }
                templateFromDb.FieldsInTemplate.Add(field);
            }
            templateFromDb.Name = template.Name;
            _db.SaveChanges();

            return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.Edited) });
        }

        public string GetTemplateFields(int templateId)
        {
            var template = _db.Templates.FirstOrDefault(t => t.Id == templateId && t.IsActive);
            if (template.TemplateIsNotNull() != null) return JsonConvert.SerializeObject(Json(new { Answer = "False" }));

            var templateOwner = template.Owner;
            var isOwner = CheckIfCurrentUserIsOwnerOfTemplate(templateOwner);
            var temaplateName = template.Name;
            var fields = _db.FieldsInTemplates.AsNoTracking().Include("Fields").Where(t => t.TemplateId == templateId)
                .Select(field => new FieldModel
                {
                    fieldName = field.Field.Name,
                    fieldDefaultValue = field.DefaultValue,
                    fieldID = field.FieldId,
                    fieldType = field.Field.FieldType.Type,
                    isSelected = true
                }).ToList();

            var templateData = new TemplateModel { fields = fields, IsOwner = isOwner, templateName = temaplateName };

            return JsonConvert.SerializeObject(templateData, Formatting.Indented);
        }

        private bool CheckIfCurrentUserIsOwnerOfTemplate(string templateOwner)
        {
            var currentUser = Session["currentUser"] as string;
            if (currentUser == null)
                return false;
            return currentUser.Equals(templateOwner);
        }

        /// <summary>
        /// Delete a template with the specified id
        /// </summary>
        /// <param name="id">template id</param>
        /// <returns>result codes from - enum Answer</returns>
        [HttpDelete]
        public ActionResult DeleteTemplate(int id)
        {
            using (var ctx = new DB2())
            {
                var templateDelete = ctx.Templates.Include(t => t.FieldsInTemplate).FirstOrDefault(t => t.Id == id);
                if (templateDelete == null)
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.WrongId) }, JsonRequestBehavior.AllowGet);

                try
                {
                    var fitArray = templateDelete.FieldsInTemplate.ToArray();

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
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.NotDeleted) }, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.Deleted) }, JsonRequestBehavior.AllowGet);
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

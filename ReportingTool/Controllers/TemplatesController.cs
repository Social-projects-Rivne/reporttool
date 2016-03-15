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

        [HttpPut]
        public ActionResult EditTemplate([ModelBinder(typeof(JsonNetModelBinder))] Template template)
        {
            var validation = template.TemplateValidForEdit();
            if (validation != null) return Json(new { Answer = validation });

            var templateFromDb = _db.Templates.FirstOrDefault(t => t.Id == template.Id && t.IsActive);
            if (templateFromDb.TemplateIsNotNull() != null) return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.WrongId) });

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

        [HttpPost]
        public ActionResult AddNewTemplate([ModelBinder(typeof(JsonNetModelBinder))] Template template)
        {
            var validation = template.TemplateValidForAdd();
            if (validation != null) return Json(new { Answer = validation });

            using (var db = new DB2())
            {
                var templateFromDb = db.Templates.FirstOrDefault(t => t.Name == template.Name);
                if (templateFromDb.IsActive)
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.AlreadyExists) });
                if (!templateFromDb.IsActive) template = templateFromDb;
                if (templateFromDb == null) db.Templates.Add(template);
                //Check if template with incoming name already exists in database
                var currentUser = Session["currentUser"] as string;
                template.Owner = currentUser;
                template.IsActive = true;

                
                db.SaveChanges();
                return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.Added) });
            }
        }

        public string GetTemplateFields(int templateId)
        {
            var template = _db.Templates.FirstOrDefault(t => t.Id == templateId && t.IsActive);
            if (template.TemplateIsNotNull() != null)
            {
                return JsonConvert.SerializeObject(Json(new { Answer = "False" }));
            }
            var templateOwner = template.Owner;
            var isOwner = CheckIfCurrentUserIsOwnerOfTemplate(templateOwner);
            var temaplateName = template.Name;
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
            var fields = getFields.ToList();

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
                if (templateDelete.TemplateIsNotNull() != null)
                {
                    return Json(new { Answer = Enum.GetName(typeof(Answer), Answer.WrongId) }, JsonRequestBehavior.AllowGet);
                }
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

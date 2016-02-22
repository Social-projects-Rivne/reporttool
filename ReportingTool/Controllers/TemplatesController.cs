using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using ReportingTool.DAL.Entities;
using ReportingTool.Core.Validation;
﻿using Newtonsoft.Json;
﻿using ReportingTool.Models;

using System.Net;
using System.Web.Hosting;
using System.Data.Entity;

namespace ReportingTool.Controllers
{
    public class TemplatesController : Controller
    {
        private enum Answer { AlreadyExists, WrongName, WrongOwnerName, Added, IsNull, NotFound, NotDeleted, Deleted };

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
        public string GetTemplateFields(int templateId)
        {
            List<TemplateFieldsDataModel> fields;
            string owner, temaplateName;
            using (var db = new DB2())
            {
                var template = db.Templates.FirstOrDefault(t => t.Id == templateId);
                if (template == null)
                {
                    return JsonConvert.SerializeObject(Json(new { Answer = "False" }));
                }
                owner = template.Owner;
                temaplateName = template.Name;
                var getFields = from filedsInTemplate in db.FieldsInTemplates
                                join field in db.Fields on filedsInTemplate.FieldId equals field.Id
                                where filedsInTemplate.TemplateId == templateId
                                select new TemplateFieldsDataModel { FieldName = field.Name, DefaultValue = filedsInTemplate.DefaultValue };
                fields = getFields.ToList();
            }
            TemplateData templateData = new TemplateData { Fields = fields, Owner = owner, TemplateName = temaplateName };
            return JsonConvert.SerializeObject(templateData, Formatting.Indented);
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

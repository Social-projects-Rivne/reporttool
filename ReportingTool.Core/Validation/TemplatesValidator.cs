using System;
using System.Collections.Generic;
using System.Linq;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Core.Validation
{
    public static class TemplatesValidator
    {
        public static bool TemplateIsCorrect(Template template)
        {
            if (template == null)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool TemplateNameIsCorrect(string templateName)
        {
            if (String.IsNullOrWhiteSpace(templateName) || templateName.Length > 128)
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        public static bool TemplateOwnerNameIsCorrect(string owner)
        {
            if (String.IsNullOrWhiteSpace(owner) || owner.Length > 128)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool IfTemplateIdExists(int templateId)
        {
            using (var db = new DB2())
            {
                if (db.Templates.Any(p=> p.Id == templateId))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            
        }
        public static bool FieldsInTemplateIsNull(ICollection<FieldsInTemplate> fieldsInTemplate)
        {
            if (fieldsInTemplate == null || fieldsInTemplate.Count == 0)
            {
                return false;
            }
            else
            {
                return true;
            }
        }
        public static bool FieldInFieldsInTemplateIsCorrect(ICollection<FieldsInTemplate> fieldsInTemplate)
        {
            bool isnull = false;
            foreach (var field in fieldsInTemplate)
            {
                using (var db = new DB2())
                {
                if (field == null || field.FieldId == 0 || db.Fields.Any(p => p.Id == field.FieldId))
                {
                    isnull = false;
                }
                else
                {
                    isnull =  true;
                }
                }
            }
            return isnull;
        }
    }


}

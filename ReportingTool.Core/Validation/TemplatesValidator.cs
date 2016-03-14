using System.Collections.Generic;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Core.Validation
{
    public static class TemplatesValidator
    {
        public static bool TemplateIsNotNull(Template template)
        {
            return template != null;
        }

        public static bool TemplateNameIsCorrect(string templateName)
        {
            return !string.IsNullOrWhiteSpace(templateName) && templateName.Length <= 128;
        }

        public static bool TemplateOwnerNameIsCorrect(string owner)
        {
            return !string.IsNullOrWhiteSpace(owner) && owner.Length <= 128;
        }

        public static bool FieldsInTemplateIsEmpty(ICollection<FieldsInTemplate> fieldsInTemplate)
        {
            return fieldsInTemplate != null && fieldsInTemplate.Count != 0;
        }
        
        public static bool FieldInFieldsInTemplateIsCorrect(ICollection<FieldsInTemplate> fieldsInTemplate)
        {
            var isnull = false;
            foreach (var field in fieldsInTemplate)
            {
                if (field == null || field.FieldId == 0)
                {
                    isnull = false;
                }
                else
                {
                    isnull = true;
                }
            }
            return isnull;
        }


    }


}

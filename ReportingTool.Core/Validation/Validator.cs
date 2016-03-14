using System.Linq;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Core.Validation
{
    public static class Validator
    {
        public static string TemplateValidation(this Template template)
        {
            if (template == null) return "WrongTemplate";
            if (string.IsNullOrWhiteSpace(template.Name) || template.Name.Length > 128)
                return "WrongName";
            if (template.FieldsInTemplate == null || template.FieldsInTemplate.Count == 0)
                return "FieldsIsEmpty";
            if (template.FieldsInTemplate.Any(field => field == null || field.FieldId == 0))
                return "FieldIsNotCorrect";
            return null;
        }

        public static string TemplateValidForEdit(this Template template)
        {
            if (template.TemplateIsNotNull() != null) { return template.TemplateIsNotNull(); }
            if (template.TemplateNameIsCorrect() != null) { return template.TemplateNameIsCorrect(); }
            if (template.FieldsInTemplateIsEmpty() != null) { return template.FieldsInTemplateIsEmpty(); }
            if (template.FieldInFieldsInTemplateIsCorrect() != null) { return template.FieldInFieldsInTemplateIsCorrect(); }
            return null;
        }

        public static string TemplateIsNotNull(this Template template)
        {
            return template == null ? "WrongTemplate" : null;
        }
        public static string TemplateNameIsCorrect(this Template template)
        {
            return (string.IsNullOrWhiteSpace(template.Name) || template.Name.Length > 128) ? "WrongName" : null;
        }
        public static string TemplateOwnerNameIsCorrect(this Template template)
        {
            return (string.IsNullOrWhiteSpace(template.Owner) || template.Owner.Length > 128) ? "WrongOwnerName" : null;
        }
        public static string FieldsInTemplateIsEmpty(this Template template)
        {
            return (template.FieldsInTemplate == null || template.FieldsInTemplate.Count == 0) ? "FieldsIsEmpty" : null;
        }
        public static string FieldInFieldsInTemplateIsCorrect(this Template template)
        {
            return (template.FieldsInTemplate.Any(field => field == null || field.FieldId == 0)) ? "FieldIsNotCorrect" : null;
        }
    }
}

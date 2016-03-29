using System.Linq;
using ReportingTool.DAL.Entities;
namespace ReportingTool.Core.Validation
{
    public static class Validator
    {
        public static string TemplateValidForAdd(this Template template)
        {
            if (template.TemplateIsNotNull() != null) { return template.TemplateIsNotNull(); }
            if (template.TemplateNameIsCorrect() != null) { return template.TemplateNameIsCorrect(); }
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

        public static string TeamValid(this Team team)
        {
            if (team.TeamIsNotNull() != null) { return team.TeamIsNotNull(); }
            if (team.TeamNameIsCorrect() != null) { return team.TeamNameIsCorrect(); }
            if (team.TeamMembersIsEmpty() != null) { return team.TeamMembersIsEmpty(); }
            if (team.MembersIsNotNull() != null) { return team.MembersIsCorrect(); }
            if (team.MembersIsCorrect() != null) { return team.MembersIsCorrect(); }
            return null;
        }

        public static string TeamIsNotNull(this Team team)
        {
            return team == null ? "WrongTeam" : null;
        }
        public static string TeamNameIsCorrect(this Team team)
        {
            return (string.IsNullOrWhiteSpace(team.Name) || team.Name.Length > 128) ? "WrongName" : null;
        }
        public static string TeamMembersIsEmpty(this Team team)
        {
            return (team.Members == null || team.Members.Count == 0) ? "MembersIsEmpty" : null;
        }
        public static string MembersIsNotNull(this Team team)
        {
            return (team.Members.Any(member => member == null)) ? "MembersIsNull" : null;
        }
        public static string MembersIsCorrect(this Team team)
        {
            return (team.Members.Any(member => string.IsNullOrWhiteSpace(member.UserName) || string.IsNullOrWhiteSpace(member.FullName))) ? "MembersIsNotCorrect" : null;
        }

            }
}

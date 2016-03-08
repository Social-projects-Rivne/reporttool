using System.Collections.Generic;

namespace ReportingTool.Core.Models
{
    public class TemplateModel
    {
        public int templateId { get; set; }
        public string templateName { get; set; }
        public List<FieldModel> fieldsInTemplate;
    }
}

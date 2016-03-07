using System.Collections.Generic;

namespace ReportingTool.Core.Models
{
    public class TemplateDTO
    {
        public int templateId { get; set; }
        public string templateName { get; set; }
        public List<FieldDTO> fieldsInTemplate;
    }
}

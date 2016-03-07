using System.Collections.Generic;

namespace ReportingTool.DAL.Models
{
    public class TemplateDTO
    {
        public int templateId { get; set; }
        public string templateName { get; set; }
        public ICollection<FieldDTO> fieldsInTemplate;
    }
}

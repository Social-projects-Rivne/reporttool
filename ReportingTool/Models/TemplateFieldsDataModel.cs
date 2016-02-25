using System.Collections.Generic;

namespace ReportingTool.Models
{
    public class TemplateFieldsDataModel
    {
        public string FieldName { get; set; }

        public string DefaultValue { get; set; }
    }

    public class TemplateData
    {
        public List<TemplateFieldsDataModel> Fields { get; set; }

        public string Owner { get; set; }

        public string TemplateName { get; set; }
    }
}
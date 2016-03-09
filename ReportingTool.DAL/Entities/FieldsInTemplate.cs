using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
    public class FieldsInTemplate
    {
        [JsonIgnore]
        public int Id { get; set; }

        [JsonProperty("defaultValue")]
        public string DefaultValue { get; set; }

        [ForeignKey("Field")]
        [JsonProperty("fieldId")]
        public int FieldId { get; set; }

        [ForeignKey("Template")]
        [JsonProperty("templateId")]
        public int TemplateId { get; set; }

        public virtual Field Field { get; set; }

        public virtual Template Template { get; set; }
    }
}
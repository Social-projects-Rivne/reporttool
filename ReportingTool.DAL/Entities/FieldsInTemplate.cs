using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
    public class FieldsInTemplate
    {
        public int Id { get; set; }
        [Required]
        [JsonProperty("isDefault")]
        public bool IsDefault { get; set; }
        public virtual ICollection<Field> Fields { get; set; }
        public virtual ICollection<Template> Templates { get; set; }
    }
}
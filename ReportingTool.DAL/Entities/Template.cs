using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
    public class Template
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonProperty("templateName")]
        public string Name { get; set; }

        [Required]
        [JsonProperty("isActive")]
        public bool IsActive { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonProperty("owner")]
        public string Owner { get; set; }

        public virtual ICollection<FieldsInTemplate> FieldsInTemplate { get; set; }
    }
}
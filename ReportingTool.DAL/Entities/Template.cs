using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportingTool.DAL.Entities
{
    public class Template
    {

        [JsonProperty("id")]
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

        [JsonProperty("fieldsInTemplate")]
        public virtual ICollection<FieldsInTemplate> FieldsInTemplate { get; set; }
    }
}
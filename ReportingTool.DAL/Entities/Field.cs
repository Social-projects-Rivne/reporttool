using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
    public class Field
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonProperty("fieldName")]
        public string Name { get; set; }

        public virtual ICollection<FieldsInTemplate> FieldsInTemplate { get; set; }
    }
}
using System.ComponentModel.DataAnnotations;

namespace ReportingTool.DAL.Entities
{
    public class FieldType
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        public string Type { get; set; }
    }
}
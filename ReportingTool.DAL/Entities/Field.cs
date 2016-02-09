using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportingTool.DAL.Entities
{
    [Table("fields", Schema = "public")]
    public partial class Field
    {
        public Field()
        {
            this.FieldsInTemplates = new HashSet<FieldInTemplate>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MinLength(4)]
        [MaxLength(50)]
        public string Name { get; set; }

        public virtual ICollection<FieldInTemplate> FieldsInTemplates { get; set; }
    }
}

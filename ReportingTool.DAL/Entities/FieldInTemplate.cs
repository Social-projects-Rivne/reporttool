using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.DAL.Entities
{
    [Table("field_in_template", Schema = "public")]
    public partial class FieldInTemplate
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("template_id")]
        public int TemplateId { get; set; }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("field_id")]
        public int FieldId { get; set; }

        [Column("default")]
        [MinLength(4)]
        [MaxLength(50)]
        public string Default { get; set; }

        public virtual Field Field { get; set; }
        public virtual Template Template { get; set; }
    }
}

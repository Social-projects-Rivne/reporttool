�using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.DAL.Entities
{
    [Table("templates", Schema = "public")]
    public partial class Template
    {

        public Template()
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

        [Required]
        [Column("owner")]
        [MinLength(4)]
        [MaxLength(50)]
        public string Owner { get; set; }

        [Required]
        [Column("is_active")]
        public bool IsActive { get; set; }

        public virtual ICollection<FieldInTemplate> FieldsInTemplates { get; set; }
    }
}

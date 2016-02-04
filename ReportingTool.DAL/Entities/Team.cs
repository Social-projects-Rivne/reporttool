using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportingTool.DAL.Entities
{
    [Table("teams", Schema = "public")]
    public partial class team
    {
        public team()
        {
            this.members = new HashSet<member>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("id")]
        public int id { get; set; }

        [Required]
        [Column("name")]
        [MinLength(4)]
        [MaxLength(50)]
        public string name { get; set; }

        [Required]
        [Column("projectkey")]
        [MinLength(4)]
        [MaxLength(50)]
        public string projectkey { get; set; }

        [Required]
        [Column("isactive")]
        public bool isactive { get; set; }

        public virtual ICollection<member> members { get; set; }

    }
}


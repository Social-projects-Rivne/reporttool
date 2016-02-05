using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportingTool.DAL.Entities
{
    [Table("teams", Schema = "public")]
    public partial class Team
    {
        public Team()
        {
            this.Members = new HashSet<Member>();
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
        [Column("projectkey")]
        [MinLength(4)]
        [MaxLength(50)]
        public string ProjectKey { get; set; }

        [Required]
        [Column("isactive")]
        public bool IsActive { get; set; }

        public virtual ICollection<Member> Members { get; set; }

    }
}


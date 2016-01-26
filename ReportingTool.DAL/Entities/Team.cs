using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ReportingTool.DAL.Entities
{
  [Table("teams", Schema = "public")]
  public partial class Team
  {
    [Key]
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

    public virtual ICollection<TeamMember> TeamMembers { get; set; }

    public Team()
    {
      this.TeamMembers = new HashSet<TeamMember>();
    }

  }
}

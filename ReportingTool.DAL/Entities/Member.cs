using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportingTool.DAL.Entities
{
  [Table("members", Schema = "public")]
  public partial class Member
  {
    [Key]
    [Required]
    [Column("id")]
    public int Id { get; set; }

    [Required]
    [Column("username")]
    [MinLength(4)]
    [MaxLength(50)]
    public string Username { get; set; }

    [Required]
    [Column("fullname")]
    [MinLength(4)]
    [MaxLength(100)]
    public string Fullname { get; set; }

    [Required]
    [Column("isactive")]
    public bool IsActive { get; set; }

    public virtual ICollection<TeamMember> TeamMembers { get; set; }

    public Member()
    {
      this.TeamMembers = new HashSet<TeamMember>();
    }

  }
}

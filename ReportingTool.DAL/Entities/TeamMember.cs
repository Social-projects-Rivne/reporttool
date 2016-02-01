using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ReportingTool.DAL.Entities
{
  [Table("teams_member", Schema = "public")]
  public partial class TeamMember
  {
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("team_id")]
    public int TeamId { get; set; }

    [Column("member_id")]
    public int MemberId { get; set; }

    
    public virtual Member Members { get; set; }
    public virtual Team Teams { get; set; }

  }
}

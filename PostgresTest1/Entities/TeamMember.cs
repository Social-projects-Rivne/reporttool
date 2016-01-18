using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace PostgresTest1
{
  [Table("teams_member", Schema = "public")]
  public class TeamMember
  {
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("team_id")]
    public int TeamId { get; set; }

    [Column("member_id")]
    public int MemberId { get; set; }
  }

  //public class DB2 : DbContext
  //{
  //  public DB2() : base(nameOrConnectionString: "RTDB") { }
  //  public DbSet<TeamMember> TeamMembers { get; set; }
  //}

}
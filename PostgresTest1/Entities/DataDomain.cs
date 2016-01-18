using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace PostgresTest1
{
  public class DB2 : DbContext
  {
    public DB2() : base(nameOrConnectionString: "RTDB") { }

    public DbSet<Member> Members { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
  }

}

using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace PostgresTest1
{
  [Table("teams", Schema = "public")]
  public class Team
  {
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("name")]
    public string Name { get; set; }
    [Column("isactive")]
    public bool Isactive { get; set; }
  }

  //public class DB2 : DbContext
  //{
  //  public DB2() : base(nameOrConnectionString: "RTDB") { }
  //  public DbSet<Team> Teams { get; set; }
  //}

}
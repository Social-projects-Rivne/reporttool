using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;


namespace PostgresTest1
{
  [Table("members", Schema = "public")]
  public class Member
  {
    [Key]
    [Column("id")]
    public int Id { get; set; }
    [Column("username")]
    public string Username { get; set; }
    [Column("fullname")]
    public string Fullname { get; set; }
  }

  //public class DB2 : DbContext
  //{
  //  public DB2() : base(nameOrConnectionString: "RTDB") { }
  //  public DbSet<Member> Members { get; set; }
  //}

}

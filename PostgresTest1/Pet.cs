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
  [Table("pets", Schema = "public")]
  public class Pet
  {
    [Key]
    [Column("id")]
    public int ID { get; set; }
    [Column("name")]
    public string Name { get; set; }
  }

  public class DB : DbContext
  {
    public DB() : base(nameOrConnectionString: "MonkeyFist") { }
    public DbSet<Pet> Pets { get; set; }
  }

}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;


namespace ReportingTool.DAL.Entities
{

  public partial class DB2 : DbContext
  {
    public DB2() : base(nameOrConnectionString: "RTDB") { }

    //protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //{
    //  throw new UnintentionalCodeFirstException();
    //}

    public DbSet<Member> Members { get; set; }
    public DbSet<Team> Teams { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
  }

}
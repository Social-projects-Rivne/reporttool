using System.Data.Entity;

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
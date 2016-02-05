using System.Data.Entity;

namespace ReportingTool.DAL.Entities
{

  public partial class DB2 : DbContext
  {
    public DB2() : base(nameOrConnectionString: "RTDB") { }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        Database.SetInitializer<DB2>(null);

        modelBuilder.Entity<Team>()
                    .HasMany<Member>(t => t.Members)
                    .WithMany(m => m.Teams)
                    .Map(mt =>
                    {
                        mt.MapLeftKey("team_id");
                        mt.MapRightKey("member_id");
                        mt.ToTable("team_member");
                    });

        modelBuilder.HasDefaultSchema("public");
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Team> Teams { get; set; }
  }

}


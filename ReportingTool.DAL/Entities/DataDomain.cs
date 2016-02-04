using System.Data.Entity;

namespace ReportingTool.DAL.Entities
{

  public partial class DB2 : DbContext
  {
    public DB2() : base(nameOrConnectionString: "RTDB") { }

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        Database.SetInitializer<DB2>(null);

        modelBuilder.Entity<team>()
                    .HasMany<member>(t => t.members)
                    .WithMany(m => m.teams)
                    .Map(mt =>
                    {
                        mt.MapLeftKey("team_id");
                        mt.MapRightKey("member_id");
                        mt.ToTable("team_member");
                    });

        modelBuilder.HasDefaultSchema("public");
    }

    public DbSet<member> members { get; set; }
    public DbSet<team> teams { get; set; }
  }

}


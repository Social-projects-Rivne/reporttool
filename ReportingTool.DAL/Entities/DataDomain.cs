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

    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    {
        Database.SetInitializer<DB2>(null);
        //modelBuilder.Entity<team>().ToTable("teams");

        //modelBuilder.Entity<member>().ToTable("members");
        //modelBuilder.Entity<member>().ToTable("members", "public");

        modelBuilder.Entity<team>()
                    .HasMany<member>(t => t.members)
                    .WithMany(m => m.teams)
                    .Map(mt =>
                    {
                        mt.MapLeftKey("team_id");
                        mt.MapRightKey("member_id");
                        mt.ToTable("team_member");
                    });

        //throw new UnintentionalCodeFirstException();

        //Configure default schema
        //modelBuilder.HasDefaultSchema("Admin");
        modelBuilder.HasDefaultSchema("public");
    }

    public DbSet<member> members { get; set; }
    public DbSet<team> teams { get; set; }
    //public DbSet<TeamMember> TeamMembers { get; set; }
  }

}


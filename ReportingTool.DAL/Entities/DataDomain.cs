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
            //modelBuilder.Entity<Team>().ToTable("teams");

            //modelBuilder.Entity<Member>().ToTable("members");
            //modelBuilder.Entity<Member>().ToTable("members", "public");

            modelBuilder.Entity<Team>()
                        .HasMany<Member>(t => t.Members)
                        .WithMany(m => m.Teams)
                        .Map(mt =>
                        {
                            mt.MapLeftKey("team_id");
                            mt.MapRightKey("member_id");
                            mt.ToTable("team_member");
                        });

            //throw new UnintentionalCodeFirstException();

            //Configure default schema
            modelBuilder.HasDefaultSchema("public");
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Team> Teams { get; set; }
    }

}


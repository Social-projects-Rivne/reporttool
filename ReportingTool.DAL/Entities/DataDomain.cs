using System.Data.Entity;

namespace ReportingTool.DAL.Entities
{
    //  old working thing
    public partial class DB2 : DbContext
    {
        public DB2() : base(nameOrConnectionString: "RTDB") 
        {
           Database.SetInitializer<DB2>(new DatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            //Configure default schema
            modelBuilder.HasDefaultSchema("public");

            //Database.SetInitializer<DB2>(null);
            //changes by ohariv
            Database.SetInitializer<DB2>(new DatabaseInitializer());

            modelBuilder.Entity<Team>().ToTable("teams", "public");
            modelBuilder.Entity<Member>().ToTable("members", "public");

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
           
        }

        public DbSet<Member> Members { get; set; }
        public DbSet<Team> Teams { get; set; }
    }

    
    // changes by ohariv
    //public class DB2 : DbContext
    //{
    //    public DB2()
    //        : base(nameOrConnectionString: "RTDB")
    //    {
    //         //Database.SetInitializer<DB2>(new DatabaseInitializer());
    //    }

    //    protected override void OnModelCreating(DbModelBuilder modelBuilder)
    //    {
    //        modelBuilder.HasDefaultSchema("public");
    //    }

    //    public DbSet<Member> Members { get; set; }

    //    public DbSet<Team> Teams { get; set; }

    //    public DbSet<Field> Fields { get; set; }

    //    public DbSet<Template> Templates { get; set; }

    //    public DbSet<FieldsInTemplate> FieldsInTemplates { get; set; }
    //}

}
using System.Data.Entity;
using ReportingTool.DAL.DataAccessLayer;

namespace ReportingTool.DAL.Entities
{
    public class DB2 : DbContext, IDB2
    {
        public DB2()
            : base(nameOrConnectionString: "RTDB")
        {
            Database.SetInitializer<DB2>(new DatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.HasDefaultSchema("public");
        }

        public DbSet<Member> Members { get; set; }

        public DbSet<Team> Teams { get; set; }

        public DbSet<Template> Templates { get; set; }

        public DbSet<Field> Fields { get; set; }

        public DbSet<FieldsInTemplate> FieldsInTemplates { get; set; }

        public DbSet<FieldType> FieldTypes { get; set; }
    }
}


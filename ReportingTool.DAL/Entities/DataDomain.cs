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

        modelBuilder.Entity<FieldInTemplate>()
            .HasKey( c => new {  c.TemplateId, c.FieldId});

        modelBuilder.Entity<Field>()
            .HasMany(c => c.FieldsInTemplates)
            .WithRequired()
            .HasForeignKey(c => c.FieldId);

        modelBuilder.Entity<Template>()
                .HasMany(c => c.FieldsInTemplates)
                .WithRequired()
                .HasForeignKey(c => c.TemplateId);

        modelBuilder.HasDefaultSchema("public");
    }

    public DbSet<Member> Members { get; set; }
    public DbSet<Team> Teams { get; set; }

    public DbSet<Template> Templates { get; set; }
    public DbSet<Field> Fields { get; set; }
    public DbSet<FieldInTemplate> FieldsInTemplates { get; set; }
    }

}


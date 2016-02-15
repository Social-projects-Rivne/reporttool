﻿using System.Data.Entity;

namespace ReportingTool.DAL.Entities
{
    public class DB2 : DbContext
    {
        public DB2()
            : base(nameOrConnectionString: "RTDB")
        {
            Database.SetInitializer<DB2>(new DatabaseInitializer());
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
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
    public DbSet<FieldsInTemplate> FieldsInTemplates { get; set; }
    }

}


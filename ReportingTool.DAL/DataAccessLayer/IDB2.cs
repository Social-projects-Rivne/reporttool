using System;
using System.Data.Entity;
using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.DataAccessLayer
{
    public interface IDB2 : IDisposable
    {
        DbSet<Member> Members { get; }
        DbSet<Team> Teams { get; }
        DbSet<Template> Templates { get; }
        DbSet<Field> Fields { get; }
        DbSet<FieldsInTemplate> FieldsInTemplates { get; }
        int SaveChanges();
    }
}

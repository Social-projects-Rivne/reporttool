//namespace ReportingTool.DAL.Migrations
//{
//    using System;
//    using System.Data.Entity;
//    using System.Data.Entity.Migrations;
//    using System.Linq;

using System.Data.Entity.Migrations;

internal sealed class Configuration : DbMigrationsConfiguration<ReportingTool.DAL.Entities.DB2>
{
    public Configuration()
    {
        AutomaticMigrationsEnabled = true;
    }

    protected override void Seed(ReportingTool.DAL.Entities.DB2 context)
    {
        //  This method will be called after migrating to the latest version.

        //  You can use the DbSet<T>.AddOrUpdate() helper extension method 
        //  to avoid creating duplicate seed data. E.g.
        //
        //    context.People.AddOrUpdate(
        //      p => p.FullName,
        //      new Person { FullName = "Andrew Peters" },
        //      new Person { FullName = "Brice Lambson" },
        //      new Person { FullName = "Rowan Miller" }
        //    );
        //
    }
}


using System.Collections.Generic;
using System.Data.Entity;

namespace ReportingTool.DAL.Entities
{
    class DatabaseInitializer : CreateDatabaseIfNotExists<DB2>
    {
        protected override void Seed(DB2 context)
        {
            var defaultFieldValues = new List<Field>
            {
                new Field {Name = "Reporter"},
                new Field {Name = "Receiver"},
                new Field {Name = "UsualTasks"},
                new Field {Name = "RisksAndIssues"},
                new Field {Name = "PlannedActivities"},
                new Field {Name = "PlannedVacations"},
                new Field {Name = "UserActivities"},
                new Field {Name = "Reasons"}
            };
            foreach (var name in defaultFieldValues)
            {
                context.Fields.Add(name);
            }
            base.Seed(context);
        }
    }
}
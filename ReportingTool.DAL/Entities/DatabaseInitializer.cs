using System.Data.Entity;

namespace ReportingTool.DAL.Entities
{
    class DatabaseInitializer : CreateDatabaseIfNotExists<DB2>
    {
        protected override void Seed(DB2 context)
        {

            var fieldTypeComboBox = context.FieldTypes.Add(new FieldType { Type = "ComboBox" });
            var fieldTypeText = context.FieldTypes.Add(new FieldType { Type = "Text" });
            var fieldTypeListBox = context.FieldTypes.Add(new FieldType { Type = "ListBox" });
            var fieldTypeDate = context.FieldTypes.Add(new FieldType { Type = "Date" });
            context.SaveChanges();

            context.Fields.Add(new Field { Name = "Reporter", FieldType = fieldTypeComboBox });
            context.Fields.Add(new Field { Name = "Receiver", FieldType = fieldTypeComboBox });
            context.Fields.Add(new Field { Name = "UsualTasks", FieldType = fieldTypeText });
            context.Fields.Add(new Field { Name = "RisksAndIssues", FieldType = fieldTypeListBox });
            context.Fields.Add(new Field { Name = "PlannedActivities", FieldType = fieldTypeListBox });
            context.Fields.Add(new Field { Name = "PlannedVacations", FieldType = fieldTypeListBox });
            context.Fields.Add(new Field { Name = "UserActivities", FieldType = fieldTypeListBox });
            context.Fields.Add(new Field { Name = "Reasons", FieldType = fieldTypeText });
            base.Seed(context);
        }
    }
}
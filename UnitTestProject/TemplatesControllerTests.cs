using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingTool.Controllers;
using ReportingTool.DAL.Entities;


namespace UnitTestProject
{
    [TestClass]
    class TemplatesControllerTests
    {
        [TestMethod]
        public void GetAllTemplates_and_GetTemplateFields_Testing()
        {
            TemplatesController controller = new TemplatesController();
            Template testTemplate = new Template { Id = 999, Name = "TestTemplate", IsActive = true, Owner = "testowner" };
            FieldsInTemplate testfield1 = new FieldsInTemplate
            {
                Id = 998,
                DefaultValue = "testvalue",
                FieldId = 1,
                TemplateId = 999
            };
            FieldsInTemplate testfield2 = new FieldsInTemplate
            {
                Id = 999,
                DefaultValue = "testvalue",
                FieldId = 1,
                TemplateId = 999
            };
            TemplateFieldsDataModel testFieldsDataModel1 = new TemplateFieldsDataModel
            {
                DefaultValue = testfield1.DefaultValue,
                FieldName = testfield1.Field.Name
            };
            TemplateFieldsDataModel testFieldsDataModel2 = new TemplateFieldsDataModel
            {
                DefaultValue = testfield2.DefaultValue,
                FieldName = testfield2.Field.Name
            };

            List<TemplateFieldsDataModel> fields = new List<TemplateFieldsDataModel>
            {
                testFieldsDataModel1,
                testFieldsDataModel2
            };
            TemplateData testresult = new TemplateData
            {
                Fields = fields,
                Owner = testTemplate.Owner,
                TemplateName = testTemplate.Name
            };
            using (var db = new DB2())
            {
                db.Templates.Add(testTemplate);
                db.FieldsInTemplates.Add(testfield1);
                db.FieldsInTemplates.Add(testfield2);
                db.SaveChanges();
            }
            var expected = JsonConvert.SerializeObject(testresult, Formatting.Indented);
            var actual = controller.GetTemplateFields(999);
            Assert.AreEqual(expected, actual);
            using (var db = new DB2())
            {
                db.Templates.Remove(testTemplate);
                db.FieldsInTemplates.Remove(testfield1);
                db.FieldsInTemplates.Remove(testfield2);
                db.SaveChanges();
            }
        }
    }
}

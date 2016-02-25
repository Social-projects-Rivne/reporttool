using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ReportingTool.Controllers;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.DAL.Entities;
using ReportingTool.Models;

namespace UnitTestProject
{
    [TestClass]
    public class TemplateTests
    {
        [TestMethod]
        public void GetAllTemplates_Testing()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate1 = new Template { Id = 1, Name = "TestTemplate1", IsActive = true, Owner = "testowner" };
            Template testTemplate2 = new Template { Id = 2, Name = "TestTemplate2", IsActive = false, Owner = "testowner" };
            Template testTemplate3 = new Template { Id = 3, Name = "TestTemplate3", IsActive = true, Owner = "testowner" };
            Template testTemplate4 = new Template { Id = 4, Name = "TestTemplate4", IsActive = false, Owner = "testowner" };

            db.Templates.Add(testTemplate1);
            db.Templates.Add(testTemplate2);
            db.Templates.Add(testTemplate3);
            db.Templates.Add(testTemplate4);

            List<Template> testresult = new List<Template>
            {
                new Template {Name = testTemplate1.Name, Id = testTemplate1.Id},
                new Template {Name = testTemplate3.Name, Id = testTemplate3.Id},
            };

            var expected = JsonConvert.SerializeObject(testresult, Formatting.Indented);
            var actual = controller.GetAllTemplates();
            Assert.AreEqual(expected, actual);
            
        }

        [TestMethod]
        public void GetTemplatesFields_Testing()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate", IsActive = true, Owner = "testowner" };
            db.Templates.Add(testTemplate);

            Field field1 = new Field { Id = 1, Name = "testfield1" };
            Field field2 = new Field { Id = 2, Name = "testfield2" };
            db.Fields.Add(field1);
            db.Fields.Add(field2);

            FieldsInTemplate testfield1 = new FieldsInTemplate { Id = 1, DefaultValue = "testvalue1", FieldId = 1,
                TemplateId = 1 };
            FieldsInTemplate testfield2 = new FieldsInTemplate { Id = 2, DefaultValue = "testvalue2", FieldId = 2,
                TemplateId = 1 };
            db.FieldsInTemplates.Add(testfield1);
            db.FieldsInTemplates.Add(testfield2);

            TemplateFieldsDataModel testFieldsDataModel1 = new TemplateFieldsDataModel
            {
                DefaultValue = testfield1.DefaultValue,
                FieldName = field1.Name
            };
            TemplateFieldsDataModel testFieldsDataModel2 = new TemplateFieldsDataModel
            {
                DefaultValue = testfield2.DefaultValue,
                FieldName = field2.Name
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

            var expected = JsonConvert.SerializeObject(testresult, Formatting.Indented);
            var actual = controller.GetTemplateFields(1);
            Assert.AreEqual(expected, actual);
        }
    }
}

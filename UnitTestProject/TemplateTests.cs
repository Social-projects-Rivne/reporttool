using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ReportingTool.Controllers;
using ReportingTool.Core.Services;
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
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate1 = new Template { Id = 1, Name = "TestTemplate1", IsActive = true, Owner = "testowner" };
            var testTemplate2 = new Template { Id = 2, Name = "TestTemplate2", IsActive = false, Owner = "testowner" };
            var testTemplate3 = new Template { Id = 3, Name = "TestTemplate3", IsActive = true, Owner = "testowner" };
            var testTemplate4 = new Template { Id = 4, Name = "TestTemplate4", IsActive = false, Owner = "testowner" };

            db.Templates.Add(testTemplate1);
            db.Templates.Add(testTemplate2);
            db.Templates.Add(testTemplate3);
            db.Templates.Add(testTemplate4);

            var testresult = new List<Template>
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
            var db = new FakeDb();
            var controller = new TemplatesController(db);
            SessionHelper.Context = MockHelper.GetFakeHttpContext();
            var testTemplate = new Template { Id = 1, Name = "TestTemplate", IsActive = true, Owner = "testUser" };
            db.Templates.Add(testTemplate);

            var field1 = new Field { Id = 1, Name = "testfield1" };
            var field2 = new Field { Id = 2, Name = "testfield2" };
            db.Fields.Add(field1);
            db.Fields.Add(field2);

            var testfield1 = new FieldsInTemplate
            {
                Id = 1,
                DefaultValue = "testvalue1",
                FieldId = 1,
                TemplateId = 1
            };
            var testfield2 = new FieldsInTemplate
            {
                Id = 2,
                DefaultValue = "testvalue2",
                FieldId = 2,
                TemplateId = 1
            };
            db.FieldsInTemplates.Add(testfield1);
            db.FieldsInTemplates.Add(testfield2);

            var testFieldsDataModel1 = new TemplateFieldsDataModel
            { DefaultValue = testfield1.DefaultValue, FieldName = field1.Name };
            var testFieldsDataModel2 = new TemplateFieldsDataModel
            { DefaultValue = testfield2.DefaultValue, FieldName = field2.Name };

            var fields = new List<TemplateFieldsDataModel>
                { testFieldsDataModel1, testFieldsDataModel2 };
            var testresult = new TemplateData
            { Fields = fields, IsOwner = true, TemplateName = testTemplate.Name };

            var expected = JsonConvert.SerializeObject(testresult, Formatting.Indented);
            var actual = controller.GetTemplateFields(1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EditTemplate_Tests_IfTemplateIsNotNull()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var expected = new JsonResult { Data = (new { Answer = "WrongTemplate" }) };
            var actual = (JsonResult)controller.EditTemplate(null);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfTemplateNameIsCorrect_WhenNameIsWhiteSpace()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            db.Templates.Add(testTemplate);

            testTemplate.Name = " ";

            var expected = new JsonResult { Data = (new { Answer = "WrongName" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfTemplateNameIsCorrect_WhenNameIsNull()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            db.Templates.Add(testTemplate);

            testTemplate.Name = null;

            var expected = new JsonResult { Data = (new { Answer = "WrongName" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfTemplateNameIsCorrect_WhenLengthIsMoreThen()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            db.Templates.Add(testTemplate);

            testTemplate.Name = "012345678901234567890123456789012345678901234567890123456789" +
                                "0123456789012345678901234567890123456789012345678901234567890123456789";

            var expected = new JsonResult { Data = (new { Answer = "WrongName" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldsInTemplateIsNull()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate = new Template { Id = 1, Name = "TestTemplate", FieldsInTemplate = null };
            db.Templates.Add(testTemplate);

            var expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldsInTemplateIsCount()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate = new Template { Id = 1, Name = "TestTemplate", FieldsInTemplate = { } };
            db.Templates.Add(testTemplate);

            var expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldInFieldsInTemplateIsCorrect_WhenFieldIsNull()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            db.FieldsInTemplates.Add(null);
            db.Templates.Add(testTemplate);

            JsonResult expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldInFieldsInTemplateIsCorrect_WhenFieldIdIsZero()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            var field = new FieldsInTemplate { FieldId = 0 };

            db.FieldsInTemplates.Add(field);
            db.Templates.Add(testTemplate);

            var expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldInFieldsInTemplateIsCorrect_WhenFieldIdIsNotExists()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            var field = new FieldsInTemplate { FieldId = 2 };
            var newField = new Field { Id = 1 };

            db.FieldsInTemplates.Add(field);
            db.Fields.Add(newField);
            db.Templates.Add(testTemplate);

            var expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfTemplateIdExists()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate1 = new Template { Id = 1 };
            var testTemplate2 = new Template { Id = 2 };
            var testTemplate3 = new Template { Id = 3 };
            var testTemplate4 = new Template { Id = 4 };

            db.Templates.Add(testTemplate1);
            db.Templates.Add(testTemplate2);
            db.Templates.Add(testTemplate3);
            db.Templates.Add(testTemplate4);

            var testTemplate5 = new Template
            {
                Id = 5,
                Name = "TestTemplate",
                FieldsInTemplate = new List<FieldsInTemplate>()
            };
            testTemplate5.FieldsInTemplate.Add(new FieldsInTemplate { FieldId = 1 });


            var expected = new JsonResult { Data = (new { Answer = "WrongId" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate5);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Testing()
        {
            var db = new FakeDb();
            var controller = new TemplatesController(db);

            var testTemplate = new Template { Id = 1, Name = "TestTemplate", IsActive = true, Owner = "testowner1", FieldsInTemplate = new List<FieldsInTemplate>()};
            db.Templates.Add(testTemplate);
            var newField1 = new Field { Id = 1, Name = "testfield1" };
            var newField2 = new Field { Id = 2, Name = "testfield2" };
            var newField3 = new Field { Id = 3, Name = "testfield3" };

            db.Fields.Add(newField1);
            db.Fields.Add(newField2);
            db.Fields.Add(newField3);

            var field1 = new FieldsInTemplate
            {
                Id = 1,
                FieldId = 1,
                TemplateId = 1,
                DefaultValue = "testvalue1"
            };
            var field2 = new FieldsInTemplate
            {
                Id = 2,
                FieldId = 2,
                TemplateId = 1,
                DefaultValue = "testvalue2"
            };
            var field3 = new FieldsInTemplate
            {
                Id = 3,
                FieldId = 3,
                TemplateId = 1,
                DefaultValue = "testvalue3"
            };
            testTemplate.FieldsInTemplate.Add(field1);
            testTemplate.FieldsInTemplate.Add(field2);
            testTemplate.FieldsInTemplate.Add(field3);
            var expected = new JsonResult { Data = (new { Answer = "Edited" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }
    }
}

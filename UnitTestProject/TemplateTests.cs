﻿using System.Collections.Generic;
using System.Web.Mvc;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Newtonsoft.Json;
using ReportingTool.Controllers;
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

            FieldsInTemplate testfield1 = new FieldsInTemplate
            {
                Id = 1,
                DefaultValue = "testvalue1",
                FieldId = 1,
                TemplateId = 1
            };
            FieldsInTemplate testfield2 = new FieldsInTemplate
            {
                Id = 2,
                DefaultValue = "testvalue2",
                FieldId = 2,
                TemplateId = 1
            };
            db.FieldsInTemplates.Add(testfield1);
            db.FieldsInTemplates.Add(testfield2);

            TemplateFieldsDataModel testFieldsDataModel1 = new TemplateFieldsDataModel
            { DefaultValue = testfield1.DefaultValue, FieldName = field1.Name };
            TemplateFieldsDataModel testFieldsDataModel2 = new TemplateFieldsDataModel
            { DefaultValue = testfield2.DefaultValue, FieldName = field2.Name };

            List<TemplateFieldsDataModel> fields = new List<TemplateFieldsDataModel>
                { testFieldsDataModel1, testFieldsDataModel2 };
            TemplateData testresult = new TemplateData
            { Fields = fields, Owner = testTemplate.Owner, TemplateName = testTemplate.Name };

            var expected = JsonConvert.SerializeObject(testresult, Formatting.Indented);
            var actual = controller.GetTemplateFields(1);
            Assert.AreEqual(expected, actual);
        }

        [TestMethod]
        public void EditTemplate_Tests_IfTemplateIsNotNull()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = null;

            JsonResult expected = new JsonResult { Data = (new { Answer = "WrongTemplate" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfTemplateNameIsCorrect_WhenNameIsWhiteSpace()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            db.Templates.Add(testTemplate);

            testTemplate.Name = " ";

            JsonResult expected = new JsonResult { Data = (new { Answer = "WrongName" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfTemplateNameIsCorrect_WhenNameIsNull()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            db.Templates.Add(testTemplate);

            testTemplate.Name = null;

            JsonResult expected = new JsonResult { Data = (new { Answer = "WrongName" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfTemplateNameIsCorrect_WhenLengthIsMoreThen()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            db.Templates.Add(testTemplate);

            testTemplate.Name = "012345678901234567890123456789012345678901234567890123456789" +
                                "0123456789012345678901234567890123456789012345678901234567890123456789";

            JsonResult expected = new JsonResult { Data = (new { Answer = "WrongName" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldsInTemplateIsNull()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate", FieldsInTemplate = null };
            db.Templates.Add(testTemplate);

            JsonResult expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldsInTemplateIsCount()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate", FieldsInTemplate = { } };
            db.Templates.Add(testTemplate);

            JsonResult expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldInFieldsInTemplateIsCorrect_WhenFieldIsNull()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            FieldsInTemplate field = null;
            db.FieldsInTemplates.Add(field);
            db.Templates.Add(testTemplate);

            JsonResult expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldInFieldsInTemplateIsCorrect_WhenFieldIdIsZero()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            FieldsInTemplate field = new FieldsInTemplate { FieldId = 0 };

            db.FieldsInTemplates.Add(field);
            db.Templates.Add(testTemplate);

            JsonResult expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Tests_IfFieldInFieldsInTemplateIsCorrect_WhenFieldIdIsNotExists()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate" };
            FieldsInTemplate field = new FieldsInTemplate { FieldId = 2 };
            Field newField = new Field { Id = 1 };

            db.FieldsInTemplates.Add(field);
            db.Fields.Add(newField);
            db.Templates.Add(testTemplate);

            JsonResult expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
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

            var newField = new Field { Id = 1, Name = "TestField" };
            db.Fields.Add(newField);

            db.Templates.Add(testTemplate1);
            db.Templates.Add(testTemplate2);
            db.Templates.Add(testTemplate3);
            db.Templates.Add(testTemplate4);

            var field = new FieldsInTemplate { FieldId = 1, DefaultValue = "testvalue" };
            var testTemplate5 = new Template { Id = 5, Name = "TestTemplate" };

            db.Templates.Add(testTemplate5);
            db.FieldsInTemplates.Add(field);

            testTemplate5.FieldsInTemplate.Add(field);
            testTemplate5.Id = 6;

            var expected = new JsonResult { Data = (new { Answer = "WrongId" }) };
            var actual = (JsonResult)controller.EditTemplate(testTemplate5);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }

        [TestMethod]
        public void EditTemplate_Testing()
        {
            FakeDb db = new FakeDb();
            TemplatesController controller = new TemplatesController(db);

            Template testTemplate = new Template { Id = 1, Name = "TestTemplate", IsActive = true, Owner = "testowner1" };
            db.Templates.Add(testTemplate);

            Field newField1 = new Field { Id = 1, Name = "testfield1" };
            Field newField2 = new Field { Id = 2, Name = "testfield2" };
            Field newField3 = new Field { Id = 3, Name = "testfield3" };

            db.Fields.Add(newField1);
            db.Fields.Add(newField2);
            db.Fields.Add(newField3);

            FieldsInTemplate field1 = new FieldsInTemplate
            {
                Id = 1,
                FieldId = 1,
                TemplateId = 1,
                DefaultValue = "testvalue1"
            };
            FieldsInTemplate field2 = new FieldsInTemplate
            {
                Id = 2,
                FieldId = 2,
                TemplateId = 1,
                DefaultValue = "testvalue2"
            };
            FieldsInTemplate field3 = new FieldsInTemplate
            {
                Id = 3,
                FieldId = 3,
                TemplateId = 1,
                DefaultValue = "testvalue3"
            };
            db.FieldsInTemplates.Add(field1);
            db.FieldsInTemplates.Add(field2);
            db.FieldsInTemplates.Add(field3);
            JsonResult expected = new JsonResult { Data = (new { Answer = "FieldsIsEmpty" }) };
            JsonResult actual = (JsonResult)controller.EditTemplate(testTemplate);
            Assert.AreEqual(expected.Data.ToString(), actual.Data.ToString());
        }
    }
}

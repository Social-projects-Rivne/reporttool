using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingTool.Controllers;
using ReportingTool.DAL.Entities;
using System.Web.Mvc;
using System.Diagnostics;
using ReportingTool.Core.Services;
using System.Web;


namespace UnitTestProject
{
    [TestClass]
    public class TemplatesControllerTests
    {
        private static void deleteTestTemplateFromDB()
        {
            try
            {
                using (var db = new DB2())
                {
                    Template templateToRemove = db.Templates.Where(t =>
                    t.Name == "SomeStrangeTemplateNameThatWillNeverBeUsed" &&
                    t.Owner == "SomeStrangeOwnerNameThatWillNeverBeUsed" &&
                    t.IsActive == false).FirstOrDefault();

                    if (templateToRemove != null)
                    {
                        db.Templates.Remove(templateToRemove);
                        db.SaveChanges();
                    }
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }
        }

       [ClassInitialize()]
       public static void TemplatesControllerTestsInitialize(TestContext testContext) 
       {
           //Check if template which will be used for test exists in database.
           //If it exists then it should be deleted.
           deleteTestTemplateFromDB();
       }

       [ClassCleanup()]
       public static void TemplatesControllerTestsCleanup() 
       { 
           //Remove added to database test template
           deleteTestTemplateFromDB();
       }
        [TestMethod]
        public void AddNewTemplete_ValidateReturnedResult_TemplateIsNull()
        {
            //Arrange
            var templatesController = new TemplatesController();
            Template template = null;
            JsonResult expectedJson = new JsonResult { Data = (new { Answer = "IsNull"}) };

            //Act
            JsonResult result = (JsonResult)templatesController.AddNewTemplate(template);

            //Assert
            Assert.IsTrue(String.Equals(expectedJson.Data.ToString(), result.Data.ToString(), 
                          StringComparison.Ordinal));
        }

        [TestMethod]
        public void AddNewTemplete_ValidateReturnedResult_TemplateNameIsEmpty()
        {
            //Arrange
            var templatesController = new TemplatesController();
            Template template = new Template {  Name = "", IsActive = true, Owner = "testOwner"};
            JsonResult expectedJson = new JsonResult { Data = (new { Answer = "WrongName" }) };

            //Act
            JsonResult result = (JsonResult)templatesController.AddNewTemplate(template);

            //Assert
            Assert.IsTrue(String.Equals(expectedJson.Data.ToString(), result.Data.ToString(),
                          StringComparison.Ordinal));
        }

        [TestMethod]
        public void AddNewTemplete_ValidateReturnedResult_TemplateNameIsMoreThan128chars()
        {
            //Arrange
            var templatesController = new TemplatesController();

            StringBuilder templateName = new StringBuilder();
            for (int i = 0; i < 129; i++)
            {
                templateName.Append("n");
            }

            Template template = new Template { Name = templateName.ToString(), IsActive = true, Owner = "testOwner" };
            JsonResult expectedJson = new JsonResult { Data = (new { Answer = "WrongName" }) };

            //Act
            JsonResult result = (JsonResult)templatesController.AddNewTemplate(template);

            //Assert
            Assert.IsTrue(String.Equals(expectedJson.Data.ToString(), result.Data.ToString(),
                          StringComparison.Ordinal));
        }

        [TestMethod]
        public void AddNewTemplete_ValidateReturnedResult_CorrectTemplateAdded()
        {
            //Arrange
            var templatesController = new TemplatesController();
            List<FieldsInTemplate> fieldsInTemplate = new List<FieldsInTemplate>();
            fieldsInTemplate.Add(new FieldsInTemplate { FieldId = 1, DefaultValue = "default value" });
            fieldsInTemplate.Add(new FieldsInTemplate { FieldId = 2, DefaultValue = "default value" });
            fieldsInTemplate.Add(new FieldsInTemplate { FieldId = 3, DefaultValue = "default value" });
            fieldsInTemplate.Add(new FieldsInTemplate { FieldId = 4, DefaultValue = "default value" });
            fieldsInTemplate.Add(new FieldsInTemplate { FieldId = 5, DefaultValue = "default value" });
            fieldsInTemplate.Add(new FieldsInTemplate { FieldId = 6, DefaultValue = "default value" });
            fieldsInTemplate.Add(new FieldsInTemplate { FieldId = 7, DefaultValue = "default value" });
            fieldsInTemplate.Add(new FieldsInTemplate { FieldId = 8, DefaultValue = "default value" });

            //TODO: assign some fake session to SessionHelper
            SessionHelper.Session = templatesController.Session;

            Template testTemplate = new Template
                {
                    Name = "SomeStrangeTemplateNameThatWillNeverBeUsed",
                    FieldsInTemplate = fieldsInTemplate
                };        

            JsonResult expectedJson = new JsonResult { Data = (new { Answer = "Added" }) };

            //Act
            JsonResult result = (JsonResult)templatesController.AddNewTemplate(testTemplate);

            //Assert
            Assert.IsTrue(String.Equals(expectedJson.Data.ToString(), result.Data.ToString(),
                          StringComparison.Ordinal));
        }
    }


}

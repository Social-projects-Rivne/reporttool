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


namespace UnitTestProject
{
    [TestClass]
    public class TemplatesControllerTests
    {
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
        public void AddNewTemplete_ValidateReturnedResult_OwnerNameIsEmpty()
        {
            //Arrange
            var templatesController = new TemplatesController();
            Template template = new Template { Name = "testTemplate", IsActive = true, Owner = "" };
            JsonResult expectedJson = new JsonResult { Data = (new { Answer = "WrongOwnerName" }) };

            //Act
            JsonResult result = (JsonResult)templatesController.AddNewTemplate(template);

            //Assert
            Assert.IsTrue(String.Equals(expectedJson.Data.ToString(), result.Data.ToString(),
                          StringComparison.Ordinal));
        }

        [TestMethod]
        public void AddNewTemplete_ValidateReturnedResult_OwnerNameIsMoreThan128chars()
        {
            //Arrange
            var templatesController = new TemplatesController();

            StringBuilder ownerName = new StringBuilder();
            for (int i = 0; i < 129; i++)
            {
                ownerName.Append("n");
            }

            Template template = new Template { Name = "testTemplate", IsActive = true, Owner = ownerName.ToString() };
            JsonResult expectedJson = new JsonResult { Data = (new { Answer = "WrongOwnerName" }) };

            //Act
            JsonResult result = (JsonResult)templatesController.AddNewTemplate(template);

            //Assert
            Assert.IsTrue(String.Equals(expectedJson.Data.ToString(), result.Data.ToString(),
                          StringComparison.Ordinal));
        }
    }


}

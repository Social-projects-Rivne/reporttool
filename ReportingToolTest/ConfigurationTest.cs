using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingTool.Models;
using ReportingTool.Controllers;
using System.Web.Mvc;
using ReportingTool.Core.Validation;

namespace ReportingToolTest
{
    public enum Answer { NotExists, IsEmpty, NotValid, Exists, Created, NotCreated };
    [TestClass]
    public class ConfigurationTest
    {
        private ConfigurationParametersModel model = new ConfigurationParametersModel() { ServerUrl = "http://ssu-jira.softserveinc.com", ProjectName = "RIVNE015.NET" };

        [TestMethod]
        public void ValidateParametes_ValidVales_ReturnedTrue()
        {
            // arrange
            string serverUrl = "http://ssu-jira.softserveinc.com";
            string projectName = "RIVNE015.NET";
            //act
            bool actual = ConfigurationHelper.ValidateParametes(serverUrl, projectName);

            //assert
            Assert.IsTrue(actual);
        }

        [TestMethod]
        public void ValidateParametes_NotValidValesInProjectName_ReturnedFalse()
        {
            // arrange
            string serverUrl = "http://ssu-jira.softserveinc.com/";
            string projectName = "RIVNE015.Net";
            //act
            bool actual = ConfigurationHelper.ValidateParametes(serverUrl, projectName);

            //assert
            Assert.IsFalse(actual);
        }

        [TestMethod]
        public void SetConfigurations_FileNotExists_ReturnedNotExists()
        {
            string filename = "../../TestFiles/Configurations1.ini";
            ConfigurationController config = new ConfigurationController();
            config.FileName = filename;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.NotExists) };

            //act
            var result = config.SetConfigurations() as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_FileIsEmpty_ReturnedIsEmpty()
        {
            string filename = "../../TestFiles/Configurations2.ini";
            ConfigurationController config = new ConfigurationController();
            config.FileName = filename;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.IsEmpty) };

            //act
            var result = config.SetConfigurations() as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionNotExists_ReturnedIsEmpty()
        {
            string filename = "../../TestFiles/Configurations3.ini";
            ConfigurationController config = new ConfigurationController();
            config.FileName = filename;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.IsEmpty) };

            //act
            var result = config.SetConfigurations() as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionIsEmpty_ReturnedIsEmpty()
        {
            string filename = "../../TestFiles/Configurations4.ini";
            ConfigurationController config = new ConfigurationController();
            config.FileName = filename;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.IsEmpty) };

            //act
            var result = config.SetConfigurations() as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysAreEmpty_ReturnedIsEmpty()
        {
            string filename = "../../TestFiles/Configurations5.ini";
            ConfigurationController config = new ConfigurationController();
            config.FileName = filename;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.IsEmpty) };

            //act
            var result = config.SetConfigurations() as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysNotValid_ReturnedNotValid()
        {
            string filename = "../../TestFiles/Configurations6.ini";
            ConfigurationController config = new ConfigurationController();
            config.FileName = filename;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.NotValid) };

            //act
            var result = config.SetConfigurations() as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysNotEmpty_ReturnedExists()
        {
            string filename = "../../TestFiles/Configurations7.ini";
            ConfigurationController config = new ConfigurationController();
            config.FileName = filename;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.Exists) };

            //act
            var result = config.SetConfigurations() as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_FileNotExists_ReturnedCreated()
        {
            string filetest = "../../TestFiles/CreatedByTest/Configurations1.Test.ini";
            ConfigurationController config = new ConfigurationController();
            config.FileName = filetest;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.Created) };

            //act
            var result = config.SetConfigurations(model) as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_FileIsEmpty_ReturnedCreated()
        {
            string filename = "../../TestFiles/Configurations2.ini";
            string filetest = "../../TestFiles/CreatedByTest/Configurations2.2.Test.ini";
            System.IO.File.Copy(filename, filetest);
            ConfigurationController config = new ConfigurationController();
            config.FileName = filetest;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.Created) };

            //act
            var result = config.SetConfigurations(model) as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionNotExists_ReturnedCreated()
        {
            string filename = "../../TestFiles/Configurations3.ini";
            string filetest = "../../TestFiles/CreatedByTest/Configurations2.3.Test.ini";
            System.IO.File.Copy(filename, filetest);
            ConfigurationController config = new ConfigurationController();
            config.FileName = filetest;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.Created) };

            //act
            var result = config.SetConfigurations(model) as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionIsEmpty_ReturnedCreated()
        {
            string filename = "../../TestFiles/Configurations4.ini";
            string filetest = "../../TestFiles/CreatedByTest/Configurations2.4.Test.ini";
            System.IO.File.Copy(filename, filetest);
            ConfigurationController config = new ConfigurationController();
            config.FileName = filetest;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.Created) };

            //act
            var result = config.SetConfigurations(model) as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysAreEmpty_ReturnedCreated()
        {
            string filename = "../../TestFiles/Configurations5.ini";
            string filetest = "../../TestFiles/CreatedByTest/Configurations2.5.Test.ini";
            System.IO.File.Copy(filename, filetest);
            ConfigurationController config = new ConfigurationController();
            config.FileName = filetest;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.Created) };

            //act
            var result = config.SetConfigurations(model) as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysNotValid_ReturnedNotCreated()
        {
            string filename = "../../TestFiles/Configurations6.ini";
            string filetest = "../../TestFiles/CreatedByTest/Configurations2.6.Test.ini";
            System.IO.File.Copy(filename, filetest);
            ConfigurationController config = new ConfigurationController();
            config.FileName = filetest;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.NotCreated) };

            //act
            var result = config.SetConfigurations(model) as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysNotEmpty_ReturnedNotCreated()
        {
            string filename = "../../TestFiles/Configurations7.ini";
            string filetest = "../../TestFiles/CreatedByTest/Configurations2.7.Test.ini";
            System.IO.File.Copy(filename, filetest);
            ConfigurationController config = new ConfigurationController();
            config.FileName = filetest;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.NotCreated) };

            //act
            var result = config.SetConfigurations(model) as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_ModelIsNull_ReturnedNotCreated()
        {
            ConfigurationController config = new ConfigurationController();
            ConfigurationParametersModel model = null;

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.NotCreated) };

            //act
            var result = config.SetConfigurations(model) as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }

        [TestMethod]
        public void SetConfigurations_ModelParameterIsNull_ReturnedNotCreated()
        {
            ConfigurationController config = new ConfigurationController();
            ConfigurationParametersModel model = new ConfigurationParametersModel() { ProjectName = "PROJECT" };

            //arrange
            var expected = new { Answer = Enum.GetName(typeof(Answer), Answer.NotCreated) };

            //act
            var result = config.SetConfigurations(model) as JsonResult;
            var actual = result.Data;

            //assert
            Assert.ReferenceEquals(actual, expected);
        }
    }
}
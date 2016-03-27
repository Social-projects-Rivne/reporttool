using System.Web.Mvc;
using IniParser;
using IniParser.Model;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingTool.Controllers;
using ReportingTool.Core.Validation.Interfaces;
using ReportingTool.Models;

namespace UnitTestProject
{
    public class FakeFileExtensionManager : IFileExtensionManager
    {
        public bool IsExistsValue { get; set; }

        public bool IsEmptyValue { get; set; }

        public IniData IniDataValue { get; set; }

        public FakeFileExtensionManager()
        {
            IsExistsValue = false;
            IsEmptyValue = true;
            IniDataValue = new IniData();
        }
        public bool IsExists(string path)
        {
            return IsExistsValue;
        }

        public bool IsEmpty(string path)
        {
            return IsEmptyValue;
        }

        public IniData ReadFile(FileIniDataParser fileIniData, string path)
        {
            return IniDataValue;
        }
        public void WriteFile(FileIniDataParser fileIniData, string path, IniData parsedData)
        {
            //simulation of writing data to the config file by FileIniDataParser
        }
    }

    [TestClass]
    public class ConfigurationTest
    {
        private const string SECTION = "GeneralConfiguration";
        private const string SERVEL_URL_KEY = "ServerUrl";
        private const string PROJECT_NAME_KEY = "ProjectName";
        public FakeFileExtensionManager FakeFileManager { get; set; }
        public static ConfigurationController Configuration { get; set; }
        public static ConfigurationParametersModel ParametersModel { get; set; }

        [ClassInitialize]
        public static void ConfigurationTestInitializer(TestContext testContext)
        {
            Configuration = new ConfigurationController();

            ParametersModel = new ConfigurationParametersModel
            {
                ProjectName = "RVNETJAN",
                ServerUrl = " http://ssu-jira.softserveinc.com"
            };
        }

        [TestInitialize]
        public void FakeFileManagerInitializer()
        {
            FakeFileManager = new FakeFileExtensionManager();
        }

        [TestMethod]
        public void SetConfigurations_FileNotExists_ReturnedNotExists()
        {
            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.NotExists };

            //act
            var actual = Configuration.SetConfigurations() as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_FileIsEmpty_ReturnedIsEmpty()
        {
            FakeFileManager.IsExistsValue = true;
            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.IsEmpty };

            //act
            var actual = Configuration.SetConfigurations() as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_SectionNotExists_ReturnedIsEmpty()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.IsEmpty };

            //act
            var actual = Configuration.SetConfigurations() as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_SectionIsEmpty_ReturnedIsEmpty()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            FakeFileManager.IniDataValue.Sections.AddSection(SECTION);
            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.IsEmpty };

            //act
            var actual = Configuration.SetConfigurations() as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysAreEmpty_ReturnedIsEmpty()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            FakeFileManager.IniDataValue.Sections.AddSection(SECTION);
            FakeFileManager.IniDataValue[SECTION].AddKey(SERVEL_URL_KEY, "");
            FakeFileManager.IniDataValue[SECTION].AddKey(PROJECT_NAME_KEY, "");

            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.IsEmpty };

            //act
            var actual = Configuration.SetConfigurations() as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysNotEmpty_ReturnedExists()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            FakeFileManager.IniDataValue.Sections.AddSection(SECTION);
            FakeFileManager.IniDataValue[SECTION].AddKey(SERVEL_URL_KEY, "http://ssu-jira.softserveinc.com");
            FakeFileManager.IniDataValue[SECTION].AddKey(PROJECT_NAME_KEY, "RVNETJAN");

            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.Exists };

            //act
            var actual = Configuration.SetConfigurations() as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_FileNotExists_ReturnedCreated()
        {
            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.Created };

            //act
            var actual = Configuration.SetConfigurations(ParametersModel) as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_FileIsEmpty_ReturnedCreated()
        {
            FakeFileManager.IsExistsValue = true;
            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.Created };

            //act
            var actual = Configuration.SetConfigurations(ParametersModel) as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_SectionNotExists_ReturnedCreated()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.Created };

            //act
            var actual = Configuration.SetConfigurations(ParametersModel) as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_SectionIsEmpty_ReturnedCreated()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            FakeFileManager.IniDataValue.Sections.AddSection(SECTION);
            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.Created };

            //act
            var actual = Configuration.SetConfigurations(ParametersModel) as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysAreEmpty_ReturnedCreated()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            FakeFileManager.IniDataValue.Sections.AddSection(SECTION);
            FakeFileManager.IniDataValue[SECTION].AddKey(SERVEL_URL_KEY, "");
            FakeFileManager.IniDataValue[SECTION].AddKey(PROJECT_NAME_KEY, "");

            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.Created };

            //act
            var actual = Configuration.SetConfigurations(ParametersModel) as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysNotValid_ReturnedNotCreated()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            Configuration.FileManager = FakeFileManager;
            var invalidModel = new ConfigurationParametersModel
            {
                ProjectName = "rv",
                ServerUrl = "ssu-jira.softserveinc.com"
            };

            //arrange
            var expected = new { Answer = Answer.NotCreated };

            //act
            var actual = Configuration.SetConfigurations(invalidModel) as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_ModelIsNull_ReturnedNotCreated()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            Configuration.FileManager = FakeFileManager;
            ConfigurationParametersModel invalidModel = null;

            //arrange
            var expected = new { Answer = Answer.NotCreated };

            //act
            var actual = Configuration.SetConfigurations(invalidModel) as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_SectionKeysNotEmpty_ReturnedNotCreated()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            FakeFileManager.IniDataValue.Sections.AddSection(SECTION);
            FakeFileManager.IniDataValue[SECTION].AddKey(SERVEL_URL_KEY, "http://ssu-jira.softserveinc.com");
            FakeFileManager.IniDataValue[SECTION].AddKey(PROJECT_NAME_KEY, "RVNETJAN");

            Configuration.FileManager = FakeFileManager;
            //arrange
            var expected = new { Answer = Answer.NotCreated };

            //act
            var actual = Configuration.SetConfigurations(ParametersModel) as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }

        [TestMethod]
        public void SetConfigurations_ModelParameterIsNull_ReturnedNotCreated()
        {
            FakeFileManager.IsExistsValue = true;
            FakeFileManager.IsEmptyValue = false;
            Configuration.FileManager = FakeFileManager;
            var invalidModel = new ConfigurationParametersModel
            {
                ProjectName = "RVNETJAN",
            };

            //arrange
            var expected = new { Answer = Answer.NotCreated };

            //act
            var actual = Configuration.SetConfigurations(invalidModel) as JsonResult;

            //assert
            Assert.AreEqual(actual.Data.ToString(), expected.ToString());
        }
    }
}
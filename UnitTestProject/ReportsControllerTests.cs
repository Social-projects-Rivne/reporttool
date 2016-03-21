using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingTool.Controllers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;
using Newtonsoft.Json;

namespace UnitTestProject
{
    [TestClass]
    public class ReportsControllerTests
    {
        [TestMethod]
        public void GetUserActivity_WrongUserName_ReturnsBadRequest()
        {
            //Arrange
            var reportsController = new ReportsController(new JiraClientMock());
            var wrongUserName = String.Empty;
            var dateFrom = "2016-01-01";
            var dateTo = "2016-01-15";

            var expectedResult = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Act
            var result = (HttpStatusCodeResult)reportsController.GetUserActivity(wrongUserName, dateFrom, dateTo);

            //Assert
            Assert.AreEqual(expectedResult.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void GetUserActivity_WrongDateFromFormat_ReturnsBadRequest()
        {
            //Arrange
            var reportsController = new ReportsController(new JiraClientMock());
            var userName = "testUserName";
            var dateFromInWrongFormat = "01-01-2016";
            var dateTo = "2016-01-15";

            var expectedResult = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Act
            var result = (HttpStatusCodeResult)reportsController.
                GetUserActivity(userName, dateFromInWrongFormat, dateTo);

            //Assert
            Assert.AreEqual(expectedResult.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void GetUserActivity_WrongDateToFormat_ReturnsBadRequest()
        {
            //Arrange
            var reportsController = new ReportsController(new JiraClientMock());
            var userName = "testUserName";
            var dateFrom = "2016-01-01";
            var dateToInWrongFormat = "15-01-2016";

            var expectedResult = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Act
            var result = (HttpStatusCodeResult)reportsController.
                GetUserActivity(userName, dateFrom, dateToInWrongFormat);

            //Assert
            Assert.AreEqual(expectedResult.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void GetUserActivity_SwappedDates_ReturnsBadRequest()
        {
            //Arrange
            var reportsController = new ReportsController(new JiraClientMock());
            var userName = "testUserName";
            var dateFrom = "2016-01-15";
            var dateTo = "2016-01-01";

            var expectedResult = new HttpStatusCodeResult(HttpStatusCode.BadRequest);

            //Act
            var result = (HttpStatusCodeResult)reportsController.
                GetUserActivity(userName, dateFrom, dateTo);

            //Assert
            Assert.AreEqual(expectedResult.StatusCode, result.StatusCode);
        }

        [TestMethod]
        public void GetUserActivity_CorrectArguments_ReturnsTime()
        {
            //Arrange
            var reportsController = new ReportsController(new JiraClientMock());
            var userName = "testUserName";
            var dateFrom = "2016-01-01";
            var dateTo = "2016-01-15";

            JsonResult expectedResult = new JsonResult { Data = (new { TimeSpent = 5000}) };

            //Act
            var result = reportsController.
                GetUserActivity(userName, dateFrom, dateTo);

            //Assert
            Assert.IsTrue(String.Equals(expectedResult.ToString(), 
                                        result.ToString()));
        }
    }
}

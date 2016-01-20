using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingTool.DAL.DataAccessLayer.Jira;

namespace ReportingTool.UnitTests
{
    [TestClass]
    public class JiraClientTests
    {
        [TestMethod]
        public void CheckingGetAllUsersFromJira()
        {
            string baseUrl = "http://ssu-jira.softserveinc.com";
            string username = "";
            string password = "";
            string project = "RVNETJAN";
            JiraClient client = new JiraClient(baseUrl, username, password);
            Assert.IsNotNull(client.GetAllUsers(project));
        }
        [TestMethod]
        public void CheckingGetUsersFromJira()
        {
            string baseUrl = "http://ssu-jira.softserveinc.com";
            string username = "";
            string password = "";
            string project = "RVNETJAN";
            JiraClient client = new JiraClient(baseUrl, username, password);
            Assert.IsNotNull(client.GetUsers(project, 0));
        }
    }
}

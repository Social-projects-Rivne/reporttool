using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingTool.DAL.DataAccessLayer;

namespace ReportingToolTest
{
    [TestClass]
    public class JiraClientTest
    {
        public IJiraClient Client { get; set; }

        [TestInitialize]
        public void InitializeJiraClientTest() 
        {
            Client = new JiraClientMock();
        }

        [TestMethod]
        public void JiraClientConstructor_ValidVales_ReturnedIsNotNull()
        {
            JiraClient instance = new JiraClient("http://ssu-jira.softserveinc.com","name","pass");
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void GetAllUsers_ValidVales_ReturnedListOfAllUsers()
        {
            //act
            var users = Client.GetAllUsers("RVNETJAN");

            //assert
            users.ForEach(u => Assert.IsNotNull(u.name));
        }

        [TestMethod]
        [ExpectedException(exceptionType: typeof(JiraClientException))]

        public void AssertStatus_InvalidValues_ThrowExeption()
        {
            //act
            var users = Client.GetAllUsers("FAILED_NAME");
            // assert is handled by ExpectedException
        }
    }
}

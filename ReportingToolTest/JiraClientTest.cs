using Microsoft.VisualStudio.TestTools.UnitTesting;
using ReportingTool.DAL.DataAccessLayer;

namespace ReportingToolTest
{
    [TestClass]
    public class JiraClientTest
    {
        [TestMethod]
        public void JiraClientConstructor_ValidVales_ReturnedIsNotNull()
        {
            JiraClient instance =new JiraClient();
            Assert.IsNotNull(instance);
        }

        [TestMethod]
        public void GetUsers_ValidVales_ReturnedListOfUsers()
        {
            // arrange
            JiraClient client=new JiraClient();

            //act
            var users = client.GetUsers("RVNETJAN", 0);

            //assert
            //Assert.IsNotNull(users.Select(u=>u.name));
            users.ForEach(u=>Assert.IsNotNull(u.name));
        }

        [TestMethod]
        public void GetAllUsers_ValidVales_ReturnedListOfAllUsers()
        {
            // arrange
            JiraClient client = new JiraClient();

            //act
            var users = client.GetAllUsers("RVNETJAN");

            //assert
            //Assert.IsNotNull(users.Select(u=>u.name));
            users.ForEach(u => Assert.IsNotNull(u.name));
        }

        [TestMethod]
        [ExpectedException(exceptionType: typeof(JiraClientException))]

        public void AssertStatus_InvalidValues_ThrowExeption()
        {
            // arrange
            JiraClient client = new JiraClient();

            //act
            var users = client.GetAllUsers("FAILED_NAME");

            // assert is handled by ExpectedException
        }
    }
}

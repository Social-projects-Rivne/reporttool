using System.Collections.Generic;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.DAL.Entities;

namespace ReportingToolTest
{
    /// <summary>
    /// Represents mock class that simulates JiraClient class
    /// </summary>
    class JiraClientMock : IJiraClient
    {
        /// <summary>
        /// Returns test users objects
        /// </summary>
        /// <param name="projectName">Project name</param>
        /// <returns>Jira users</returns>
        public List<JiraUser> GetAllUsers(string projectName)
        {
            if (projectName != "RVNETJAN")
            {
                throw new JiraClientException("Not valid project name");
            }
            var users = new List<JiraUser>
            {
                new JiraUser {name = "adavytc", displayName = "Adriana Davydyuk"},
                new JiraUser {name = "alubetc", displayName = "Aleksandr Lubenskyi"},
                new JiraUser {name = "alistc", displayName = "Aleksey Lisogurskiy"},
                new JiraUser {name = "dyulitc", displayName = "Dzyubak Yuliya"},
                new JiraUser {name = "isenktc", displayName = "Igor Senkiv"},
                new JiraUser {name = "lshyitc", displayName = "Lora Shyian"},
                new JiraUser {name = "myurintc", displayName = "Mikhail Yurin"},
                new JiraUser {name = "okruktc", displayName = "Olena Kruk"},
                new JiraUser {name = "schyktc", displayName = "Sofija Chykota"},
                new JiraUser {name = "vsavktc", displayName = "Vasily Savka"}
            };
            return users;
        }
    }
}

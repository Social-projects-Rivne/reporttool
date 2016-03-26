using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportingTool.Core.Services;
using ReportingTool.Core.Validation;
using ReportingTool.Core.Models;

namespace UnitTestProject
{
    public class JiraClientServiceMock : IJiraClientService
    {
        public int GetUserActivity(string userName, string dateFrom, string dateTo)
        {
            const int TimeSpent = 5000;

            if (!ReportsValidator.UserNameIsCorrect(userName) ||
                !ReportsValidator.DatesAreCorrect(dateFrom, dateTo))
            {
                throw new ArgumentException();
            }

            return TimeSpent;
        }

        public List<IssueModel> GetUserIssues(string userName, string dateFrom, string dateTo)
        {
            if (!ReportsValidator.UserNameIsCorrect(userName) ||
                !ReportsValidator.DatesAreCorrect(dateFrom, dateTo))
            {
                throw new ArgumentException();
            }

            List<IssueModel> result = new List<IssueModel> 
            { 
                new IssueModel 
                {   key = "issueKey1",
                    loggedTime = 600,
                    status = "In Progress",
                    summary = "Issue 1 summary"
                },
                new IssueModel 
                {   key = "issueKey2",
                    loggedTime = 0,
                    status = "To Do",
                    summary = "Issue 2 summary"
                }
            };

            return result;
        }
    }
}

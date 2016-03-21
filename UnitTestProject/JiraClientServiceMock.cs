using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportingTool.Core.Services;
using ReportingTool.Core.Validation;

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

        public List<ReportingTool.Core.Models.IssueModel> GetUserIssues(string userName, string dateFrom, string dateTo)
        {
            throw new NotImplementedException();
        }
    }
}

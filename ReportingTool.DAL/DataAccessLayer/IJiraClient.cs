using System.Collections.Generic;
using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.DataAccessLayer
{
    public interface IJiraClient
    {
        List<JiraUser> GetAllUsers(string projectName);
        List<JiraUser> GetUsers(string projectName, int startAt);
        Worklog GetWorklogByIssueKey(string issueKey);
        List<Issue> GetUserIssues(string userName, string dateFrom, string dateTo);
    }
}
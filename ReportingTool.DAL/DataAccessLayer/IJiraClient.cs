using System.Collections.Generic;
using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.DataAccessLayer
{
    public interface IJiraClient
    {
        List<JiraUser> GetAllUsers(string projectName);
        List<JiraUser> GetUsers(string projectName, int startAt);
        List<Issue> GetAllIssues(string dateFrom, string dateTo);
        Worklog GetWorklogByIssueKey(string issueKey);
    }
}
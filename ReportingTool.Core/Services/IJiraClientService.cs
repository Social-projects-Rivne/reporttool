using System;
using System.Collections.Generic;
using ReportingTool.DAL.Entities;

namespace ReportingTool.Core.Services
{
    public interface IJiraClientService
    {
        int GetUserActivity(string userName, string dateFrom, string dateTo);
        List<Issue> GetIssuesWithWorklogs(string dateFrom, string dateTo);
    }
}

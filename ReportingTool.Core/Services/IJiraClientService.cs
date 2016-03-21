using System;
using System.Collections.Generic;
using ReportingTool.DAL.Entities;
using ReportingTool.Core.Models;

namespace ReportingTool.Core.Services
{
    public interface IJiraClientService
    {
        int GetUserActivity(string userName, string dateFrom, string dateTo);
        List<IssueModel> GetUserIssues(string userName, string dateFrom, string dateTo);
    }
}

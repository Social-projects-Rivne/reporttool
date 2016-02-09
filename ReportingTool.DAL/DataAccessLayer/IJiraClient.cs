using System.Collections.Generic;
using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL.DataAccessLayer
{
    public interface IJiraClient
    {
        List<JiraUser> GetAllUsers(string projectName);
    }
}
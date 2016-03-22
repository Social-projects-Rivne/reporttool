using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;

using ReportingTool.DAL.Entities;
using ReportingTool.DAL.DataAccessLayer;
using System.Web.UI;
using ReportingTool.Core.Validation;
using ReportingTool.Core.Models;


namespace ReportingTool.Core.Services
{
    public class JiraClientService : IJiraClientService
    {
        private IJiraClient jiraClient;
        public JiraClientService(IJiraClient jiraclient)
        {
            this.jiraClient = jiraclient;
        }

        public JiraClientService()
        {
            jiraClient = HttpContext.Current.Session["jiraClient"] as JiraClient;
        }

        /// <summary>
        /// Counting of spent time in specific worklogs
        /// </summary>
        /// <param name="worklog">Worklog contains list of worklogs deatails</param>
        /// <param name="userName">Login of specific user</param>
        /// <returns>Spent time in seconds</returns>
        private int countWorklogsTimeSpent(Worklog worklog, string userName)
        {
            int timeSpent = 0;
            foreach (var worklogDetails in worklog.worklogs)
            {
                if (worklogDetails.author.name == userName)
                {
                    timeSpent += worklogDetails.timeSpentSeconds;
                }
            }
            return timeSpent;
        }

        /// <summary>
        /// Counting time spent in issues of specific user
        /// </summary>
        /// <param name="userName">Login of specific user</param>
        /// <param name="dateFrom">Lower boundary of time period</param>
        /// <param name="dateTo">Upper boundary of time period</param>
        /// <returns>Spent time in seconds</returns>
        public int GetUserActivity(string userName, string dateFrom, string dateTo)
        {
            if (!ReportsValidator.UserNameIsCorrect(userName) ||
                !ReportsValidator.DatesAreCorrect(dateFrom, dateTo))
            {
                throw new ArgumentException();
            }

            var issues = jiraClient.GetUserIssues(userName, dateFrom, dateTo);
            if (issues == null)
            {
                return 0;
            }

            var timeSpent = 0;
            foreach (var issue in issues)
            {
                //if total quantity of worklogs in issue is more than allowed by Jira
                if (issue.fields.worklog.total > issue.fields.worklog.maxResults)
                {
                    //get all worklogs of specific issue
                    Worklog worklog = jiraClient.GetWorklogByIssueKey(issue.key);

                    timeSpent += countWorklogsTimeSpent(worklog, userName);
                }
                else
                    if (issue.fields.worklog.worklogs != null)
                    {
                        timeSpent += countWorklogsTimeSpent(issue.fields.worklog, userName);
                    }
            }
            return timeSpent;
        }

        /// <summary>
        /// Retreiveing issue models for specific user for specific period of time
        /// </summary>
        /// <param name="userName">Login of specific user</param>
        /// <param name="dateFrom">Lower boundary of time period</param>
        /// <param name="dateTo">Upper boundary of time period</param>
        /// <returns>List of issue models</returns>
        public List<IssueModel> GetUserIssues(string userName, string dateFrom, string dateTo)
        {
            List<IssueModel> result = new List<IssueModel>();

            var issuesWithUsersWorklogs = jiraClient.GetUserIssues(userName, dateFrom, dateTo);
            if (issuesWithUsersWorklogs != null)
            {
                foreach (var issue in issuesWithUsersWorklogs)
                {
                    IssueModel issueModel = new IssueModel
                    {
                        key = issue.key,
                        status = issue.fields.status.name,
                        summary = issue.fields.summary
                    };

                    if (issue.fields.worklog.worklogs != null)
                    {
                        foreach (var worklog in issue.fields.worklog.worklogs)
                        {
                            if (worklog.author.name == userName)
                            {
                                issueModel.loggedTime += worklog.timeSpentSeconds;
                            }
                        }
                    }
                    result.Add(issueModel);
                }
            }
            return result;
        }
    }
}

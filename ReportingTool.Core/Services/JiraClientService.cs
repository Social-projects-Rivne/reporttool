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
        /// Retrieving list of issues of current project for period of time  
        /// </summary>
        /// <param name="dateFrom">Lower boundary of time period</param>
        /// <param name="dateTo">Upper boundary of time period</param>
        /// <returns>List of issues</returns>
        public List<Issue> GetIssuesWithWorklogs(string dateFrom, string dateTo)
        {
            return jiraClient.GetAllIssues(dateFrom, dateTo);
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

            var issues = jiraClient.GetAllIssues(dateFrom, dateTo);
            if (issues == null)
            {
                return 0;
            }

            var timeSpent = 0;
            foreach (var issue in issues)
            {
                //if total quantity of worklogs in issue is more than allowed my Jira
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
        /// Retrieveing issues of specific user with worklogs
        /// </summary>
        /// <param name="userName">Login of specific user</param>
        /// <param name="dateFrom">Lower boundary of time period</param>
        /// <param name="dateTo">Upper boundary of time period</param>
        /// <returns>List of issues entities</returns>
        public List<Issue> GetIssuesWithUserWorklogs(string userName, string dateFrom, string dateTo)
        {
            List<Issue> result = new List<Issue>();

            var issues = jiraClient.GetAllIssues(dateFrom, dateTo);

            if (issues != null)
            {
                foreach (var issue in issues)
                {
                    Worklog worklog = jiraClient.GetWorklogByIssueKey(issue.key);
                    var worklogs = worklog.worklogs.Where(w => w.author.name == userName).ToList();
                    bool issueHasAssignee = issue.fields.assignee != null;

                    if (worklogs.Count != 0 || 
                        (issueHasAssignee && issue.fields.assignee.name == userName))
                    {
                        result.Add(issue);
                    }
                }
            }
            return result;
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
            if (!ReportsValidator.UserNameIsCorrect(userName) ||
                !ReportsValidator.DatesAreCorrect(dateFrom, dateTo))
            {
                throw new ArgumentException();
            }

            List<IssueModel> result = new List<IssueModel>();
            
            var issuesWithUsersWorklogs = GetIssuesWithUserWorklogs(userName, dateFrom, dateTo);
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

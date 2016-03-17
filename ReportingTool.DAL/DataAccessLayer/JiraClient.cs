﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;
using System.Net;
using ReportingTool.DAL.Entities;
using System.Web;
using Jira.SDK;
using Jira.SDK.Domain;


namespace ReportingTool.DAL.DataAccessLayer
{
    public class JiraClient : IJiraClient
    {
        private string username;
        private string password;
        private string baseApiUrl;
        private JsonDeserializer deserializer;
        
        public JiraClient(string baseUrl, string username, string password)
        {
            this.username = username;
            this.password = password;
            baseApiUrl = new Uri(new Uri(baseUrl), "rest/api/2/").ToString();
            deserializer = new JsonDeserializer();
        }

        private RestRequest CreateRequest(Method method, String path)
        {
            var request = new RestRequest { Method = method, Resource = path, RequestFormat = DataFormat.Json };
            request.AddHeader("Authorization", "Basic " + Convert.ToBase64String(Encoding.UTF8.GetBytes(String.Format("{0}:{1}", username, password))));
            return request;
        }
        private IRestResponse ExecuteRequest(RestRequest request)
        {
            var client = new RestClient(baseApiUrl);
            return client.Execute(request);
        }

        private void AssertStatus(IRestResponse response, HttpStatusCode status)
        {
            if (response.ErrorException != null)
                throw new JiraClientException("Transport level error: " + response.ErrorMessage, response.ErrorException);
            if (response.StatusCode != status)
                throw new JiraClientException("JIRA returned wrong status: " + response.StatusDescription, response.Content);
        }

        public List<JiraUser> GetUsers(string projectName, int startAt)
        {
            string path = "user/assignable/search?project=" + projectName + "&startAt=" + startAt + "&maxResults=" + 1000;
            var request = CreateRequest(Method.GET, path);

            var response = ExecuteRequest(request);
            AssertStatus(response, HttpStatusCode.OK);
            return deserializer.Deserialize<List<JiraUser>>(response);

        }
        
        public List<JiraUser> GetAllUsers(string projectName)
        {
            List<JiraUser> result = new List<JiraUser>();
            int count = 0;
            int cursor = 0;
            do
            {
                var users = GetUsers(projectName, cursor);
                count = users.Count;
                foreach (var u in users)
                {
                    result.Add(u);
                }
                cursor += 1000;
            } while (count >= 1000);

            return result;
        }

        /// <summary>
        /// Retreiving of issues using one request
        /// </summary>
        /// <param name="dateFrom">Lower boundary of time period</param>
        /// <param name="dateTo">Upper boundary of time period</param>
        /// <param name="startAt">Start position query</param>
        /// <returns></returns>
        public List<Issue> GetIssues(string dateFrom, string dateTo, int startAt)
        {
            var curProjectName = HttpContext.Current.Session["projectKey"] as string;

            var correctedDateFrom = DateTime.Parse(dateFrom).AddDays(-1).ToString("yyyy-MM-dd");
            var correctedDateTo = DateTime.Parse(dateTo).AddDays(+1).ToString("yyyy-MM-dd");

            string path = "search?jql=updated >" + correctedDateFrom + 
                " and updated < " + correctedDateTo + 
                " and project = " + curProjectName + 
                " and timespent > 0&fields=summary,worklog" +
                "&startAt=" + startAt + "&maxResults=" + 50;
            var request = CreateRequest(Method.GET, path);

            var response = ExecuteRequest(request);
            AssertStatus(response, HttpStatusCode.OK);

            deserializer.RootElement = "issues";
            return deserializer.Deserialize<List<Issue>>(response);
        }

        /// <summary>
        /// Retreiving of all issues considering restrictions of Jira
        /// </summary>
        /// <param name="dateFrom">Lower boundary of time period</param>
        /// <param name="dateTo">Upper boundary of time period</param>
        /// <returns>List of issues</returns>
        public List<Issue> GetAllIssues(string dateFrom, string dateTo)
        {
            List<Issue> result = new List<Issue>();
            int count = 0;
            int cursor = 0;
            do
            {
                var issues = GetIssues(dateFrom, dateTo, cursor);
                count = issues.Count;
                foreach (var issue in issues)
                {
                    result.Add(issue);
                }
                cursor += 50;
            } while (count >= 50);

            return result;
        }
        /// <summary>
        /// Retreiving of worklog for issue with specific key
        /// </summary>
        /// <param name="issueKey">Key of specific issue</param>
        /// <returns>Worklog of cpecific issue</returns>
        public  Worklog GetWorklogByIssueKey(string issueKey)
        {
            string path = "/issue/" + issueKey + "/worklog";
            var request = CreateRequest(Method.GET, path);

            var response = ExecuteRequest(request);
            AssertStatus(response, HttpStatusCode.OK);

            deserializer.RootElement = "issues.fields.worklog";
            return deserializer.Deserialize<Worklog>(response);
        }

    }
}

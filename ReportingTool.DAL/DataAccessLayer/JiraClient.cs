using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using RestSharp;
using RestSharp.Deserializers;
using System.Net;
using ReportingTool.DAL.Entities;


namespace ReportingTool.DAL.DataAccessLayer
{
    public class JiraClient
    {
        private string username;
        private string password;
        private string baseApiUrl;
        private JsonDeserializer deserializer;
        public JiraClient(string baseUrl = "http://ssu-jira.softserveinc.com/", string username = "ofeodtc", string password = "jss-em}t")
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
            int i = 1;
            do
            {
                count = GetUsers(projectName, cursor).Count;
                foreach (var u in GetUsers(projectName, cursor))
                {
                    result.Add(u);
                }
                cursor += 1000;
            } while (count >= 1000);

            return result;
        }

        //public List<JiraUser> GetUsersInGroup(string groupName)
        //{
        //    string path = "group?groupname=" + groupName;
        //    var request = CreateRequest(Method.GET, path);

        //    var response = ExecuteRequest(request);
        //    AssertStatus(response, HttpStatusCode.OK);
        //    return deserializer.Deserialize<List<JiraUser>>(response);
        //}
    }
}

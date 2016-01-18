using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.DAL.Entities;

namespace ReportingTool.DAL
{
    class Program
    {
        static void Main(string[] args)
        {
            JiraClient client = new JiraClient("http://ssu-jira.softserveinc.com", "ybobrtc", "azv3sx0b");
            int count = 0;
            int cursor = 0;
            int i = 1;
            do
            {
                count = client.GetUsernames("RVNETJAN", cursor).Count;
                foreach (var u in client.GetUsernames("RVNETJAN", cursor))
                {
                    Console.WriteLine(i + ": " + u.name);
                    i++;
                }
                cursor += 1000;
            } while (count >= 1000);

        }
    }
}

using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace ReportingTool.DAL.Entities
{
    [JsonObject]
    public class JiraUser
    {
        public string name { get; set; }

        public string displayName { get; set; }
    }
}

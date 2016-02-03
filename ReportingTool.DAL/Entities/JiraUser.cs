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
        [JsonProperty(PropertyName = "userName")]
        public string name { get; set; }

        [JsonProperty(PropertyName = "fullName")]
        public string displayName { get; set; }
    }
}

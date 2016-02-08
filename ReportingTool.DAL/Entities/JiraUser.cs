using Newtonsoft.Json;

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

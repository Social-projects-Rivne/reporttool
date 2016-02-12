using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
    public class Team
    {
        public Team()
        {
            this.Members = new HashSet<Member>();
        }
        [JsonProperty("teamID")]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonProperty("teamName")]
        public string Name { get; set; }

        [Required]
       // [JsonProperty("isActive")]
        [JsonIgnore]
        public bool IsActive { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonProperty("projectKey")]
        public string ProjectKey { get; set; }

        public virtual ICollection<Member> Members { get; set; }
    }
}
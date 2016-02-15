using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
    // changes by ohariv
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
        [JsonIgnore]
        public bool IsActive { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonIgnore]
        public string ProjectKey { get; set; }

        [JsonProperty("members")]
        public virtual ICollection<Member> Members { get; set; }
    }
}
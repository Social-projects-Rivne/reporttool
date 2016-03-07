using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [Index("IX_NameAndProjectKey", 1, IsUnique = true)]
        public string Name { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActive { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonIgnore]
        [Index("IX_NameAndProjectKey", 2, IsUnique = true)]
        public string ProjectKey { get; set; }

        [JsonProperty("members")]
        public virtual ICollection<Member> Members { get; set; }
    }
}
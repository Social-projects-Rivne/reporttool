using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
    public class Member
    {
        public Member()
        {
            this.Teams = new HashSet<Team>();
        }

        public Member(string userName, string fullName)
        {
            this.UserName = userName;
            this.FullName = fullName;
        }
        [JsonProperty("memberID")]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonProperty("memberUserName")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonProperty("memberFullName")]
        public string FullName { get; set; }

        [Required]
        [JsonProperty("memberIsActive")]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<Team> Teams { get; set; }
    }
}


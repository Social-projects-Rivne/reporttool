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
        //[JsonProperty("memberID")]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [Required]
        [MaxLength(128)]
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [Required]
        [JsonIgnore]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<Team> Teams { get; set; }
    }
}


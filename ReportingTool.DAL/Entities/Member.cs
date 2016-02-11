using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data.Entity;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
    [Table("members", Schema = "public")]
    public partial class Member
    {
        public Member()
        {
            this.Teams = new HashSet<Team>();
        }

        public Member(string userName, string fullName) {
            this.userName = userName;
            this.fullName = fullName;
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("id")]
        [JsonProperty("memberID")]
        public int Id { get; set; }

        [Required]
        [Column("username")]
        [MinLength(4)]
        [MaxLength(50)]
        [JsonProperty("memberUserName")]
        public string userName { get; set; }

        [Required]
        [Column("fullname")]
        [MinLength(4)]
        [MaxLength(100)]
        [JsonProperty("memberFullName")]
        public string fullName { get; set; }

        [Required]
        [Column("isactive")]
        [JsonProperty("memberIsActive")]
        public bool IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<Team> Teams { get; set; }

    }

}


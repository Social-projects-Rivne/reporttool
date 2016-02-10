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

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("id")]
        public int Id { get; set; }

        [Required]
        [Column("username")]
        [MinLength(4)]
        [MaxLength(50)]
        public string Username { get; set; }

        [Required]
        [Column("fullname")]
        [MinLength(4)]
        [MaxLength(100)]
        public string Fullname { get; set; }

        [Required]
        [Column("isactive")]
        public bool IsActive { get; set; }

        public virtual ICollection<Team> Teams { get; set; }

    }

    [Table("members", Schema = "public")]
    public partial class member
    {
        public member()
        {
            this.teams = new HashSet<team>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("id")]
        [JsonProperty("memberID")]
        public int id { get; set; }

        [Required]
        [Column("username")]
        [MinLength(4)]
        [MaxLength(50)]
        [JsonProperty("memberUserName")]
        public string username { get; set; }

        [Required]
        [Column("fullname")]
        [MinLength(4)]
        [MaxLength(100)]
        [JsonProperty("memberFullName")]
        public string fullname { get; set; }

        [Required]
        [Column("isactive")]
        [JsonProperty("memberIsActive")]
        public bool isactive { get; set; }

        [JsonIgnore]
        public virtual ICollection<team> teams { get; set; }

    }
}


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
        [JsonProperty("memberID")]
        [JsonIgnore]        //  update
        public int Id { get; set; }

        [Required]
        [Column("username")]
        [MinLength(4)]
        [MaxLength(50)]
        [JsonProperty("memberUserName")]
        public string Username { get; set; }

        [Required]
        [Column("fullname")]
        [MinLength(4)]
        [MaxLength(100)]
        [JsonProperty("memberFullName")]
        public string Fullname { get; set; }

        [Required]
        [Column("isactive")]
        [JsonProperty("memberIsActive")]
        [JsonIgnore]        //  update
        public bool IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<Team> Teams { get; set; }

    }

}


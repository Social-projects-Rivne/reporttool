using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
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
        [JsonProperty("userName")]
        public string UserName { get; set; }

        [Required]
        [Column("fullname")]
        [MinLength(4)]
        [MaxLength(100)]
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [Required]
        [Column("isactive")]
        [JsonProperty("isActive")]
        [JsonIgnore]        //  update
        public bool IsActive { get; set; }

        [JsonIgnore]
        public virtual ICollection<Team> Teams { get; set; }

    }


    //  changes by ohariv
    //public class Member
    //{
    //    public Member()
    //    {
    //        this.Teams = new HashSet<Team>();
    //    }

    //    public Member(string userName, string fullName)
    //    {
    //        this.UserName = userName;
    //        this.FullName = fullName;
    //    }
        
    //    [JsonIgnore]
    //    public int Id { get; set; }

    //    [Required]
    //    [MaxLength(128)]
    //    [JsonProperty("userName")]
    //    public string UserName { get; set; }

    //    [Required]
    //    [MaxLength(128)]
    //    [JsonProperty("fullName")]
    //    public string FullName { get; set; }

    //    [Required]
    //    [JsonIgnore]
    //    public bool IsActive { get; set; }

    //    [JsonIgnore]
    //    public virtual ICollection<Team> Teams { get; set; }
    //}
}


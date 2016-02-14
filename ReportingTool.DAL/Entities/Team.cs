using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
    //  old working thing
    [Table("teams", Schema = "public")]
    public partial class Team
    {
        public Team()
        {
            this.Members = new HashSet<Member>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("id")]
        [JsonProperty("teamID")]
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MinLength(4)]
        [MaxLength(50)]
        [JsonProperty("teamName")]
        public string Name { get; set; }

        [Required]
        [Column("projectkey")]
        [MinLength(4)]
        [MaxLength(50)]
        [JsonProperty("projectKey")]
        [JsonIgnore]        //  update
        public string ProjectKey { get; set; }

        [Required]
        [Column("isactive")]
        [JsonProperty("isActive")]
        [JsonIgnore]        //  update
        public bool IsActive { get; set; }

        [JsonProperty("members")]
        public virtual ICollection<Member> Members { get; set; }

    }


    // changes by ohariv
    //public class Team
    //{
    //    public Team()
    //    {
    //        this.Members = new HashSet<Member>();
    //    }
    //    [JsonProperty("teamID")]
    //    public int Id { get; set; }

    //    [Required]
    //    [MaxLength(128)]
    //    [JsonProperty("teamName")]
    //    public string Name { get; set; }

    //    [Required]
    //    [JsonIgnore]
    //    public bool IsActive { get; set; }

    //    [Required]
    //    [MaxLength(128)]
    //    [JsonIgnore]
    //    public string ProjectKey { get; set; }

    //      [JsonProperty("members")]
    //    public virtual ICollection<Member> Members { get; set; }
    //}
}
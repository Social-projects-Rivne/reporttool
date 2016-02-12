using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

using System.Data.Entity;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{
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
        //[JsonProperty("projectKey")]
        [JsonIgnore]
        public string ProjectKey { get; set; }

        [Required]
        [Column("isactive")]
       // [JsonProperty("isActive")]
        [JsonIgnore]
        public bool IsActive { get; set; }

        public virtual ICollection<Member> Members { get; set; }

    }

}


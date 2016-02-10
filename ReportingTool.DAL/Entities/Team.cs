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
        public int Id { get; set; }

        [Required]
        [Column("name")]
        [MinLength(4)]
        [MaxLength(50)]
        public string Name { get; set; }

        [Required]
        [Column("projectkey")]
        [MinLength(4)]
        [MaxLength(50)]
        public string ProjectKey { get; set; }

        [Required]
        [Column("isactive")]
        public bool IsActive { get; set; }

        public virtual ICollection<Member> Members { get; set; }

    }

    [Table("teams", Schema = "public")]
    public partial class team
    {
        public team()
        {
            this.members = new HashSet<member>();
        }

        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [Required]
        [Column("id")]
        [JsonProperty("teamID")]
        public int id { get; set; }

        [Required]
        [Column("name")]
        [MinLength(4)]
        [MaxLength(50)]
        [JsonProperty("teamName")]
        public string name { get; set; }

        [Required]
        [Column("projectkey")]
        [MinLength(4)]
        [MaxLength(50)]
        [JsonProperty("projectKey")]
        public string projectkey { get; set; }

        [Required]
        [Column("isactive")]
        [JsonProperty("isActive")]
        public bool isactive { get; set; }

        public virtual ICollection<member> members { get; set; }

    }
}


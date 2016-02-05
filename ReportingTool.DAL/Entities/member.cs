using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;
using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities
{

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


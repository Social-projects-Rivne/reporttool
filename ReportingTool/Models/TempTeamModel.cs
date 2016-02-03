using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReportingTool.Models
{
    public class TempTeamModel
    {
        public TempTeamModel() { }
        public TempTeamModel(int teamID, string teamName) {
            this.teamID = teamID;
            this.teamName = teamName;
        }

        public int teamID;
        [Required]
        public string teamName;
        [Required]
        public List<TempMemberModel> members = new List<TempMemberModel>();
    }

    public class TempMemberModel
    {
        public TempMemberModel() { }
        public TempMemberModel(string userName, string fullName) {
            this.userName = userName;
            this.fullName = fullName;
        }

        public string userName;
        public string fullName;
    }
}
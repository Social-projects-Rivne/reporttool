using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReportingTool.Models
{
    public class TempTeamModel
    {
        public int teamID { get; set; }
        [Required]
        public string teamName { get; set; }
        [Required]
        private List<TempMemberModel> _members = new List<TempMemberModel>();

        public List<TempMemberModel> members
        {
            get { return _members; }
            set { _members = value; }
        }
        public TempTeamModel() { }
        
        public TempTeamModel(string teamID, string teamName)
        {
            this.teamID = int.Parse(teamID);
            this.teamName = teamName;
        }

        public TempTeamModel(string teamID, string teamName, List<TempMemberModel> members)
        {
            this.teamID = int.Parse(teamID);
            this.teamName = teamName;
            this._members = members;
        }

    }

    public class TempMemberModel
    {
        public TempMemberModel() { }
        public TempMemberModel(string userName, string fullName) {
            this.userName = userName;
            this.fullName = fullName;
        }

        public string userName { get; set; }
        public string fullName { get; set; }
    }
}
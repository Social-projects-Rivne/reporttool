using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace ReportingTool.Models
{
    public class TempTeamModel
    {

        public int teamID;
        [Required]
        public string teamName;
        [Required]
        public List<TempMemberModel> members;
    }

    public class TempMemberModel
    {
        public string userName;
        public string fullName;
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

//using Newtonsoft.Json;

namespace ReportingTool.DAL.Entities.JSON.Models
{
    public partial class TeamJSM
    {
        public List<MemberJSM> members = null;
        //public member[] Members = {};

        public TeamJSM()
        {
            members = new List<MemberJSM>();
        }

        public int         teamID { get; set; }
        public string   teamName { get; set; }
        public string   projectKey { get; set; }
        public bool      isActive { get; set; }

    }
}

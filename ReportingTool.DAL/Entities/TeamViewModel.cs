using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.DAL.Entities
{

    public partial class TeamViewModel
    {
        public List<Member> Members = null;

        public TeamViewModel()
        {
            Members = new List<Member>();
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string ProjectKey { get; set; }
        public bool IsActive { get; set; }

         //public List<Member> Members 
         //{ 
         //    get {return members};
         //    set; 
         //}

    }
}

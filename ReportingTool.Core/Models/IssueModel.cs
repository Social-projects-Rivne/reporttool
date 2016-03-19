using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReportingTool.Core.Models
{
    public class IssueModel
    {
        public string key { set; get; }
        public string summary { set; get; }
        public string status { set; get; }
        public int loggedTime { set; get; }
    }
}

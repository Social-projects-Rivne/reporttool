using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Data.Entity;

namespace ReportingTool.DAL.Entities.JSON.Models
{
    public class MemberJSM
    {
        public int memberID { get; set; }
        public string userName { get; set; }
        public string fullName { get; set; }
        public bool isActive { get; set; }
    }
}




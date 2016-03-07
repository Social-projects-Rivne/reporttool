using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace ReportingTool.Core.Services
{
    public class SessionHelper
    {
        private static HttpContext context;

        public static HttpContext Context
        {
            set
            {
                context = value;
            }

            get
            {
                return context;
            }
        }
    }
}

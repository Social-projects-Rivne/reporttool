using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.SessionState;

namespace ReportingTool.Core.Services
{
   public static class SessionHelper
    {
        private static HttpSessionStateBase session;


        public static HttpSessionStateBase Session
        {
            set 
            {
                session = value;
            }

            get
            {
                return session;
            }
        }
    }
}

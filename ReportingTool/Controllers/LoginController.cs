using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ReportingTool.Models;
using System.Diagnostics;
using Jira.SDK;
using Jira.SDK.Domain;
using System.Web.Security;
using System.Threading;
using IniParser;
using IniParser.Model;
using System.Web.Hosting;


namespace ReportingTool.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private string FILE_NAME = HostingEnvironment.MapPath("~/Configurations.ini"); 
        private const string SECTION = "GeneralConfiguration";
        private const string SERVEL_URL_KEY = "ServerUrl";

        private string getServerUrl()
        {
            FileIniDataParser fileIniData = new FileIniDataParser();
            IniData parsedData = fileIniData.ReadFile(FILE_NAME);
            return parsedData[SECTION][SERVEL_URL_KEY];
        }

        private bool IsUserValid(string userName, string password)
        {
            var server = getServerUrl();

            Jira.SDK.Jira jira = new Jira.SDK.Jira();

            try
            {
                jira.Connect(server, userName, password);
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                return false;
            }

            return true;     
        }

        private bool ConnectionExists(string server)
        {
            Jira.SDK.Jira jira = new Jira.SDK.Jira();
            try
            {
                jira.Connect(getServerUrl(), "test", "test");
            }
            catch (System.Net.WebException)
            {
                return false;
            }
            catch (System.Runtime.Serialization.SerializationException)
            {
                return true;
            }
            return true;
        }

        [AllowAnonymous]
        [HttpGet]
        public ActionResult Index() 
        { 
            return View(); 
        }

        [AllowAnonymous]
        [HttpPost]
        public JsonResult CheckCredentials(Credentials credentials)
        {
            if (!ConnectionExists(""))
            {
                return Json(new { Status = "connectionError" });
            }
            else
            {
                bool isUserValid = IsUserValid(credentials.UserName, credentials.Password);

                if (isUserValid)
                {
                    FormsAuthentication.SetAuthCookie(credentials.UserName, false);
                    Session.Add("currentUser", credentials.UserName);
                    return Json(new { Status = "validCredentials" });
                }
                return Json(new { Status = "invalidCredentials" });
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public JsonResult CheckSession()
        {
            string value = Session["currentUser"] as string;

            if (String.IsNullOrEmpty(value))
            {
                // null or empty
                return Json(new { Status = "sessionNotExists" }, JsonRequestBehavior.AllowGet);
            }
            else
                return Json(new { Status = "sessionExists" }, JsonRequestBehavior.AllowGet);
        }

        [AllowAnonymous]
        [HttpGet]
        public JsonResult Logout()
        {
            Session.Abandon();
            FormsAuthentication.SignOut();
            System.Web.HttpContext.Current.User = null;
            return Json(new { Status = "loggedOut" }, JsonRequestBehavior.AllowGet);
        }
	}
}
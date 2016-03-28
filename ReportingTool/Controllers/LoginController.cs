using System.Net;
using System.Runtime.Serialization;
using System.Security.Principal;
using System.Web.Hosting;
using System.Web.Mvc;
using System.Web.Security;
using IniParser;
using ReportingTool.DAL.DataAccessLayer;
using ReportingTool.Models;
using System;

namespace ReportingTool.Controllers
{
    [Authorize]
    public class LoginController : Controller
    {
        private string FILE_NAME = HostingEnvironment.MapPath("~/Configurations.ini");
        private const string SECTION = "GeneralConfiguration";
        private const string SERVEL_URL_KEY = "ServerUrl";
        private const string PROJECT_NAME_KEY = "ProjectName";

        private string getServerUrl()
        {
            var fileIniData = new FileIniDataParser();
            var parsedData = fileIniData.ReadFile(FILE_NAME);
            return parsedData[SECTION][SERVEL_URL_KEY];
        }

        private string getProjectKey()
        {
            var fileIniData = new FileIniDataParser();
            var parsedData = fileIniData.ReadFile(FILE_NAME);
            return parsedData[SECTION][PROJECT_NAME_KEY];
        }

        private bool isUserValidated(string userName, string password)
        {
            var server = getServerUrl();
            var jira = new Jira.SDK.Jira();

            try
            {
                jira.Connect(server, userName, password);
            }
            catch (SerializationException)
            {
                return false;
            }

            return true;
        }

        private void definePrincipal(string login)
        {
            IIdentity id = new GenericIdentity(login);
            var roles = new[] { "existing user" };
            System.Web.HttpContext.Current.User = new GenericPrincipal(id, roles);
        }

        private bool connectionExists(string server)
        {
            var jira = new Jira.SDK.Jira();
            try
            {
                jira.Connect(getServerUrl(), "test", "test");
            }
            catch (WebException)
            {
                return false;
            }
            catch (SerializationException)
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
            if (!connectionExists(""))
            {
                return Json(new { Status = "connectionError" });
            }
            var isUserValid = isUserValidated(credentials.UserName, credentials.Password);
            var isUserAuthenticated = (System.Web.HttpContext.Current.User != null) &&
                                       System.Web.HttpContext.Current.User.Identity.IsAuthenticated;

            if (isUserValid || isUserAuthenticated)
            {
                var client = new JiraClient(getServerUrl(), credentials.UserName, credentials.Password);

                definePrincipal(credentials.UserName);
                FormsAuthentication.SetAuthCookie(credentials.UserName, false);

                Session.Add("currentUser", credentials.UserName);
                Session.Add("projectKey", getProjectKey());
                Session.Add("jiraClient", client);

                return Json(new { Status = "validCredentials" });
            }
            return Json(new { Status = "invalidCredentials" });
        }
        [AllowAnonymous]
        [HttpGet]
        public JsonResult CheckSession()
        {
            var userInSession = Session["currentUser"] as string;
            var userInHTTPContext = System.Web.HttpContext.Current.User.Identity.Name;

            if (String.IsNullOrEmpty(userInSession) || String.IsNullOrEmpty(userInHTTPContext))
                return Json(new { Status = "sessionNotExists" }, JsonRequestBehavior.AllowGet);

            if (userInSession.Equals(userInHTTPContext))
                return Json(new { Status = "sessionExists" }, JsonRequestBehavior.AllowGet);

            return Json(new { Status = "sessionNotExists" }, JsonRequestBehavior.AllowGet);
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
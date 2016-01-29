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


namespace ReportingTool.Controllers
{
    //[Authorize]
    public class LoginController : Controller
    {
        private bool IsUserValid(string server, string userName, string password)
        {
            server = "http://ssu-jira.softserveinc.com";

            Jira.SDK.Jira jira = new Jira.SDK.Jira();

            try
            {
                //to do: get Jira server from configuration file
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
            server = "http://ssu-jira.softserveinc.com";
            Jira.SDK.Jira jira = new Jira.SDK.Jira();
            try
            {
                //to do: get Jira server from configuration file
                jira.Connect(server, "test", "test");
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
    
        [HttpGet]
        public ActionResult Index() { 
            return View(); }

        [HttpPost]
        public JsonResult CheckCredentials(Credentials credentials)
        {
            if (!ConnectionExists(""))
            {
                return Json(new { Status = "connectionError" });
            }
            else
            {
                bool isUserValid = IsUserValid("as", credentials.UserName, credentials.Password);
                if (isUserValid)
                {
                    FormsAuthentication.SetAuthCookie(credentials.UserName, false);
                    return Json(new { Status = "validCredentials" });
                }
                return Json(new { Status = "invalidCredentials" });
            }
        }
        [AllowAnonymous]
        [HttpGet]
        public ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
	}
}
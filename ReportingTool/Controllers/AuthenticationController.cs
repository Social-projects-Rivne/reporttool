using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ReportingTool.Controllers
{
    [RoutePrefix("authentication")]
    public class AuthenticationController : Controller
    {
        [/*Route("login"),*/ HttpGet]
        public ActionResult Index()
        {
            return View();
        }
    }
}
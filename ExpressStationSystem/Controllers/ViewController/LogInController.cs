using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class LogInController : Controller
    {
        // GET: LogIn
        public ActionResult AfterLogin()
        {
            return View();
        }

        public ActionResult Login()
        {
            return View();
        }

        public ActionResult changePassword()
        {
            return View();
        }
    }
}
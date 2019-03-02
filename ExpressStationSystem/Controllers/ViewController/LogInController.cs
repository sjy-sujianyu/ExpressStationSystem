using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressStationSystem.Controllers;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class LogInController : Controller
    {
        [HttpPost]
        public ActionResult AfterLogin(FormCollection form)
        {
            string phone = Request.Form["UserPhone"];
            string passWord = Request.Form["Password"];
            if (new LoginController().LandOfManager(phone, passWord))
            {
                return View();
            }
            else
            {
                return Content(string.Format("<script>alert('登陆失败');parent.window.location='Login';</script>"));
            }
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
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult AllMember()
        {
            return View();
        }

        public ActionResult AddMember()
        {
            return View();
        }
    }
}
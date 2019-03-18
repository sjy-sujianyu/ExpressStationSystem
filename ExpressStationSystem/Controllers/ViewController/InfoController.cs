using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class InfoController : Controller
    {
        // GET: Info
        public ActionResult AllInfo()
        {
            return View();
        }
    }
}
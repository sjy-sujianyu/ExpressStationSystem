using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            ViewBag.Title = "Home Page";
            string text = Global.publishMessage;
            if(text != null && text.Contains('|'))
            {
                string[] text2 = text.Split('|');
                ViewBag.title = text2[0];
                ViewBag.content = text2[1];
            }
            else
            {
                ViewBag.title = "无公告";
                ViewBag.content = text;
            }
            return View();
        }
    }
}

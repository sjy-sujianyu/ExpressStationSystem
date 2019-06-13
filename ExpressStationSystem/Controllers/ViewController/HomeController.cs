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
            string textL = Global.pulibshLeaveMessage;
            if (text != null && text.Contains('|'))
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
            if (textL != null && textL.Contains('|'))
            {
                string[] text3 = textL.Split('|');
                ViewBag.Ltitle = text3[0];
                ViewBag.Lcontent = text3[1];
            }
            else
            {
                ViewBag.Ltitle = "无公告";
                ViewBag.Lcontent = text;
            }
            return View();
        }
    }
}

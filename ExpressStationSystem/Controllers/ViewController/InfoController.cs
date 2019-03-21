using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
        public ActionResult AllInfo(string date1,string date2)
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            if (date1 == null || date1 == "")
            {
                date1 = DateTime.Now.ToString("yyyy-MM-dd");
            }
            if (date2 == null || date2 == "")
            {
                date2 = DateTime.Now.ToString("yyyy-MM-dd");
            }
            ViewBag.date1 = date1;
            ViewBag.date2 = date2;
            var number = new QueryController().GetStatistic(Convert.ToDateTime(date1),Convert.ToDateTime(date2));
            var jsonStr = JsonConvert.SerializeObject(number);
            JObject jo = (JObject)JsonConvert.DeserializeObject(jsonStr);
            ViewBag.number = jo;
            return View();
        }

        public ActionResult ModifyCommision()
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            return View();
        }

        private Boolean checkCookies()
        {
            if (Request.Cookies["MyCook"] != null)
            {
                if (new LoginController().LandOfManager(Request.Cookies["MyCook"]["userid"], Request.Cookies["MyCook"]["password"]))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
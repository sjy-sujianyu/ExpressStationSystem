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
            if (date1 == null)
            {
                date1 = "1999-11-11";
                date2 = "2019-03-18";
            }
            var number = new QueryController().GetStatistic(Convert.ToDateTime(date1),Convert.ToDateTime(date2));
            var jsonStr = JsonConvert.SerializeObject(number);
            JObject jo = (JObject)JsonConvert.DeserializeObject(jsonStr);
            ViewBag.number = jo;
            return View();
        }

        public ActionResult ModifyCommision()
        {
            return View();
        }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class CarController : Controller
    {
        string lastStatus = "";
        // GET: Car
        public ActionResult Car(string status, string searchWithType, string searchWithID)
        {
            if(status != null)
            {
                lastStatus = status;
            }

            //得到返回的信息
            

            return View();
        }
    }
}
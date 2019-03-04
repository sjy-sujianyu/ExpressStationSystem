using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressStationSystem.Controllers;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class PackageController : Controller
    {
        public ActionResult Package()
        {
            List<int> packagesID = new ManagerController().GetAllPackage();
            List<string> nameList = new List<string>();
            List<string> phoneList = new List<string>();
            List<DateTime> timeList = new List<DateTime>();
            List<string> statusList = new List<string>();
            
            foreach (var one in packagesID)
            {
                if(new QueryController().GetAllInfo(one).error == null)
                {
                    statusList.Add(new QueryController().GetAllInfo(one).package.status);
                }
                else
                {
                    statusList.Add(new QueryController().GetAllInfo(one).error.status);
                }

                nameList.Add(new QueryController().GetAllInfo(one).dest.name);

                phoneList.Add(new QueryController().GetAllInfo(one).dest.phone);

                timeList.Add(new QueryController().GetAllInfo(one).package.time);
                
            }
            ViewBag.packagesID = packagesID;
            ViewBag.nameList = nameList;
            ViewBag.phoneList = phoneList;
            ViewBag.timeList = timeList;
            ViewBag.statusList = statusList;
            return View();
        }
        
    }
}
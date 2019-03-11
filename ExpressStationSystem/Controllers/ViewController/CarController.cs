using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressStationSystem.Controllers;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class CarController : Controller
    {
        string defaultSearchWith = "按车牌";
        string defaultSearchWithContent = "";
        string defaultStatus = "选择分类";
        // GET: Car
        public ActionResult Car(string status, string searchWith, string searchWithContent)
        {
            if(status == "undefined" || status == null)
            {
                ViewBag.status = defaultStatus;
            }
            else
            {
                ViewBag.status = status;
            }
            
            if (searchWith == null)
            {
                ViewBag.searchWith = defaultSearchWith;
            }
            else
            {
                ViewBag.searchWith = searchWith;
            }
            if(searchWithContent == null)
            {
                ViewBag.searchWithContent = defaultSearchWithContent;
            }
            else
            {
                ViewBag.searchWithContent = searchWithContent;
            }
            //得到返回的车辆信息
            var carInfoList = new VehicleController().GetAllVehicle();
            //决定显示的car
            List<dynamic> step = new List<dynamic>();
            List<dynamic> showCar = new List<dynamic>();
            //第一次筛选
            foreach (var car in carInfoList)
            {
                //在用
                if (status == "在用中")
                {
                    if (car.onDuty)
                    {
                        step.Add(car);
                    }
                    
                }
                //闲置
                else if(status == "闲置中")
                {
                    if (!car.onDuty)
                    {
                        step.Add(car);
                    }
                }
                //显示全部，不筛选
                else
                {
                    step.Add(car);
                }
            }

            foreach(var car in step)
            {
                if (searchWith != null)
                {
                    if (searchWith == "按类型")
                    {
                        if (car.type.ToString().StartsWith(searchWithContent))
                        {
                            showCar.Add(car);
                        }
                    }
                    else if (searchWith == "按车牌")
                    {
                        if (car.plateNumber.ToString().StartsWith(searchWithContent))
                        {
                            showCar.Add(car);
                        }
                    }
                }
                else
                {
                    showCar.Add(car);
                }
            }


            ViewBag.showCar = showCar;
            return View();
        }
        public ActionResult AddCar()
        {
            return View();
        }
    }
}
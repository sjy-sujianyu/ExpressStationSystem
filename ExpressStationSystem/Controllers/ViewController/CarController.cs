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
        public ActionResult Car(string status, string searchWith, string searchWithContent, string page)
        {
            int pageNum = 10;
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            if (status == "undefined" || status == null)
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
            List<dynamic> showCar2 = new List<dynamic>();
            //第一次筛选
            foreach (var car in carInfoList)
            {
                if(car.isDelete)
                {
                    continue;
                }
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

            //默认在第一页
            if (page == null || page == "" || Convert.ToInt32(page) <= 0)
            {
                page = "1";
            }
            //临时数据
            int pageTemp = Convert.ToInt32(page);
            //第三次筛选，是把该页的信息的插进临时数组
            for (int i = (pageTemp - 1) * pageNum; i < pageTemp * pageNum; i++)
            {
                if (i >= showCar.Count)
                {
                    break;
                }
                else
                {
                    showCar2.Add(showCar[i]);
                }
            }
            //前端要用到的数据
            if (showCar.Count % pageNum == 0)
            {
                ViewBag.PageSum = showCar.Count / pageNum;
            }
            else
            {
                ViewBag.PageSum = showCar.Count / pageNum + 1;
            }
            ViewBag.currentPage = pageTemp;
            ViewBag.showCar = showCar2;
            return View();
        }
        public ActionResult AddCar()
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            return View();
        }
    }
}
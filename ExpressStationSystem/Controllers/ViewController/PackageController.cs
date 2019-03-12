using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressStationSystem.Controllers;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class PackageController : Controller
    {
        public ActionResult Package(string status, string searchWith, string searchWithContent, string searchWithContent2)
        {
            string defaultSearchWith = "按姓名";
            string defaultSearchWithContent = "";
            string defaultStatus = "选择分类";

            if (status == "undefined" || status == null)
            {
                status = defaultStatus;
                ViewBag.status = status;
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
            if (searchWithContent == null)
            {
                ViewBag.searchWithContent = defaultSearchWithContent;
            }
            else
            {
                ViewBag.searchWithContent = searchWithContent;
            }
            //人员信息
            //List<int> PIDList = new ManagerController().GetAllPackage();
            //中间数组
            List<dynamic> step = new List<dynamic>();
            //决定显示的前端数组
            List<dynamic> showPackage = new List<dynamic>();
            //第一次筛选
            //foreach (var PID in PIDList)
            //{
            //    var PackageInfo = new QueryController().GetAllInfo(PID);
            //    var josnStr = JsonConvert.SerializeObject(PackageInfo);
            //    JObject jo = (JObject)JsonConvert.DeserializeObject(josnStr);

            //    if (status == PackageInfo.package.status)
            //    {
            //        step.Add(jo);
            //        continue;
            //    }
            //    else if (PackageInfo.error != null && status == PackageInfo.error.status)
            //    {
            //        step.Add(jo);
            //        continue;
            //    }
            //    else if (status == defaultStatus)
            //    {
            //        step.Add(jo);
            //        continue;
            //    }
            //}

            foreach (var PInfo in step)
            {
                if (searchWith != null)
                {
                    if (searchWith == "按单号")
                    {
                        if (PInfo.package.id.ToString().StartsWith(searchWithContent))
                        {
                            showPackage.Add(PInfo);
                        }
                    }
                    else if (searchWith == "按姓名")
                    {
                        if (PInfo.dest.name.ToString().StartsWith(searchWithContent))
                        {
                            showPackage.Add(PInfo);
                        }
                    }
                    else if (searchWith == "按手机")
                    {
                        if (PInfo.dest.phone.ToString().StartsWith(searchWithContent))
                        {
                            showPackage.Add(PInfo);
                        }
                    }
                    else if (searchWith == "按时间")
                    {
                        DateTime t1 = Convert.ToDateTime(PInfo.package.time.ToString());
                        DateTime t2 = Convert.ToDateTime(searchWithContent);
                        DateTime t3 = Convert.ToDateTime(searchWithContent2);
                        int compNum = DateTime.Compare(t1, t2);
                        int compNum2 = DateTime.Compare(t1, t3);
                        if (compNum >= 0 && compNum2 <=0)
                        {
                            showPackage.Add(PInfo);
                        }
                    }
                }
                else
                {
                    showPackage.Add(PInfo);
                }
            }
            ViewBag.showPackage = showPackage;
            return View();
            //nameList.Clear();
            //phoneList.Clear();
            //timeList.Clear();
            //statusList.Clear();

            ////获取最新的包裹列表
            //List<int> packagesID = new ManagerController().GetAllPackage();

            //foreach (var one in packagesID)
            //{
            //    var packageInfo = new QueryController().GetAllInfo(one);
            //    //包裹状态
            //    string packageStatus = packageInfo.package.status;
            //    if (status != null)
            //    {
            //        if (packageInfo.error == null)
            //        {
            //            errorStatusList.Add(0);
            //        }
            //        else
            //        {
            //            errorStatusList.Add(1);
            //        }
            //        //状态相同就添加数组
            //        if(packageStatus == status)
            //        {
            //            PIDList.Add(one);

            //            statusList.Add(packageStatus);

            //            nameList.Add(packageInfo.dest.name);

            //            phoneList.Add(packageInfo.dest.phone);

            //            timeList.Add(packageInfo.package.time);
            //        }
            //        continue;
            //    }
            //    else if(searchWithName != null)
            //    {
            //        if(packageInfo.dest.name.ToString().StartsWith(searchWithName))
            //        {
            //            if (packageInfo.error == null)
            //            {
            //                errorStatusList.Add(0);
            //            }
            //            else
            //            {
            //                errorStatusList.Add(1);
            //            }
            //            PIDList.Add(one);

            //            statusList.Add(packageInfo.package.status);

            //            nameList.Add(packageInfo.dest.name);

            //            phoneList.Add(packageInfo.dest.phone);

            //            timeList.Add(packageInfo.package.time);
            //        }
            //        continue;
            //    }
            //    else if(searchWithPhone != null)
            //    {
            //        if (packageInfo.dest.phone.ToString().StartsWith(searchWithPhone))
            //        {
            //            if (packageInfo.error == null)
            //            {
            //                errorStatusList.Add(0);
            //            }
            //            else
            //            {
            //                errorStatusList.Add(1);
            //            }

            //            PIDList.Add(one);

            //            statusList.Add(packageInfo.package.status);

            //            nameList.Add(packageInfo.dest.name);

            //            phoneList.Add(packageInfo.dest.phone);

            //            timeList.Add(packageInfo.package.time);
            //        }
            //        continue;
            //    }
            //    else if (searchWithPID != null)
            //    {
            //        if (one.ToString().ToString().StartsWith(searchWithPID))
            //        {
            //            if (packageInfo.error == null)
            //            {
            //                errorStatusList.Add(0);
            //            }
            //            else
            //            {
            //                errorStatusList.Add(1);
            //            }
            //            PIDList.Add(one);

            //            statusList.Add(packageInfo.package.status);

            //            nameList.Add(packageInfo.dest.name);

            //            phoneList.Add(packageInfo.dest.phone);

            //            timeList.Add(packageInfo.package.time);
            //        }
            //        continue;
            //    }
            //    else if (searchWithTime != null)
            //    {

            //        if (packageInfo.package.time.ToString().StartsWith(searchWithTime))
            //        {
            //            if (packageInfo.error == null)
            //            {
            //                errorStatusList.Add(0);
            //            }
            //            else
            //            {
            //                errorStatusList.Add(1);
            //            }
            //            PIDList.Add(one);

            //            statusList.Add(packageInfo.package.status);

            //            nameList.Add(packageInfo.dest.name);

            //            phoneList.Add(packageInfo.dest.phone);

            //            timeList.Add(packageInfo.package.time);
            //        }
            //        continue;
            //    }
            //    else
            //    {
            //        if (packageInfo.error == null)
            //        {
            //            errorStatusList.Add(0);
            //        }
            //        else
            //        {
            //            errorStatusList.Add(1);
            //        }
            //        PIDList.Add(one);

            //        statusList.Add(packageInfo.package.status);

            //        nameList.Add(packageInfo.dest.name);

            //        phoneList.Add(packageInfo.dest.phone);

            //        timeList.Add(packageInfo.package.time);

            //        continue;
            //    }

            //}

            //ViewBag.errorStatusList = errorStatusList;
            //ViewBag.packagesID = PIDList;
            //ViewBag.nameList = nameList;
            //ViewBag.phoneList = phoneList;
            //ViewBag.timeList = timeList;
            //ViewBag.statusList = statusList;
            //return View();
        }


        public ActionResult checkPackage(string id)
        {
            if (id == "" || id == null)
            {
                return View();
            }
            var pack = new QueryController().GetAllInfo(Convert.ToInt32(id));

            var josnStr = JsonConvert.SerializeObject(pack);
            JObject jo = (JObject)JsonConvert.DeserializeObject(josnStr);

            ViewBag.package = jo;

            return View();

        }

    }


}
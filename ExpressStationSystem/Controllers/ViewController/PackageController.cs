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
        List<int> PIDList = new List<int>();
        List<string> nameList = new List<string>();
        List<string> phoneList = new List<string>();
        List<DateTime> timeList = new List<DateTime>();
        List<string> statusList = new List<string>();
        List<int> errorStatusList = new List<int>();

        public ActionResult Package(string status, string searchWithName, string searchWithPhone, string searchWithPID, string searchWithTime)
        {
            nameList.Clear();
            phoneList.Clear();
            timeList.Clear();
            statusList.Clear();

            //获取最新的包裹列表
            List<int> packagesID = new ManagerController().GetAllPackage();

            foreach (var one in packagesID)
            {
                var packageInfo = new QueryController().GetAllInfo(one);
                //包裹状态
                string packageStatus = packageInfo.package.status;
                if (status != null)
                {
                    if (packageInfo.error == null)
                    {
                        errorStatusList.Add(0);
                    }
                    else
                    {
                        errorStatusList.Add(1);
                    }
                    //状态相同就添加数组
                    if(packageStatus == status)
                    {
                        PIDList.Add(one);

                        statusList.Add(packageStatus);

                        nameList.Add(packageInfo.dest.name);

                        phoneList.Add(packageInfo.dest.phone);

                        timeList.Add(packageInfo.package.time);
                    }
                    continue;
                }
                else if(searchWithName != null)
                {
                    if(packageInfo.dest.name.ToString().StartsWith(searchWithName))
                    {
                        if (packageInfo.error == null)
                        {
                            errorStatusList.Add(0);
                        }
                        else
                        {
                            errorStatusList.Add(1);
                        }
                        PIDList.Add(one);

                        statusList.Add(packageInfo.package.status);

                        nameList.Add(packageInfo.dest.name);

                        phoneList.Add(packageInfo.dest.phone);

                        timeList.Add(packageInfo.package.time);
                    }
                    continue;
                }
                else if(searchWithPhone != null)
                {
                    if (packageInfo.dest.phone.ToString().StartsWith(searchWithPhone))
                    {
                        if (packageInfo.error == null)
                        {
                            errorStatusList.Add(0);
                        }
                        else
                        {
                            errorStatusList.Add(1);
                        }

                        PIDList.Add(one);

                        statusList.Add(packageInfo.package.status);

                        nameList.Add(packageInfo.dest.name);

                        phoneList.Add(packageInfo.dest.phone);

                        timeList.Add(packageInfo.package.time);
                    }
                    continue;
                }
                else if (searchWithPID != null)
                {
                    if (one.ToString().ToString().StartsWith(searchWithPID))
                    {
                        if (packageInfo.error == null)
                        {
                            errorStatusList.Add(0);
                        }
                        else
                        {
                            errorStatusList.Add(1);
                        }
                        PIDList.Add(one);

                        statusList.Add(packageInfo.package.status);

                        nameList.Add(packageInfo.dest.name);

                        phoneList.Add(packageInfo.dest.phone);

                        timeList.Add(packageInfo.package.time);
                    }
                    continue;
                }
                else if (searchWithTime != null)
                {
                    
                    if (packageInfo.package.time.ToString().StartsWith(searchWithTime))
                    {
                        if (packageInfo.error == null)
                        {
                            errorStatusList.Add(0);
                        }
                        else
                        {
                            errorStatusList.Add(1);
                        }
                        PIDList.Add(one);

                        statusList.Add(packageInfo.package.status);

                        nameList.Add(packageInfo.dest.name);

                        phoneList.Add(packageInfo.dest.phone);

                        timeList.Add(packageInfo.package.time);
                    }
                    continue;
                }
                else
                {
                    if (packageInfo.error == null)
                    {
                        errorStatusList.Add(0);
                    }
                    else
                    {
                        errorStatusList.Add(1);
                    }
                    PIDList.Add(one);

                    statusList.Add(packageInfo.package.status);

                    nameList.Add(packageInfo.dest.name);

                    phoneList.Add(packageInfo.dest.phone);

                    timeList.Add(packageInfo.package.time);

                    continue;
                }
                
            }

            ViewBag.errorStatusList = errorStatusList;
            ViewBag.packagesID = PIDList;
            ViewBag.nameList = nameList;
            ViewBag.phoneList = phoneList;
            ViewBag.timeList = timeList;
            ViewBag.statusList = statusList;
            return View();
        }
        
    }
}
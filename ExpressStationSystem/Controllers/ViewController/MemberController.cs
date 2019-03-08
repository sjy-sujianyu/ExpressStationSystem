using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using ExpressStationSystem.Controllers;
using Microsoft.Ajax.Utilities;

namespace ExpressStationSystem.Controllers.ViewController
{
   
    public class MemberController : Controller
    {
        //List<string> showImgList = new List<string>();
        //List<string> showNameList = new List<string>();
        //List<string> showPhoneList = new List<string>();
        //List<string> showJobList = new List<string>();
        //List<string> showIsOnDutyList = new List<string>();

        //string leaveType = "";

        // GET: Member
        public ActionResult AllMember(string status, string searchWith, string searchWithContent)
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
            var memberIDList = new ManagerController().GetAllMember();
            //中间数组
            List<dynamic> step = new List<dynamic>();
            //决定显示的前端数组
            List<dynamic> showMen = new List<dynamic>();
            //第一次筛选
            foreach(var menID in memberIDList)
            {
                var menInfo = new QueryController().GetMemberAllInfo(menID);

                if (status == "休息中")
                {
                    if (!menInfo.onDuty)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if(status == "工作中")
                {
                    if (menInfo.onDuty)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == "已辞职")
                {
                    if (menInfo.isDelete)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if(status == defaultStatus)
                {
                    step.Add(menInfo);
                    continue;
                }
                else
                {
                    if(menInfo.job == status)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                
            }

            foreach(var menInfo in step)
            {
                if(searchWith != null)
                {
                    if (searchWith == "按姓名")
                    {
                        if (menInfo.name.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                    else if (searchWith == "按手机")
                    {
                        if (menInfo.mId.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                }
                else
                {
                    showMen.Add(menInfo);
                }
            }
            ViewBag.showMen = showMen;
            return View();
        }

        public ActionResult AddMember()
        {
            return View();
        }

        public ActionResult DeleteMember(string status, string searchWith, string searchWithContent)
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
            var memberIDList = new ManagerController().GetAllMember();
            //中间数组
            List<dynamic> step = new List<dynamic>();
            //决定显示的前端数组
            List<dynamic> showMen = new List<dynamic>();
            //第一次筛选
            foreach (var menID in memberIDList)
            {
                var menInfo = new QueryController().GetMemberAllInfo(menID);

                if (status == "休息中")
                {
                    if (!menInfo.onDuty)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == "工作中")
                {
                    if (menInfo.onDuty)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == "已辞职")
                {
                    if (menInfo.isDelete)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == defaultStatus)
                {
                    step.Add(menInfo);
                    continue;
                }
                else
                {
                    if (menInfo.job == status)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }

            }

            foreach (var menInfo in step)
            {
                if (searchWith != null)
                {
                    if (searchWith == "按姓名")
                    {
                        if (menInfo.name.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                    else if (searchWith == "按手机")
                    {
                        if (menInfo.mId.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                }
                else
                {
                    showMen.Add(menInfo);
                }
            }
            ViewBag.showMen = showMen;
            return View();
        }

        public ActionResult Mission(string status, string searchWith, string searchWithContent)
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
            var memberIDList = new ManagerController().GetAllMember();
            //中间数组
            List<dynamic> step = new List<dynamic>();
            //决定显示的前端数组
            List<dynamic> showMen = new List<dynamic>();
            //第一次筛选
            foreach (var menID in memberIDList)
            {
                var menInfo = new QueryController().GetMemberAllInfo(menID);

                if (status == defaultStatus && !menInfo.isDelete)
                {
                    step.Add(menInfo);
                    continue;
                }
                else
                {
                    if (menInfo.job == status)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }

            }
            foreach (var menInfo in step)
            {
                if (searchWith != null)
                {
                    if (searchWith == "按姓名")
                    {
                        if (menInfo.name.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                    else if (searchWith == "按手机")
                    {
                        if (menInfo.mId.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                }
                else
                {
                    showMen.Add(menInfo);
                }
            }
            ViewBag.showMen = showMen;
            return View();
            ////重新请求数据库，获取员工ID
            //List<string> MID = new ManagerController().GetAllMember();

            ////清空原本数组
            //showImgList.Clear();
            //showNameList.Clear();
            //showPhoneList.Clear();
            //showJobList.Clear();
            //showIsOnDutyList.Clear();

            //foreach (var one in MID)
            //{
            //    var info = new QueryController().GetMemberAllInfo(one);
            //    if (info.job != "经理")
            //        if (!info.isDelete)
            //        {
            //            showImgList.Add(info.imagePath);
            //            showNameList.Add(info.name);
            //            showPhoneList.Add(info.mId);
            //            showJobList.Add(info.job);
            //            if (info.onDuty)
            //            {
            //                showIsOnDutyList.Add("工作中");
            //            }
            //            else
            //            {
            //                showIsOnDutyList.Add("休息中");
            //            }
            //        }
            //}

            //ViewBag.showImgList = showImgList;
            //ViewBag.showNameList = showNameList;
            //ViewBag.showPhoneList = showPhoneList;
            //ViewBag.showJobList = showJobList;
            //ViewBag.showIsOnDutyList = showIsOnDutyList;
        }

        public ActionResult Wages(string status, string searchWith, string searchWithContent)
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
            var memberIDList = new ManagerController().GetAllMember();
            //中间数组
            List<dynamic> step = new List<dynamic>();
            //决定显示的前端数组
            List<dynamic> showMen = new List<dynamic>();
            //第一次筛选
            foreach (var menID in memberIDList)
            {
                var menInfo = new QueryController().GetMemberAllInfo(menID);

                if (status == "休息中")
                {
                    if (!menInfo.onDuty)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == "工作中")
                {
                    if (menInfo.onDuty)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == "已辞职")
                {
                    if (menInfo.isDelete)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == defaultStatus)
                {
                    step.Add(menInfo);
                    continue;
                }
                else
                {
                    if (menInfo.job == status)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }

            }
            //先定好日期，当前月的的第一天
            DateTime date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            //第二次是搜索的筛选
            foreach (var menInfo in step)
            {
                if (searchWith != null)
                {
                    if (searchWith == "按姓名")
                    {
                        if (menInfo.name.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                    else if (searchWith == "按手机")
                    {
                        if (menInfo.mId.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                    else if(searchWith == "按月份")
                    {
                        //改变日期
                        string[] arr = searchWithContent.Split('-');
                        date = new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1);
                        showMen = step;
                        break;
                    }
                }
                else
                {
                    showMen.Add(menInfo);
                }
            }

            //获取工资详情
            List<dynamic> moneyInfo = new List<dynamic>();
            foreach(var man in showMen)
            {
                moneyInfo.Add(new MoneyController().GetSalary(man.mId, date));
            }

            ViewBag.moneyInfo = moneyInfo;
            ViewBag.showMen = showMen;
            return View();
            //重新请求数据库，获取员工ID
            //List<string> MID = new ManagerController().GetAllMember();

            //int sumPickUp = 0;
            //int sumTransfer = 0;
            //int sumDelivery = 0;
            //decimal sumSalary = 0;

            ////清空原本数组
            //showImgList.Clear();
            //showNameList.Clear();
            //showPhoneList.Clear();
            ////每个员工的统计
            //List<int> PickUpCount = new List<int>();
            //List<int> DeliveryCount = new List<int>();
            //List<int> TransferCount = new List<int>();
            //List<decimal> baseSalary = new List<decimal>();
            //List<decimal> subsidy = new List<decimal>();
            //List<decimal> fine = new List<decimal>();
            //List<dynamic> totalSalary = new List<dynamic>();

            //foreach(var one in MID)
            //{
            //    var info = new QueryController().GetMemberAllInfo(one);

            //    if (info.job != "经理" && !info.isDelete)
            //    {
            //        showImgList.Add(info.imagePath);
            //        showNameList.Add(info.name);
            //        showPhoneList.Add(info.mId);

            //        var money = new MoneyController().GetSalary(one, new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1));
            //        if (searchWithMonth != null)
            //        {
            //            string[] arr = searchWithMonth.Split('-');
            //            //如果是搜索月份的，就换
            //            money = new MoneyController().GetSalary(one, new DateTime(Convert.ToInt32(arr[0]), Convert.ToInt32(arr[1]), 1));
            //        }

            //        baseSalary.Add(money.baseSalary);
            //        PickUpCount.Add(money.commission.pickUp.PickUpCount);
            //        DeliveryCount.Add(money.commission.delivery.DeliveryCount);
            //        TransferCount.Add(money.commission.transfer.TransferCount);
            //        subsidy.Add(money.subsidy.total);
            //        fine.Add(money.fine.total);

            //        sumPickUp += money.commission.pickUp.PickUpCount;
            //        sumTransfer += money.commission.transfer.TransferCount;
            //        sumDelivery += money.commission.delivery.DeliveryCount;

            //        totalSalary.Add(money.baseSalary + money.commission.pickUp.total + money.commission.delivery.total + money.commission.transfer.total + money.subsidy.total - money.fine.total);

            //        sumSalary += (money.baseSalary + money.commission.pickUp.total + money.commission.delivery.total + money.commission.transfer.total + money.subsidy.total - money.fine.total);
            //    }

            //}

            //ViewBag.showImgList = showImgList;
            //ViewBag.showNameList = showNameList;
            //ViewBag.showPhoneList = showPhoneList;
            //ViewBag.baseSalary = baseSalary;
            //ViewBag.PickUpCount = PickUpCount;
            //ViewBag.DeliveryCount = DeliveryCount;
            //ViewBag.TransferCount = TransferCount;
            //ViewBag.subsidy = subsidy;
            //ViewBag.fine = fine;
            //ViewBag.totalSalary = totalSalary;

            //ViewBag.sumPickUp = sumPickUp;
            //ViewBag.sumTransfer = sumTransfer;
            //ViewBag.sumDelivery = sumDelivery;
            //ViewBag.sumSalary = sumSalary;
        }
        public static T JsonToObj<T>(string json)
        {
            JavaScriptSerializer jserializer = new JavaScriptSerializer();
            jserializer.MaxJsonLength = int.MaxValue;
            if (string.IsNullOrEmpty(json)) json = "{}";
            T obj = jserializer.Deserialize<T>(json);
            return obj;
        }

        public ActionResult Fine(string status, string searchWith, string searchWithContent)
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
            var memberIDList = new ManagerController().GetAllMember();
            //中间数组
            List<dynamic> step = new List<dynamic>();
            //决定显示的前端数组
            List<dynamic> showMen = new List<dynamic>();
            //第一次筛选
            foreach (var menID in memberIDList)
            {
                var menInfo = new QueryController().GetMemberAllInfo(menID);

                if (status == "休息中")
                {
                    if (!menInfo.onDuty)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == "工作中")
                {
                    if (menInfo.onDuty)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == "已辞职")
                {
                    if (menInfo.isDelete)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }
                else if (status == defaultStatus)
                {
                    step.Add(menInfo);
                    continue;
                }
                else
                {
                    if (menInfo.job == status)
                    {
                        step.Add(menInfo);
                        continue;
                    }
                }

            }

            foreach (var menInfo in step)
            {
                if (searchWith != null)
                {
                    if (searchWith == "按姓名")
                    {
                        if (menInfo.name.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                    else if (searchWith == "按手机")
                    {
                        if (menInfo.mId.ToString().StartsWith(searchWithContent))
                        {
                            showMen.Add(menInfo);
                        }
                    }
                }
                else
                {
                    showMen.Add(menInfo);
                }
            }
            ViewBag.showMen = showMen;
            return View();
            ////重新请求数据库，获取员工ID
            //List<string> MID = new ManagerController().GetAllMember();

            ////清空原本数组
            //showImgList.Clear();
            //showNameList.Clear();
            //showPhoneList.Clear();
            //showJobList.Clear();
            //showIsOnDutyList.Clear();

            //if (searchWithName != null)
            //{
            //    //按名字查找
            //    foreach (var one in MID)
            //    {
            //        var info = new QueryController().GetMemberAllInfo(one);
            //        if (info.name == searchWithName || info.name.StartsWith(searchWithName))
            //        {
            //            showImgList.Add(info.imagePath);
            //            showNameList.Add(info.name);
            //            showPhoneList.Add(info.mId);
            //            showJobList.Add(info.job);
            //            if (info.onDuty)
            //            {
            //                showIsOnDutyList.Add("工作中");
            //            }
            //            else
            //            {
            //                showIsOnDutyList.Add("休息中");
            //            }
            //        }
            //    }
            //    ViewBag.showImgList = showImgList;
            //    ViewBag.showNameList = showNameList;
            //    ViewBag.showPhoneList = showPhoneList;
            //    ViewBag.showJobList = showJobList;
            //    ViewBag.showIsOnDutyList = showIsOnDutyList;
            //    return View();
            //}
            //else if (searchWithPhone != null)
            //{
            //    //按手机号查询
            //    foreach (var one in MID)
            //    {
            //        var info = new QueryController().GetMemberAllInfo(one);
            //        if (info.mId == searchWithPhone || info.mId.StartsWith(searchWithPhone))
            //        {
            //            showImgList.Add(info.imagePath);
            //            showNameList.Add(info.name);
            //            showPhoneList.Add(info.mId);
            //            showJobList.Add(info.job);
            //            if (info.onDuty)
            //            {
            //                showIsOnDutyList.Add("工作中");
            //            }
            //            else
            //            {
            //                showIsOnDutyList.Add("休息中");
            //            }
            //        }
            //    }
            //    ViewBag.showImgList = showImgList;
            //    ViewBag.showNameList = showNameList;
            //    ViewBag.showPhoneList = showPhoneList;
            //    ViewBag.showJobList = showJobList;
            //    ViewBag.showIsOnDutyList = showIsOnDutyList;
            //    return View();
            //}

            ////默认情况
            //if (status == "已辞职")
            //{
            //    foreach (var one in MID)
            //    {
            //        if ((new QueryController().GetMemberAllInfo(one).isDelete))
            //        {
            //            showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
            //            showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
            //            showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
            //            showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
            //            if (new QueryController().GetMemberAllInfo(one).onDuty)
            //            {
            //                showIsOnDutyList.Add("工作中");
            //            }
            //            else
            //            {
            //                showIsOnDutyList.Add("休息中");
            //            }

            //        }
            //    }
            //}
            //else if (status == null)
            //{
            //    foreach (var one in MID)
            //    {
            //        if (!(new QueryController().GetMemberAllInfo(one).isDelete))
            //        {
            //            showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
            //            showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
            //            showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
            //            showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
            //            if (new QueryController().GetMemberAllInfo(one).onDuty)
            //            {
            //                showIsOnDutyList.Add("工作中");
            //            }
            //            else
            //            {
            //                showIsOnDutyList.Add("休息中");
            //            }
            //        }
            //    }
            //}
            //else if (status == "休息中")
            //{
            //    foreach (var one in MID)
            //    {
            //        if (!(new QueryController().GetMemberAllInfo(one).onDuty) && !(new QueryController().GetMemberAllInfo(one).isDelete))
            //        {
            //            showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
            //            showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
            //            showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
            //            showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
            //            showIsOnDutyList.Add(status);
            //        }
            //    }
            //}
            //else if (status == "工作中")
            //{
            //    foreach (var one in MID)
            //    {
            //        if ((new QueryController().GetMemberAllInfo(one).onDuty) && !(new QueryController().GetMemberAllInfo(one).isDelete))
            //        {
            //            showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
            //            showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
            //            showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
            //            showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
            //            showIsOnDutyList.Add(status);
            //        }
            //    }
            //}
            //else
            //{
            //    foreach (var one in MID)
            //    {
            //        if ((new QueryController().GetMemberAllInfo(one).job) == status && !(new QueryController().GetMemberAllInfo(one).isDelete))
            //        {
            //            showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
            //            showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
            //            showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
            //            showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
            //            if (new QueryController().GetMemberAllInfo(one).onDuty)
            //            {
            //                showIsOnDutyList.Add("工作中");
            //            }
            //            else
            //            {
            //                showIsOnDutyList.Add("休息中");
            //            }
            //        }
            //    }
            //}

            //ViewBag.showImgList = showImgList;
            //ViewBag.showNameList = showNameList;
            //ViewBag.showPhoneList = showPhoneList;
            //ViewBag.showJobList = showJobList;
            //ViewBag.showIsOnDutyList = showIsOnDutyList;

            return View();
        }

        public ActionResult DetailMember(string id)
        {
            if(id != null)
            {
                var member = new QueryController().GetMemberAllInfo(id);
                ViewBag.thisID = id;
                ViewBag.thisName = new QueryController().GetMemberAllInfo(id).name;
                ViewBag.thisImagePath = new QueryController().GetMemberAllInfo(id).imagePath;
                ViewBag.thisBaseSalary = new QueryController().GetMemberAllInfo(id).baseSalary;
                ViewBag.thisJob = new QueryController().GetMemberAllInfo(id).job;

                return View();
            }
            else
            {
                return View();
            }
        }

        public ActionResult changeMemberDetail(string id)
        {
            if(id == null)
            {
                id = "15813322560";
            }
            var member = new QueryController().GetMemberAllInfo(id);
            ViewBag.thisID = id;
            ViewBag.thisName = new QueryController().GetMemberAllInfo(id).name;
            ViewBag.thisImagePath = new QueryController().GetMemberAllInfo(id).imagePath;
            ViewBag.thisBaseSalary = new QueryController().GetMemberAllInfo(id).baseSalary;
            ViewBag.thisJob = new QueryController().GetMemberAllInfo(id).job;

            return View();
        }

        public ActionResult Leave(string status, string searchWith, string searchWithContent)
        {
            //if(type != null)
            //{
            //    leaveType = type;
            //}

            //List<string> applicantsImg = new List<string>();

            //List<string> applicantsID = new List<string>();
            //List<string> applicantsName = new List<string>();
            //List<string> applicantsJob = new List<string>();
            //List<string> applicantsReason = new List<string>();
            //List<DateTime> applicantsApplyTime = new List<DateTime>();
            //List<DateTime> applicantsStartTime = new List<DateTime>();
            //List<DateTime> applicantsEndTime = new List<DateTime>();
            
            //List<string> leaveStatus = new List<string>();
            //List<int> applicantsTimes = new List<int>();

            //List<int> checkLID = new List<int>();
            //if(leaveType == "history")
            //{
            //    //20年前
            //    DateTime oldTime = DateTime.Now.AddYears(-20);
            //    //当今时间
            //    DateTime now = DateTime.Now;
            //    //获取历史的LID

            //}
            //else
            //{
            //    //获取月份
            //    DateTime thisMonthStart = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
            //    DateTime thisMonthEnd = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);
            //    //获取未处理的LID

            //}

            ////返回历史
            //if (leaveType == "history")
            //{
            //    //20年前
            //    DateTime oldTime = DateTime.Now.AddYears(-20);
            //    //当今时间
            //    DateTime now = DateTime.Now;

            //    List<int> unFinishList = new List<int>();

            //    //遍历请假条的状态
            //    for(short status = 1; status <= 3; status++)
            //    {
            //        //得到历史lid列表
            //        var getList = new LeaveController().GetLeaveListByStatus(oldTime,now,status);

            //        foreach(var lid in getList)
            //        {
            //            var leaveInfo = new LeaveController().GetLeaveInfo(lid);

            //            if(searchWithDate != null && leaveInfo.leave.time.ToString().StartWith(searchWithDate))
            //            {
            //                var mid = new LeaveController().GetLeaveInfo(lid).member.mId;

            //                applicantsImg.Add(leaveInfo.member.imagePath);
            //                applicantsID.Add(mid);
            //                applicantsName.Add(leaveInfo.member.name);
            //                applicantsJob.Add(leaveInfo.member.job);
            //                applicantsReason.Add(leaveInfo.leave.reason);
            //                applicantsApplyTime.Add(leaveInfo.leave.time);
            //                applicantsStartTime.Add(leaveInfo.leave.srcTime);
            //                applicantsEndTime.Add(leaveInfo.leave.endTime);

            //                applicantsTimes.Add(new LeaveController().GetLeaveCount(mid, oldTime, now));

            //                if (leaveInfo.leave.status == 1)
            //                {
            //                    leaveStatus.Add("被拒绝");
            //                }
            //                else if (leaveInfo.leave.status == 2)
            //                {
            //                    leaveStatus.Add("申请成功");
            //                }
            //                else if (leaveInfo.leave.status == 3)
            //                {
            //                    leaveStatus.Add("已销假");
            //                }
            //                unFinishList.Add(lid);
            //            }
            
            //        }
            //    }
            //    ViewBag.unFinishLID = unFinishList;
            //}
            ////返回未处理
            //else
            //{
            //    //获取月份
            //    DateTime thisMonthStart = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
            //    DateTime thisMonthEnd = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);
            //    //得到未处理的请假条ID
            //    var unFinishLID = new LeaveController().GetLeaveList();

            //    foreach(var lid in unFinishLID)
            //    {
            //        var mid = new LeaveController().GetLeaveInfo(lid).member.mId;
            //        applicantsImg.Add(new LeaveController().GetLeaveInfo(lid).member.imagePath);
            //        applicantsID.Add(mid);
            //        applicantsName.Add(new LeaveController().GetLeaveInfo(lid).member.name);
            //        applicantsJob.Add(new LeaveController().GetLeaveInfo(lid).member.job);
            //        applicantsReason.Add(new LeaveController().GetLeaveInfo(lid).leave.reason);
            //        applicantsApplyTime.Add(new LeaveController().GetLeaveInfo(lid).leave.time);
            //        applicantsStartTime.Add(new LeaveController().GetLeaveInfo(lid).leave.srcTime);
            //        applicantsEndTime.Add(new LeaveController().GetLeaveInfo(lid).leave.endTime);

            //        applicantsTimes.Add(new LeaveController().GetLeaveCount(mid, thisMonthStart, thisMonthEnd));

            //        leaveStatus.Add("审核中");
            //    }
            //    ViewBag.unFinishLID = unFinishLID;
            //}

            //ViewBag.applicantsImg = applicantsImg;
            //ViewBag.applicantsID = applicantsID;
            //ViewBag.applicantsName = applicantsName;
            //ViewBag.applicantsJob = applicantsJob;
            //ViewBag.applicantsReason = applicantsReason;
            //ViewBag.applicantsApplyTime = applicantsApplyTime;
            //ViewBag.applicantsStartTime = applicantsStartTime;
            //ViewBag.applicantsEndTime = applicantsEndTime;
            //ViewBag.applicantsTimes = applicantsTimes;
            //ViewBag.leaveStatus = leaveStatus;
            return View();
        }

        public ActionResult moneyList(string status, string searchWith, string searchWithContent)
        {
            //重新请求数据库，获取员工ID
            List<string> MID = new ManagerController().GetAllMember();

            ////清空原本数组
            //showImgList.Clear();
            //showNameList.Clear();
            //showPhoneList.Clear();

            return View();
        }
    }
}
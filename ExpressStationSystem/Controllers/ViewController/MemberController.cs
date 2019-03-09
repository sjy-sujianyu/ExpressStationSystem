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
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace ExpressStationSystem.Controllers.ViewController
{
   
    public class MemberController : Controller
    {
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
                //已经辞职的不需要
                if (status != "已辞职" && menInfo.isDelete)
                {
                    continue;
                }
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
                //已经辞职的不需要
                if (status != "已辞职" && menInfo.isDelete)
                {
                    continue;
                }
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
                //已经辞职的不需要
                if (status != "已辞职" && menInfo.isDelete)
                {
                    continue;
                }
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
                //已经辞职的不需要
                if (status != "已辞职" && menInfo.isDelete)
                {
                    continue;
                }
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
            double sumD = 0;
            double sumP = 0;
            double sumT = 0;
            double sumS = 0;

            List<dynamic> moneyInfo = new List<dynamic>();
            foreach(var man in showMen)
            {
                var josnStr = JsonConvert.SerializeObject(new MoneyController().GetSalary(man.mId, date));
                JObject jo = (JObject)JsonConvert.DeserializeObject(josnStr);
                moneyInfo.Add(jo);

                sumD += Convert.ToDouble(jo["commission"]["delivery"]["DeliveryCount"]);
                sumP += Convert.ToDouble(jo["commission"]["pickUp"]["PickUpCount"]);
                sumT += Convert.ToDouble(jo["commission"]["transfer"]["TransferCount"]);
                sumS += Convert.ToDouble(jo["total"]);
            }

            ViewBag.sumD = sumD;
            ViewBag.sumP = sumP;
            ViewBag.sumT = sumT;
            ViewBag.sumS = sumS;

            ViewBag.moneyInfo = moneyInfo;
            ViewBag.showMen = showMen;
            return View();

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

                //已经辞职的不需要
                if(status != "已辞职" && menInfo.isDelete)
                {
                    continue;
                }

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

            //默认是未处理
            var LIDList = new LeaveController().GetLeaveList();
            if(status == "历史记录")
            {
                //20年前
                DateTime oldTime = DateTime.Now.AddYears(-20);
                //当今
                DateTime now = DateTime.Now;
                //获取历史的的LID
                LIDList = new LeaveController().GetHistoryLeave(oldTime, now);
            }

            List<dynamic> showLeaveInfoList = new List<dynamic>();

            foreach(var LID in LIDList)
            {
                var LeaveInfo = new LeaveController().GetLeaveInfo(LID);

                if(searchWith != null)
                {
                    if(searchWith == "按姓名")
                    {
                        if(LeaveInfo.member.name.ToString().StartsWith(searchWithContent))
                        {
                            showLeaveInfoList.Add(LeaveInfo);
                        }
                    }
                    else if(searchWith == "按手机")
                    {
                        if (LeaveInfo.member.mId.ToString().StartsWith(searchWithContent))
                        {
                            showLeaveInfoList.Add(LeaveInfo);
                        }
                    }
                    else if(searchWith == "按日期")
                    {
                        if(LeaveInfo.leave.time.ToString().StartsWith(searchWithContent))
                        {
                            showLeaveInfoList.Add(LeaveInfo);
                        }
                    }
                    else
                    {
                        showLeaveInfoList.Add(LeaveInfo);
                    }
                }
                else
                {
                    showLeaveInfoList.Add(LeaveInfo);
                }

            }

            ViewBag.showLeaveInfoList = showLeaveInfoList;
            return View();
        }

        public ActionResult moneyList(string status, string searchWith, string searchWithContent)
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
            //20年前
            DateTime oldTime = DateTime.Now.AddYears(-20);
            //当今
            DateTime now = DateTime.Now;
            //获取历史罚款的的ID
            List<int> moneyID = new MoneyController().GetMoneyHistory(oldTime,now);

            List<dynamic> step = new List<dynamic>();
            //决定显示的前端数组
            List<dynamic> showMoneyList = new List<dynamic>();
            //第一次筛选
            foreach (var mid in moneyID)
            {
                var moneyInfo = new MoneyController().GetMoneyInfo(mid);

                //已经辞职的不需要
                if (status != "已辞职" && moneyInfo.member.isDelete)
                {
                    continue;
                }
                if (status == "补贴")
                {
                    if (moneyInfo.money.subsidy > 0)
                    {
                        step.Add(moneyInfo);
                        continue;
                    }
                }
                else if (status == "罚款")
                {
                    if (moneyInfo.money.fine > 0)
                    {
                        step.Add(moneyInfo);
                        continue;
                    }
                }
                else
                {
                    step.Add(moneyInfo);
                    continue;
                }
            }
            //第二次是搜索的筛选
            foreach (var menInfo in step)
            {
                var josnStr = JsonConvert.SerializeObject(menInfo);
                JObject jo = (JObject)JsonConvert.DeserializeObject(josnStr);

                if (searchWith != null)
                {
                    if (searchWith == "按姓名")
                    {
                        if (menInfo.member.name.ToString().StartsWith(searchWithContent))
                        {
                            showMoneyList.Add(jo);
                        }
                    }
                    else if (searchWith == "按手机")
                    {
                        if (menInfo.member.mId.ToString().StartsWith(searchWithContent))
                        {
                            showMoneyList.Add(jo);
                        }
                    }
                    else if (searchWith == "按月份")
                    {
                        if (menInfo.money.time.ToString().StartsWith(searchWithContent))
                        {
                            showMoneyList.Add(jo);
                        }
                    }
                }
                else
                {
                    showMoneyList.Add(jo);
                }
            }
            ViewBag.showMoneyList = showMoneyList;
            return View();
        }
    }
}
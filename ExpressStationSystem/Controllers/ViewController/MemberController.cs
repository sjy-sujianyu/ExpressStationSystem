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
        // GET: Member
        public ActionResult AllMember(string status, string searchWith, string searchWithContent, string date1, string date2, string page)
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            int pageNum = 10;
            string defaultSearchWith = "按姓名";
            string defaultSearchWithContent = "";
            string defaultStatus = "选择分类";

            if (status == "undefined" || status == null || status =="")
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
            if (date1 == "" || date1 == null)
            {
                ViewBag.thisDate1 = "";
            }
            else
            {
                ViewBag.thisDate1 = date1;
            }
            if (date2 == "" || date2 == null)
            {
                ViewBag.thisDate2 = "";
            }
            else
            {
                ViewBag.thisDate2 = date2;
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
                //不对时间的也不要
                if ((date1 != null && date1 != "") && menInfo.time.CompareTo(Convert.ToDateTime(date1)) <= 0)
                {
                    continue;
                }
                if ((date2 != null && date2 != "") && menInfo.time.CompareTo(Convert.ToDateTime(date2).AddDays(1)) >= 0)
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

            //默认在第一页
            if (page == null || page == "" || Convert.ToInt32(page) <= 0)
            {
                page = "1";
            }
            //临时数据
            int pageTemp = Convert.ToInt32(page);
            List<dynamic> showMen2 = new List<dynamic>();
            //第三次筛选，是把该页的信息的插进临时数组
            for (int i = (pageTemp - 1) * pageNum; i < pageTemp * pageNum; i++)
            {
                if (i >= showMen.Count)
                {
                    break;
                }
                else
                {
                    showMen2.Add(showMen[i]);
                }

            }
            //前端数据
            if (showMen.Count % pageNum == 0)
            {
                ViewBag.PageSum = showMen.Count / pageNum;
            }
            else
            {
                ViewBag.PageSum = showMen.Count / pageNum + 1;
            }
            ViewBag.currentPage = pageTemp;
            ViewBag.showMen = showMen2;
            return View();
        }

        public ActionResult AddMember()
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            return View();
        }

        public ActionResult DeleteMember(string status, string searchWith, string searchWithContent, string page)
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            int pageNum = 10;
            string defaultSearchWith = "按姓名";
            string defaultSearchWithContent = "";
            string defaultStatus = "选择分类";

            if (status == "undefined" || status == null || status == "")
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
            //默认在第一页
            if (page == null || page == "" || Convert.ToInt32(page) <= 0)
            {
                page = "1";
            }
            //临时数据
            int pageTemp = Convert.ToInt32(page);
            List<dynamic> showMen2 = new List<dynamic>();
            //第三次筛选，是把该页的信息的插进临时数组
            for (int i = (pageTemp - 1) * pageNum; i < pageTemp * pageNum; i++)
            {
                if (i >= showMen.Count)
                {
                    break;
                }
                else
                {
                    showMen2.Add(showMen[i]);
                }

            }
            //前端数据
            if (showMen.Count % pageNum == 0)
            {
                ViewBag.PageSum = showMen.Count / pageNum;
            }
            else
            {
                ViewBag.PageSum = showMen.Count / pageNum + 1;
            }
            ViewBag.currentPage = pageTemp;
            ViewBag.showMen = showMen2;
            return View();
        }

        public ActionResult Mission(string status, string searchWith, string searchWithContent,string page)
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            int pageNum = 10;
            string defaultSearchWith = "按姓名";
            string defaultSearchWithContent = "";
            string defaultStatus = "选择分类";
           
            if (status == "undefined" || status == null || status == "")
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
                if ((status != "已辞职" && menInfo.isDelete) || menInfo.job == "经理")
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
            //默认在第一页
            if (page == null || page == "" || Convert.ToInt32(page) <= 0)
            {
                page = "1";
            }
            //临时数据
            int pageTemp = Convert.ToInt32(page);
            List<dynamic> showMen2 = new List<dynamic>();
            //第三次筛选，是把该页的信息的插进临时数组
            for (int i = (pageTemp - 1) * pageNum; i < pageTemp * pageNum; i++)
            {
                if (i >= showMen.Count)
                {
                    break;
                }
                else
                {
                    showMen2.Add(showMen[i]);
                }

            }
            //前端数据
            if (showMen.Count % pageNum == 0)
            {
                ViewBag.PageSum = showMen.Count / pageNum;
            }
            else
            {
                ViewBag.PageSum = showMen.Count / pageNum + 1;
            }
            ViewBag.currentPage = pageTemp;
            ViewBag.showMen = showMen2;
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

        public ActionResult Wages(string status, string searchWith, string searchWithContent,string month, string page)
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            int pageNum = 10;
            string defaultSearchWith = "按姓名";
            string defaultSearchWithContent = "";
            string defaultStatus = "选择分类";

            if (status == "undefined" || status == null || status == "")
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
            DateTime date;
            if (month == "" || month == null)
            {
                date = new DateTime(DateTime.Now.Year, DateTime.Now.Month, 1);
            }
            else
            {
                date = Convert.ToDateTime(month);
            }
            ViewBag.thisMonth = date.Year.ToString() + "-" + date.Month.ToString();
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
                }
                else
                {
                    showMen.Add(menInfo);
                }
            }
            //默认在第一页
            if (page == null || page == "" || Convert.ToInt32(page) <= 0)
            {
                page = "1";
            }
            //临时数据
            int pageTemp = Convert.ToInt32(page);
            List<dynamic> showMen2 = new List<dynamic>();
            //第三次筛选，是把该页的信息的插进临时数组
            for (int i = (pageTemp - 1) * pageNum; i < pageTemp * pageNum; i++)
            {
                if (i >= showMen.Count)
                {
                    break;
                }
                else
                {
                    showMen2.Add(showMen[i]);
                }

            }
            //前端数据
            if (showMen.Count % pageNum == 0)
            {
                ViewBag.PageSum = showMen.Count / pageNum;
            }
            else
            {
                ViewBag.PageSum = showMen.Count / pageNum + 1;
            }
            ViewBag.currentPage = pageTemp;
            //获取工资详情
            double sumD = 0;
            double sumP = 0;
            double sumT = 0;
            double sumS = 0;

            List<dynamic> moneyInfo = new List<dynamic>();
            //统计工资
            foreach(var man in showMen)
            {
                //得到工资
                var josnStr = JsonConvert.SerializeObject(new MoneyController().GetSalary(man.mId, date));
                JObject jo = (JObject)JsonConvert.DeserializeObject(josnStr);

                sumD += Convert.ToDouble(jo["commission"]["delivery"]["DeliveryCount"]);
                sumP += Convert.ToDouble(jo["commission"]["pickUp"]["PickUpCount"]);
                sumT += Convert.ToDouble(jo["commission"]["transfer"]["TransferCount"]);
                sumS += Convert.ToDouble(jo["total"]);
            }
            //显示的数组
            foreach(var man in showMen2)
            {
                var josnStr = JsonConvert.SerializeObject(new MoneyController().GetSalary(man.mId, date));
                JObject jo = (JObject)JsonConvert.DeserializeObject(josnStr);
                moneyInfo.Add(jo);
            }

            ViewBag.sumD = sumD;
            ViewBag.sumP = sumP;
            ViewBag.sumT = sumT;
            ViewBag.sumS = sumS;

            ViewBag.moneyInfo = moneyInfo;
            ViewBag.showMen = showMen2;
            return View();

        }

        public ActionResult Fine(string status, string searchWith, string searchWithContent, string page)
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }

            var managername = (new QueryController().GetMemberAllInfo(Request.Cookies["MyCook"]["userid"])).name;
            ViewBag.managername = managername;

            int pageNum = 10;
            string defaultSearchWith = "按姓名";
            string defaultSearchWithContent = "";
            string defaultStatus = "选择分类";

            if (status == "undefined" || status == null || status == "")
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
            //默认在第一页
            if (page == null || page == "" || Convert.ToInt32(page) <= 0)
            {
                page = "1";
            }
            //临时数据
            int pageTemp = Convert.ToInt32(page);
            List<dynamic> showMen2 = new List<dynamic>();
            //第三次筛选，是把该页的信息的插进临时数组
            for (int i = (pageTemp - 1) * pageNum; i < pageTemp * pageNum; i++)
            {
                if (i >= showMen.Count)
                {
                    break;
                }
                else
                {
                    showMen2.Add(showMen[i]);
                }

            }
            //前端数据
            if (showMen.Count % pageNum == 0)
            {
                ViewBag.PageSum = showMen.Count / pageNum;
            }
            else
            {
                ViewBag.PageSum = showMen.Count / pageNum + 1;
            }
            ViewBag.currentPage = pageTemp;
            ViewBag.showMen = showMen2;
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
        }

        public ActionResult DetailMember(string id)
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            if (id != null)
            {
                var record = new QueryController().GetTotalRecordByAccount(id);
                ViewBag.DeliveryCount = record.DeliveryCount;
                ViewBag.PickUpCount = record.PickUpCount;
                ViewBag.TransferCount = record.TransferCount;

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
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            if (id == null)
            {
                id = "15813322560";
            }

            var record = new QueryController().GetTotalRecordByAccount(id);
            ViewBag.DeliveryCount = record.DeliveryCount;
            ViewBag.PickUpCount = record.PickUpCount;
            ViewBag.TransferCount = record.TransferCount;

            var member = new QueryController().GetMemberAllInfo(id);
            ViewBag.thisID = id;
            ViewBag.thisName = new QueryController().GetMemberAllInfo(id).name;
            ViewBag.thisImagePath = new QueryController().GetMemberAllInfo(id).imagePath;
            ViewBag.thisBaseSalary = new QueryController().GetMemberAllInfo(id).baseSalary;
            ViewBag.thisJob = new QueryController().GetMemberAllInfo(id).job;

            return View();
        }

        public ActionResult Leave(string status, string searchWith, string searchWithContent, string date1, string date2, string page)
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }
            int pageNum = 7;
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
            //处理日期
            if (date1 == "" || date1 == null)
            {
                ViewBag.thisDate1 = "";
            }
            else
            {
                ViewBag.thisDate1 = date1;
            }
            if (date2 == "" || date2 == null)
            {
                ViewBag.thisDate2 = "";
            }
            else
            {
                ViewBag.thisDate2 = date2;
            }
            //默认是未处理列表
            var LIDList = new LeaveController().GetLeaveList();
            //如果是历史记录的就改变数组
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
                if ((date1 != null && date1 != "") && Convert.ToDateTime(date1).CompareTo(Convert.ToDateTime(LeaveInfo.leave.time)) >= 0)
                {
                    continue;
                }
                if ((date2 != null && date2 != "") && Convert.ToDateTime(date2).AddDays(1).CompareTo(Convert.ToDateTime(LeaveInfo.leave.time)) <= 0)
                {
                    continue;
                }
                if (searchWith != null)
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
            List<dynamic> showLeaveInfoList2 = new List<dynamic>();
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
                if (i >= showLeaveInfoList.Count)
                {
                    break;
                }
                else
                {
                    showLeaveInfoList2.Add(showLeaveInfoList[i]);
                }

            }
            ViewBag.showLeaveInfoList = showLeaveInfoList2;
            if (showLeaveInfoList.Count % pageNum == 0)
            {
                ViewBag.PageSum = showLeaveInfoList.Count / pageNum;
            }
            else
            {
                ViewBag.PageSum = showLeaveInfoList.Count / pageNum + 1;
            }
            ViewBag.currentPage = pageTemp;
            return View();
        }

        public ActionResult moneyList(string status, string searchWith, string searchWithContent, string date1, string date2, string page)
        {
            if (!checkCookies())
            {
                return Content(string.Format("<script>alert('请先登陆');parent.window.location='/Login/Login';</script>"));
            }

            var managername = (new QueryController().GetMemberAllInfo(Request.Cookies["MyCook"]["userid"])).name;
            ViewBag.managername = managername;
            int pageNum = 10;
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
            if(date1 == "" || date1 == null)
            {
                ViewBag.thisDate1 = "";
            }
            else
            {
                ViewBag.thisDate1 = date1;
            }
            if (date2 == "" || date2 == null)
            {
                ViewBag.thisDate2 = "";
            }
            else
            {
                ViewBag.thisDate2 = date2;
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
                if ((date1 != null && date1 != "")&& moneyInfo.money.time.CompareTo(Convert.ToDateTime(date1)) <= 0)
                {
                    continue;
                }
                if((date2 != null && date2 != "") && moneyInfo.money.time.CompareTo(Convert.ToDateTime(date2).AddDays(1)) >= 0)
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
                }
                else
                {
                    showMoneyList.Add(jo);
                }
            }
            //默认在第一页
            if (page == null || page == "" || Convert.ToInt32(page) <= 0)
            {
                page = "1";
            }
            //临时数据
            int pageTemp = Convert.ToInt32(page);
            List<dynamic> showMen2 = new List<dynamic>();
            //第三次筛选，是把该页的信息的插进临时数组
            for (int i = (pageTemp - 1) * pageNum; i < pageTemp * pageNum; i++)
            {
                if (i >= showMoneyList.Count)
                {
                    break;
                }
                else
                {
                    showMen2.Add(showMoneyList[i]);
                }

            }
            //前端数据
            if (showMoneyList.Count % pageNum == 0)
            {
                ViewBag.PageSum = showMoneyList.Count / pageNum;
            }
            else
            {
                ViewBag.PageSum = showMoneyList.Count / pageNum + 1;
            }
            ViewBag.currentPage = pageTemp;
            ViewBag.showMoneyList = showMen2;
            return View();
        }
    }
}
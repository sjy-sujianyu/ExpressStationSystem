using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ExpressStationSystem.Controllers;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class MemberController : Controller
    {
        List<string> showImgList = new List<string>();
        List<string> showNameList = new List<string>();
        List<string> showPhoneList = new List<string>();
        List<string> showJobList = new List<string>();
        List<string> showIsOnDutyList = new List<string>();
        // GET: Member
        public ActionResult AllMember(string status)
        {
            //重新请求数据库，获取员工ID
            List<string> MID = new ManagerController().GetAllMember();

            //清空原本数组
            showImgList.Clear();
            showNameList.Clear();
            showPhoneList.Clear();
            showJobList.Clear();
            showIsOnDutyList.Clear();

            if (status == "已辞职")
            {
                foreach (var one in MID)
                {
                    if ((new QueryController().GetMemberAllInfo(one).isDelete))
                    {
                        showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
                        showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
                        showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
                        showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
                        if(new QueryController().GetMemberAllInfo(one).onDuty)
                        {
                            showIsOnDutyList.Add("工作中");
                        }
                        else
                        {
                            showIsOnDutyList.Add("休息中");
                        }
                       
                    }
                }
            }else if(status == null)
            {
                foreach (var one in MID)
                {
                    if (!(new QueryController().GetMemberAllInfo(one).isDelete))
                    {
                        showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
                        showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
                        showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
                        showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
                        if (new QueryController().GetMemberAllInfo(one).onDuty)
                        {
                            showIsOnDutyList.Add("工作中");
                        }
                        else
                        {
                            showIsOnDutyList.Add("休息中");
                        }
                    }
                }
            }else if(status == "休息中")
            {
                foreach (var one in MID)
                {
                    if (!(new QueryController().GetMemberAllInfo(one).onDuty) && !(new QueryController().GetMemberAllInfo(one).isDelete))
                    {
                        showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
                        showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
                        showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
                        showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
                        showIsOnDutyList.Add("休息中");
                    }
                }
            }else
            {
                foreach (var one in MID)
                {
                    if ((new QueryController().GetMemberAllInfo(one).job) == status && !(new QueryController().GetMemberAllInfo(one).isDelete))
                    {
                        showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
                        showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
                        showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
                        showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
                        if (new QueryController().GetMemberAllInfo(one).onDuty)
                        {
                            showIsOnDutyList.Add("工作中");
                        }
                        else
                        {
                            showIsOnDutyList.Add("休息中");
                        }
                    }
                }
            }

            ViewBag.showImgList = showImgList;
            ViewBag.showNameList = showNameList;
            ViewBag.showPhoneList = showPhoneList;
            ViewBag.showJobList = showJobList;
            ViewBag.showIsOnDutyList = showIsOnDutyList;

            return View();
        }

        public ActionResult AddMember()
        {

            return View();
        }

        public ActionResult DeleteMember()
        {
            List<string> MID = new ManagerController().GetAllMember();
            List<string> imgList = new List<string>();
            List<string> nameList = new List<string>();
            List<string> phoneList = new List<string>();
            List<string> jobList = new List<string>();

            foreach (var one in MID)
            {
                if(! new QueryController().GetMemberAllInfo(one).isDelete)
                {
                    var member = new QueryController().GetMemberAllInfo(one);
                    imgList.Add(member.imagePath);

                    nameList.Add(member.name);

                    phoneList.Add(member.mId);

                    jobList.Add(member.job);
                }
                
            }

            ViewBag.MID = MID;
            ViewBag.imgList = imgList;
            ViewBag.phoneList = phoneList;
            ViewBag.nameList = nameList;
            ViewBag.jobList = jobList;
            return View();
        }

        public ActionResult Mission()
        {
            return View();
        }

        public ActionResult Wages()
        {
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

        public ActionResult Leave(string type)
        {
            List<string> applicantsImg = new List<string>();

            List<string> applicantsID = new List<string>();
            List<string> applicantsName = new List<string>();
            List<string> applicantsJob = new List<string>();
            List<string> applicantsReason = new List<string>();
            List<DateTime> applicantsApplyTime = new List<DateTime>();
            List<DateTime> applicantsStartTime = new List<DateTime>();
            List<DateTime> applicantsEndTime = new List<DateTime>();


            List<int> applicantsTimes = new List<int>();

            //获取月份
            DateTime thisMonthStart = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date;
            DateTime thisMonthEnd = DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1);

            //返回历史
            if (type == "history")
            {

            }
            //返回未处理
            else
            {
                //得到未处理的请假条ID
                var unFinishLID = new LeaveController().GetLeaveList();

                foreach(var lid in unFinishLID)
                {
                    var mid = new LeaveController().GetLeaveInfo(lid).member.mId;
                    applicantsImg.Add(new LeaveController().GetLeaveInfo(lid).member.imagePath);
                    applicantsID.Add(mid);
                    applicantsName.Add(new LeaveController().GetLeaveInfo(lid).member.name);
                    applicantsJob.Add(new LeaveController().GetLeaveInfo(lid).member.job);
                    applicantsReason.Add(new LeaveController().GetLeaveInfo(lid).leave.reason);
                    applicantsApplyTime.Add(new LeaveController().GetLeaveInfo(lid).leave.time);
                    applicantsStartTime.Add(new LeaveController().GetLeaveInfo(lid).leave.srcTime);
                    applicantsEndTime.Add(new LeaveController().GetLeaveInfo(lid).leave.endTime);

                    applicantsTimes.Add(new LeaveController().GetLeaveCount(mid, thisMonthStart, thisMonthEnd));
                }
                ViewBag.unFinishLID = unFinishLID;
            }
            
            ViewBag.applicantsImg = applicantsImg;
            ViewBag.applicantsID = applicantsID;
            ViewBag.applicantsName = applicantsName;
            ViewBag.applicantsJob = applicantsJob;
            ViewBag.applicantsReason = applicantsReason;
            ViewBag.applicantsApplyTime = applicantsApplyTime;
            ViewBag.applicantsStartTime = applicantsStartTime;
            ViewBag.applicantsEndTime = applicantsEndTime;
            ViewBag.applicantsTimes = applicantsTimes;
            return View();
        }
    }
}
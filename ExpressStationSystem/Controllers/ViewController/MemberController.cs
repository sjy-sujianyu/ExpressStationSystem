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
        public ActionResult AllMember(string status, string searchWithName, string searchWithPhone)
        {
            //重新请求数据库，获取员工ID
            List<string> MID = new ManagerController().GetAllMember();

            //清空原本数组
            showImgList.Clear();
            showNameList.Clear();
            showPhoneList.Clear();
            showJobList.Clear();
            showIsOnDutyList.Clear();

            if (searchWithName != null)
            {
                //按名字查找
                foreach (var one in MID)
                {
                    var info = new QueryController().GetMemberAllInfo(one);
                    if (info.name == searchWithName || info.name.Contains(searchWithName))
                    {
                        showImgList.Add(info.imagePath);
                        showNameList.Add(info.name);
                        showPhoneList.Add(info.mId);
                        showJobList.Add(info.job);
                        if (info.onDuty)
                        {
                            showIsOnDutyList.Add("工作中");
                        }
                        else
                        {
                            showIsOnDutyList.Add("休息中");
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
            else if(searchWithPhone != null)
            {
                //按手机号查询
                foreach (var one in MID)
                {
                    var info = new QueryController().GetMemberAllInfo(one);
                    if (info.mId == searchWithPhone || info.mId.Contains(searchWithPhone))
                    {
                        showImgList.Add(info.imagePath);
                        showNameList.Add(info.name);
                        showPhoneList.Add(info.mId);
                        showJobList.Add(info.job);
                        if (info.onDuty)
                        {
                            showIsOnDutyList.Add("工作中");
                        }
                        else
                        {
                            showIsOnDutyList.Add("休息中");
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
            
            //默认情况
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
            else if (status == null)
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
            }
            else if (status == "休息中")
            {
                foreach (var one in MID)
                {
                    if (!(new QueryController().GetMemberAllInfo(one).onDuty) && !(new QueryController().GetMemberAllInfo(one).isDelete))
                    {
                        showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
                        showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
                        showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
                        showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
                        showIsOnDutyList.Add(status);
                    }
                }
            }
            else if (status == "工作中")
            {
                foreach (var one in MID)
                {
                    if ((new QueryController().GetMemberAllInfo(one).onDuty) && !(new QueryController().GetMemberAllInfo(one).isDelete))
                    {
                        showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
                        showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
                        showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
                        showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
                        showIsOnDutyList.Add(status);
                    }
                }
            }
            else
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

        public ActionResult DeleteMember(string status, string searchWithName, string searchWithPhone)
        {
            
            List<string> MID = new ManagerController().GetAllMember();

            //清空原本数组
            showImgList.Clear();
            showNameList.Clear();
            showPhoneList.Clear();
            showJobList.Clear();

            if (searchWithName != null)
            {
                //按名字查找
                foreach (var one in MID)
                {
                    var info = new QueryController().GetMemberAllInfo(one);
                    if (info.job == "经理")
                    {
                        continue;
                    }
                    if (info.name == searchWithName || info.name.Contains(searchWithName))
                    {
                        showImgList.Add(info.imagePath);
                        showNameList.Add(info.name);
                        showPhoneList.Add(info.mId);
                        showJobList.Add(info.job);
                        
                    }
                }
                ViewBag.imgList = showImgList;
                ViewBag.phoneList = showPhoneList;
                ViewBag.nameList = showNameList;
                ViewBag.jobList = showJobList;
                return View();
            }
            else if (searchWithPhone != null)
            {
                //按手机号查询
                foreach (var one in MID)
                {
                    var info = new QueryController().GetMemberAllInfo(one);
                    if (info.job == "经理")
                    {
                        continue;
                    }
                    if (info.mId == searchWithPhone || info.mId.Contains(searchWithPhone))
                    {
                        showImgList.Add(info.imagePath);
                        showNameList.Add(info.name);
                        showPhoneList.Add(info.mId);
                        showJobList.Add(info.job);
                        
                    }
                }
                ViewBag.imgList = showImgList;
                ViewBag.phoneList = showPhoneList;
                ViewBag.nameList = showNameList;
                ViewBag.jobList = showJobList;
                return View();
            }

            foreach (var one in MID)
            {
                var member = new QueryController().GetMemberAllInfo(one);
                if(member.job == "经理")
                {
                    continue;
                }
                if (status != null)
                {
                    if (! member.isDelete && member.job == status)
                    {
                        showImgList.Add(member.imagePath);

                        showNameList.Add(member.name);

                        showPhoneList.Add(member.mId);

                        showJobList.Add(member.job);
                    }
                }
                else if(! member.isDelete)
                {
                    showImgList.Add(member.imagePath);

                    showNameList.Add(member.name);

                    showPhoneList.Add(member.mId);

                    showJobList.Add(member.job);
                }
                
            }
            
            ViewBag.imgList = showImgList;
            ViewBag.phoneList = showPhoneList;
            ViewBag.nameList = showNameList;
            ViewBag.jobList = showJobList;
            return View();
        }

        public ActionResult Mission()
        {
            //重新请求数据库，获取员工ID
            List<string> MID = new ManagerController().GetAllMember();

            //清空原本数组
            showImgList.Clear();
            showNameList.Clear();
            showPhoneList.Clear();
            showJobList.Clear();
            showIsOnDutyList.Clear();

            foreach (var one in MID)
            {
                var info = new QueryController().GetMemberAllInfo(one);
                if (info.job != "经理")
                    if (!info.isDelete)
                    {
                        showImgList.Add(info.imagePath);
                        showNameList.Add(info.name);
                        showPhoneList.Add(info.mId);
                        showJobList.Add(info.job);
                        if (info.onDuty)
                        {
                            showIsOnDutyList.Add("工作中");
                        }
                        else
                        {
                            showIsOnDutyList.Add("休息中");
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

        public ActionResult Wages()
        {
            //重新请求数据库，获取员工ID
            List<string> MID = new ManagerController().GetAllMember();

            int sumPickUp = 0;
            int sumTransfer = 0;
            int sumDelivery = 0;
            decimal sumSalary = 0;

            //清空原本数组
            showImgList.Clear();
            showNameList.Clear();
            showPhoneList.Clear();

            List<int> PickUpCount = new List<int>();
            List<int> DeliveryCount = new List<int>();
            List<int> TransferCount = new List<int>();
            List<decimal> baseSalary = new List<decimal>();
            List<decimal> subsidy = new List<decimal>();
            List<decimal> fine = new List<decimal>();
            
            List<dynamic> totalSalary = new List<dynamic>();

            foreach(var one in MID)
            {
                var info = new QueryController().GetMemberAllInfo(one);

                if (info.job != "经理" && !info.isDelete)
                {
                    showImgList.Add(info.imagePath);
                    showNameList.Add(info.name);
                    showPhoneList.Add(info.mId);

                    var money = new MoneyController().GetSalary(one, DateTime.Now.AddDays(1 - DateTime.Now.Day).Date.AddMonths(1).AddSeconds(-1));
                    baseSalary.Add(money.baseSalary);
                    PickUpCount.Add(money.commission.pickUp.PickUpCount);
                    DeliveryCount.Add(money.commission.delivery.DeliveryCount);
                    TransferCount.Add(money.commission.transfer.TransferCount);
                    subsidy.Add(money.subsidy.total);
                    fine.Add(money.fine.total);

                    sumPickUp += money.commission.pickUp.PickUpCount;
                    sumTransfer += money.commission.transfer.TransferCount;
                    sumDelivery += money.commission.delivery.DeliveryCount;


                    totalSalary.Add(money.baseSalary + money.commission.pickUp.total + money.commission.delivery.total + money.commission.transfer.total + money.subsidy.total - money.fine.total);

                    sumSalary += (money.baseSalary + money.commission.pickUp.total + money.commission.delivery.total + money.commission.transfer.total + money.subsidy.total - money.fine.total);
                }
                
            }

            ViewBag.showImgList = showImgList;
            ViewBag.showNameList = showNameList;
            ViewBag.showPhoneList = showPhoneList;
            ViewBag.baseSalary = baseSalary;
            ViewBag.PickUpCount = PickUpCount;
            ViewBag.DeliveryCount = DeliveryCount;
            ViewBag.TransferCount = TransferCount;
            ViewBag.subsidy = subsidy;
            ViewBag.fine = fine;
            ViewBag.totalSalary = totalSalary;

            ViewBag.sumPickUp = sumPickUp;
            ViewBag.sumTransfer = sumTransfer;
            ViewBag.sumDelivery = sumDelivery;
            ViewBag.sumSalary = sumSalary;

            return View();
        }

        public ActionResult Fine()
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
                //20年前
                DateTime oldTime = DateTime.Now.AddYears(-20);
                //当今时间
                DateTime now = DateTime.Now;

                for(int status = 1; status <= 3; status++)
                {
                    var getList = new LeaveController();
                }
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
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

            if(status == "已辞职")
            {
                foreach (var one in MID)
                {
                    if ((new QueryController().GetMemberAllInfo(one).isDelete))
                    {
                        showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
                        showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
                        showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
                        showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
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
                    }
                }
            }

            ViewBag.showImgList = showImgList;
            ViewBag.showNameList = showNameList;
            ViewBag.showPhoneList = showPhoneList;
            ViewBag.showJobList = showJobList;

            return View();
        }

        //public ActionResult AllMember()
        //{
        //    //重新请求数据库，获取员工ID
        //    List<string> MID = new ManagerController().GetAllMember();

        //    //清空原本数组
        //    showImgList.Clear();
        //    showNameList.Clear();
        //    showPhoneList.Clear();
        //    showJobList.Clear();

        //    //默认获取全部的人员
        //    foreach (var one in MID)
        //    {
        //        if (!(new QueryController().GetMemberAllInfo(one).isDelete))
        //        {
        //            showImgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
        //            showNameList.Add(new QueryController().GetMemberAllInfo(one).name);
        //            showPhoneList.Add(new QueryController().GetMemberAllInfo(one).mId);
        //            showJobList.Add(new QueryController().GetMemberAllInfo(one).job);
        //        }
        //    }

        //    ViewBag.showImgList = showImgList;
        //    ViewBag.showNameList = showNameList;
        //    ViewBag.showPhoneList = showPhoneList;
        //    ViewBag.showJobList = showJobList;

        //    return View();
        //}

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
                var member = new QueryController().GetMemberAllInfo(one);
                imgList.Add(member.imagePath);

                nameList.Add(member.name);

                phoneList.Add(member.mId);

                jobList.Add(member.job);
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

        public ActionResult DetailMember()
        {
            return View();
        }

        public ActionResult changeMemberDetail()
        {
            return View();
        }

        public ActionResult Leave()
        {
            return View();
        }
    }
}
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
        // GET: Member
        public ActionResult AllMember()
        {
            List<string> MID = new ManagerController().GetAllMember().onDuty;
            List<string> imgList = new List<string>();
            List<string> nameList = new List<string>();
            List<string> phoneList = new List<string>();
            List<string> jobList = new List<string>();
            List<int> showArray = new List<int>();

            int num = 0;
            foreach (var one in MID)
            {
                showArray.Add(num++);
                imgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);
                nameList.Add(new QueryController().GetMemberAllInfo(one).name);
                phoneList.Add(new QueryController().GetMemberAllInfo(one).account);
                jobList.Add(new QueryController().GetMemberAllInfo(one).job);
            }
            ViewBag.Sum = num;
            ViewBag.showArray = showArray;
            ViewBag.MID = MID;
            ViewBag.imgList = imgList;
            ViewBag.phoneList = phoneList;
            ViewBag.nameList = nameList;
            ViewBag.jobList = jobList;
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
                var member = new QueryController().GetMemberAllInfo(one);
                imgList.Add(member.imagePath);

                nameList.Add(member.name);

                phoneList.Add(member.account);

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
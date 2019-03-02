using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class MemberController : Controller
    {
        // GET: Member
        public ActionResult AllMember()
        {
           //// List<string> MID = new ManagerController().GetAllMemberOnDuty();
           // List<string> imgList = new List<string>();
           // List<string> nameList = new List<string>();
           // List<string> phoneList = new List<string>();
           // List<string> jobList = new List<string>();
            
           // foreach(var one in MID)
           // {
           //     imgList.Add(new QueryController().GetMemberAllInfo(one).imagePath);

           //     nameList.Add(new QueryController().GetMemberAllInfo(one).name);

           //     phoneList.Add(new QueryController().GetMemberAllInfo(one).account);

           //     jobList.Add(new QueryController().GetMemberAllInfo(one).job);
           // }

           // ViewBag.MID = MID;
           // ViewBag.imgList = imgList;
           // ViewBag.phoneList = phoneList;
           // ViewBag.nameList = nameList;
           // ViewBag.jobList = jobList;
            return View();
        }

        public ActionResult AddMember()
        {
            return View();
        }

        public ActionResult DeleteMember()
        {
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
        
    }
}
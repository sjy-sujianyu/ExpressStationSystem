using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ExpressStationSystem.Controllers;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers.ViewController
{
    public class LogInController : Controller
    {
        [HttpPost]
        public ActionResult AfterLogin(FormCollection form)
        {
            if (form != null && Request.Cookies["MyCook"] == null)
            {
                string phone = Request.Form["UserPhone"];
                string passWord = Request.Form["Password"];

                if (new LoginController().LandOfManager(phone, passWord))
                {
                    HttpCookie cookie = new HttpCookie("MyCook");//初使化并设置Cookie的名称
                    DateTime dt = DateTime.Now;
                    TimeSpan ts = new TimeSpan(1, 0, 0, 0, 0);//过期时间为1分钟
                    cookie.Expires = dt.Add(ts);//设置过期时间
                    cookie.Values.Add("userid", phone);
                    cookie.Values.Add("password", passWord);
                    Response.AppendCookie(cookie);
                    return Content(string.Format("<script>alert('Success');parent.window.location='/Login/AfterLogin';</script>"));
                }
                else
                {
                    return Content(string.Format("<script>alert('登陆失败');parent.window.location='/Login/Login';</script>"));
                }
            }
            else
            {
                return Content(string.Format("<script>alert('请重新登陆');parent.window.location='/Login/Login';</script>"));
            }
        }

        public ActionResult AfterLogin()
        {
            if (checkCookies())
            {
                string text = Global.publishMessage;
                if (text  != null && text.Contains('|'))
                {
                    string[] text2 = text.Split('|');
                    ViewBag.title = text2[0];
                    ViewBag.content = text2[1];
                }
                else
                {
                    ViewBag.title = "未知公告";
                    ViewBag.content = text;
                }
                return View();
            }
            else
            {
                return Content(string.Format("<script>alert('请重新登陆');parent.window.location='/Login/Login';</script>"));
            }
        }

        public ActionResult Login()
        {
            if (checkCookies())
            {
                return Content(string.Format("<script>parent.window.location='/Login/AfterLogin';alert('欢迎');</script>"));
            }
            else
            {
                return View();
            }
        }

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

        public ActionResult changePassword()
        {
            return View();
        }

        public ActionResult forget()
        {
            return View();
        }

        public ActionResult exitLogin()
        {
            if(Request.Cookies["MyCook"] != null)
            {
                HttpCookie cok = Request.Cookies["MyCook"];
                if (cok != null)
                {
                    TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
                    cok.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
                    Response.AppendCookie(cok);
                }
            }
            return Content(string.Format("<script>alert('已经退出登陆');parent.window.location='/Home/Index';</script>"));
        }
        //修改Cookie
        //protected void Button3_Click(object sender, EventArgs e)
        //{
        //    //获取客户端的Cookie对象
        //    HttpCookie cok = Request.Cookies["MyCook"];

        //    if (cok != null)
        //    {
        //        //修改Cookie的两种方法
        //        cok.Values["userid"] = "alter-value";
        //        cok.Values.Set("userid", "alter-value");

        //        //往Cookie里加入新的内容
        //        cok.Values.Set("newid", "newValue");
        //        Response.AppendCookie(cok);
        //    }
        //}

        //删除Cookie
        //protected void Button4_Click(object sender, EventArgs e)
        //{

        //    HttpCookie cok = Request.Cookies["MyCook"];
        //    if (cok != null)
        //    {
        //        if (!CheckBox1.Checked)
        //        {
        //            cok.Values.Remove("userid");//移除键值为userid的值
        //        }
        //        else
        //        {
        //            TimeSpan ts = new TimeSpan(-1, 0, 0, 0);
        //            cok.Expires = DateTime.Now.Add(ts);//删除整个Cookie，只要把过期时间设置为现在
        //        }
        //        Response.AppendCookie(cok);
        //    }
        //}
    }
}
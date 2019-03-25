﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class PickUpController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;

        // GET: api/PickUp/GetReadytoReceiveByCondition
        /// <summary>
        /// 按条件查询要揽件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <remarks>按条件查询要揽件的包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("PickUp/GetReadytoReceiveByCondition")]
        public dynamic GetReadytoReceiveByCondition(string str,string type,int page,int pageSize)
        {
            if(str is null||type is null)
            {
                return null;
            }
            db = new DataClasses1DataContext(connstr);
            var a = GetReadytoReceive(0,0);
            List <dynamic> list = new List<dynamic>();
            foreach (var x in a)
            {
                if(type=="单号"&&x.package.id.ToString().StartsWith(str))
                {
                    list.Add(x);
                }
                else if(type=="姓名"&& x.src.name.StartsWith(str))
                {
                    list.Add(x);
                }
                else if(type=="电话"&& x.src.phone.StartsWith(str))
                {
                    list.Add(x);
                }
                else if(type=="街道"&&x.src.street.StartsWith(str))
                {
                    list.Add(x);
                }
            }
            return new ToolsController().splitpage(list, page, pageSize);
        }

        // GET: api/PickUp/GetReceivingByCondition
        /// <summary>
        /// 按条件查询要揽件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <param name="account">员工的mId</param>
        /// <remarks>按条件查询要揽件的包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("PickUp/GetReceivingByCondition")]
        public dynamic GetReceivingByCondition(string account,string str, string type,int page,int pageSize)
        {
            if (str is null || type is null)
            {
                return null;
            }
            db = new DataClasses1DataContext(connstr);
            var a = GetReceiving(account,0,0);
            List<dynamic> list = new List<dynamic>();
            foreach (var x in a)
            {
                if (type == "单号" && x.package.id.ToString().StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "姓名" && x.src.name.StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "电话" && x.src.phone.StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "街道" && x.src.street.StartsWith(str))
                {
                    list.Add(x);
                }
            }
            return new ToolsController().splitpage(list, page, pageSize);
        }
        // GET: api/PickUp/GetReadytoScan
        /// <summary>
        /// 获取上个网点转来待扫件的包裹ID
        /// </summary>
        /// <remarks>获取上个网点转来待扫件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("PickUp/GetReadytoScan")]
        public dynamic GetReadytoScan(int page,int pageSize)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package join b in db.Path on a.id equals b.id  orderby b.time descending where a.status == "运输中" group b by b.id into g select g.First();
            List <dynamic> list = new List<dynamic>();
            foreach (var x in selectQuery)
            {
                if(splitPlace(x.curPlace).street.Contains("华南农业大学")&&x.isArrival==true)
                {
                    list.Add(x);
                }
            }
            sw.Stop();
            TimeSpan ts2 = sw.Elapsed;
            Console.WriteLine("Stopwatch总共花费{0}ms.", ts2.TotalMilliseconds);
            return new ToolsController().splitpage(list, page, pageSize);
        }

        // GET: api/PickUp/GetReadytoReceive
        /// <summary>
        /// 获取已下单,要揽件的包裹ID
        /// </summary>
        /// <remarks>获取待揽件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("PickUp/GetReadytoReceive")]
        public dynamic GetReadytoReceive(int page,int pageSize)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package join b in db.AddressBook  on a.sendId equals b.aId join c in db.AddressBook on a.receiverId equals c.aId  where a.status == "已下单"&&b.street.Contains("华南农业大学") orderby a.time descending select new { package = a, src = b, dest = c };
            List<dynamic> list = new List<dynamic>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return new ToolsController().splitpage(list,page,pageSize);
        }

        // GET: api/PickUp/GetReceiving
        /// <summary>
        /// 获取某员工正在揽件的包裹ID
        /// </summary>
        /// <remarks>获取某员工正在揽件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("PickUp/GetReceiving")]
        public dynamic GetReceiving(string account,int page, int pageSize)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.PickUp.GroupBy(p=>p.id).Select(g=>g.OrderByDescending(t=>t.time).First()) join b in db.Package on a.id equals b.id where b.status == "待揽件" && a.mId == account&&a.isDone==false join c in db.AddressBook on b.sendId equals c.aId join d in db.AddressBook on b.receiverId equals d.aId select new { package = b, src = c, dest = d };
            List<dynamic> list = new List<dynamic>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }

            return new ToolsController().splitpage(list, page, pageSize);
        }
        // Post: api/PickUp/Post
        /// <summary>
        /// 添加待揽件包裹信息
        /// </summary>
        /// <remarks>添加待揽件包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("PickUp/Post")]
        public bool Post(PickUpClass x)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var package = db.Package.Single(a => a.id == x.id);
                if(package.status!="已下单")
                {
                    return false;
                }
                PickUp pk = new PickUp();
                pk.id = x.id;
                pk.mId = x.mId;
                pk.time = DateTime.Now;
                pk.isDone = false;
                db.PickUp.InsertOnSubmit(pk);
                package.status = "待揽件";
                package.time = DateTime.Now;
                db.SubmitChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
        // Post: api/PickUp/ConfirmPost
        /// <summary>
        /// 添加确认揽件包裹信息
        /// </summary>
        /// <remarks>添加确认揽件包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("PickUp/ConfirmPost")]
        public bool ConfirmPost(PickUpClass x)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var package = db.Package.Single(a => a.id == x.id);
                if (package.status != "待揽件")
                {
                    return false;
                }
                PickUp pk = new PickUp();
                pk.id = x.id;
                pk.mId = x.mId;
                pk.time = DateTime.Now;
                pk.isDone = true;
                db.PickUp.InsertOnSubmit(pk);
                package.status = "已扫件";
                package.time = DateTime.Now;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        // PUT: api/PickUp/RevokeReceive
        /// <summary>
        /// 包裹的状态从待揽件撤销为已下单
        /// </summary>
        /// <remarks>包裹的状态从待揽件撤销为已下单</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("PickUp/RevokeReceive")]
        public bool RevokeReceive(IdClass iclass)
        {
            db = new DataClasses1DataContext(connstr);
            var x = db.Package.SingleOrDefault(a => a.id == iclass.id);
            if (x is null)
            {
                return false;
            }
            else
            {
                x.status = "已下单";
                db.SubmitChanges();
                return true;
                
            }
        }


        // PUT: api/PickUp/Scan?id={id}
        /// <summary>
        /// 包裹的状态从运输中状态变为已扫件
        /// </summary>
        /// <remarks>包裹的状态从运输中状态变为已扫件</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("PickUp/Scan")]
        public bool Scan(IdClass iclass)
        {
            db = new DataClasses1DataContext(connstr);
            var x = db.Package.SingleOrDefault(a => a.id == iclass.id);
            if (x is null)
            {
                return false;
            }
            else
            {
                if (x.status!="运输中")
                {
                    return false;
                }
                x.status = "已扫件";
                x.time = DateTime.Now;
                var receiver = db.AddressBook.SingleOrDefault(a => a.aId == x.receiverId);
                if(receiver!=null&&!receiver.street.Contains("华南农业大学"))
                {
                    Error error = new Error();
                    error.id = x.id;
                    error.introduction = "目的地不在此网点附近";
                    error.status = "错件";
                    error.time = DateTime.Now;
                    x.time= DateTime.Now; 
                    try
                    {
                        db.Error.InsertOnSubmit(error);
                    }
                    catch
                    {

                    }
                }
                db.SubmitChanges();
                return true; 
            }
        }
        private dynamic splitPlace(string place)
        {
            string[] str = place.Split('-');
            return new { province = str[0], city = str[1], street = str[2] };
        }
    }
}

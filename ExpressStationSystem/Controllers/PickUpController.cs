﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class PickUpController : ApiController
    {
        private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
        public List<int> GetReadytoReceiveByCondition(string str,string type)
        {
            db = new DataClasses1DataContext(connstr);
            var a = GetReadytoReceive();
            List <int> list = new List<int>();
            foreach (var x in a)
            {
                var ob = new QueryController().GetAllInfo(x);
                if(type=="单号"&&ob.package.id.ToString().StartsWith(str))
                {
                    list.Add(x);
                }
                else if(type=="姓名"&&ob.src.name.StartsWith(str))
                {
                    list.Add(x);
                }
                else if(type=="电话"&&ob.src.phone.StartsWith(str))
                {
                    list.Add(x);
                }
                else if(type=="街道"&&ob.src.street.StartsWith(str))
                {
                    list.Add(x);
                }
            }
            return list;
        }
        // GET: api/PickUp/GetReadytoScan
        /// <summary>
        /// 获取上个网点转来待扫件的包裹ID
        /// </summary>
        /// <remarks>获取上个网点转来待扫件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("PickUp/GetReadytoScan")]
        public List<int> GetReadytoScan()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package where  a.status == "初始" select a.id;
            List <int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }

        // GET: api/PickUp/GetReadytoReceive
        /// <summary>
        /// 获取已下单,要揽件的包裹ID
        /// </summary>
        /// <remarks>获取待揽件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("PickUp/GetReadytoReceive")]
        public List<int> GetReadytoReceive()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package where a.status == "已下单" select a.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }

        // GET: api/PickUp/GetReceiving
        /// <summary>
        /// 获取某员工正在揽件的包裹ID
        /// </summary>
        /// <remarks>获取某员工正在揽件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("PickUp/GetReceiving")]
        public List<int> GetReceiving(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.PickUp join b in db.Package on a.id equals b.id where b.status == "待揽件" && a.mId == account select b.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }

            return list;
        }
        // PUT: api/PickUp/Post
        /// <summary>
        /// 添加待揽件包裹信息
        /// </summary>
        /// <remarks>添加待揽件包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("PickUp/Post")]
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
                db.PickUp.InsertOnSubmit(pk);
                package.status = "待揽件";
                db.SubmitChanges();
                return true;
            }
            catch(Exception)
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
        public bool RevokeReceive(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var x = db.Package.SingleOrDefault(a => a.id == id);
            if(x is null)
            {
                return false;
            }
            else
            {
                if(x.status!="待揽件")
                {
                    return false;
                }
                x.status = "已下单";
                db.SubmitChanges();
                return true;
            }
        }


        // PUT: api/PickUp/Scan?id={id}
        /// <summary>
        /// 包裹的状态从待揽件状态或者初始状态变为已扫件
        /// </summary>
        /// <remarks>包裹的状态从待揽件状态或者初始状态变为已扫件</remarks>
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
                if (x.status != "待揽件"&&x.status!="初始")
                {
                    return false;
                }
                x.status = "已扫件";
                db.SubmitChanges();
                return true; 
            }
        }
    }
}

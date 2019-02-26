using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem
{

    public class DeliveryController : ApiController
    {

        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;

        // GET: api/Delivery/GetReadytoDelivery
        /// <summary>
        /// 获取已扫件,要派件的包裹ID
        /// </summary>
        /// <remarks>获取待派件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetReadytoDelivery")]
        public List<int> GetReadytDelivery()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package where a.status == "已扫件" select a.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                var selectQuery2 = from a in db.AddressBook where a.aId == x select a.street;
                if(selectQuery2.SingleOrDefault() == "华南农业大学")
                    list.Add(x);
            }
            return list;
        }

        // Post: api/Delivery/Post
        /// <summary>
        /// 添加待派件包裹信息
        /// </summary>
        /// <remarks>添加待派件包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Delivery/Post")]
        public bool Post(DeliveryClass x)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var package = db.Package.Single(a => a.id == x.Id);
                if (package.status != "已下单")
                {
                    return false;
                }
                Delivery del = new Delivery();
                del.id = x.Id;
                del.mId = x.Mid;
                del.time = DateTime.Now;
                del.isDone = false;
                db.Delivery.InsertOnSubmit(del);
                package.status = "待派件";
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // PUT: api/Delivery/RevokeDelivery
        /// <summary>
        /// 包裹的状态从待派件撤销为已扫件
        /// </summary>
        /// <remarks>包裹的状态从待派件撤销为已扫件</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Delivery/RevokeDelivery")]
        public bool RevokeDelivery(IdClass iclass)
        {
            db = new DataClasses1DataContext(connstr);
            var x = db.Package.SingleOrDefault(a => a.id == iclass.id);
            if (x is null || x.status != "待派件")
            {
                return false;
            }
            else
            {
                x.status = "已扫件";
                db.SubmitChanges();
                return true;

            }
        }

        // GET: api/Delivery/GetDelivering
        /// <summary>
        /// 获取某员工正在派件的包裹ID
        /// </summary>
        /// <remarks>获取某员工正在派件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetDelivering")]
        public List<int> GetDelivering(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.PickUp.GroupBy(p => p.id).Select(g => g.OrderByDescending(t => t.time).First()) join b in db.Package on a.id equals b.id where b.status == "待派件" && a.mId == account && a.isDone == false select b.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }

            return list;
        }

        // Post: api/Delivery/ConfirmPost
        /// <summary>
        /// 添加已签收包裹信息
        /// </summary>
        /// <remarks>添加已签收包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Delivery/ConfirmPost")]
        public bool ConfirmPost(DeliveryClass x)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var package = db.Package.Single(a => a.id == x.Id);
                if (package.status != "待派件")
                {
                    return false;
                }
                Delivery del = new Delivery();
                del.id = x.Id;
                del.mId = x.Mid;
                del.time = DateTime.Now;
                del.isDone = true;
                db.Delivery.InsertOnSubmit(del);
                package.status = "已签收";
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

    }
}
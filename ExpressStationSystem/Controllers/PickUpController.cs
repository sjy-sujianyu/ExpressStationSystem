using System;
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
        // GET: api/PickUp/GetReadytoScan
        /// <summary>
        /// 获取待扫件的包裹ID
        /// </summary>
        /// <remarks>获取待扫件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("PickUp/GetReadytoScan")]
        public List<int> GetReadytoScan()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package where a.status == "待揽收" || a.status == "待扫件" select a.id;
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
        // POST: api/PickUp
        public void Post([FromBody]string value)
        {
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
                return true;
            }
        }

        // DELETE: api/PickUp/5
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{

    public class TransferController : ApiController
    {

        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;

        // GET: api/Transfer/GetReadyToTransfer
        /// <summary>
        /// 获取已扫件,要出站的包裹ID
        /// </summary>
        /// <remarks>获取待出站的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Transfer/GetReadyToTransfer")]
        public List<int> GetReadyToTransfer()
        {
            db = new DataClasses1DataContext(connstr);
            var readytoDelivery = db.Package.Where(a => a.status == "已扫件").Join(db.AddressBook.Where(a => !a.street.Contains("华南农业大学")), a => a.receiverId, b => b.aId, (a, b) => a.id);
            List<int> list = new List<int>();
            foreach (var x in readytoDelivery)
            {
                list.Add(x);
            }
            return list;
        }

        // Post: api/Transfer/PostList
        /// <summary>
        /// 添加出站包裹信息
        /// </summary>
        /// <param name="ids">包裹id数组</param>
        /// <param name="m">员工id</param>
        /// <param name="v">交通工具id</param>
        /// <remarks>添加出站包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Transfer/PostList")]
        public bool PostList(List<int> ids, VidClass v, MemberClass m)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                foreach (var id in ids)
                {
                    Transfer tran = new Transfer();
                    TransferClass t = new TransferClass();
                    t.id = id;
                    t.mid = m.mId;
                    t.vid = v.vId;
                    Post(t);
                    db.SubmitChanges();
                }
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }


        // Post: api/Transfer/Post
        /// <summary>
        /// 添加出站包裹信息
        /// </summary>
        /// <param name="x">出站包裹信息</param>
        /// <remarks>添加出站包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Transfer/Post")]
        public bool Post(TransferClass x)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var package = db.Package.Single(a => a.id == x.id);
                if (package.status != "已扫件")
                {
                    return false;
                }
                Transfer tran = new Transfer();
                tran.id = x.id;
                tran.mId = x.mid;
                tran.time = DateTime.Now;
                tran.isDone = false;
                db.Transfer.InsertOnSubmit(tran);
                package.status = "运输中";
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // GET: api/Transfer/GetPackageIdOnVehicle
        /// <summary>
        /// 获取已上车的包裹ID
        /// </summary>
        /// <param name="x">交通工具id</param>
        /// <remarks>获取已上车的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Transfer/GetPackageIdOnVehicle")]
        public List<int> GetPackageIdOnVehicle(VidClass x)
        {
            db = new DataClasses1DataContext(connstr);
            var transfering = db.Package.Where(a => a.status == "运输中");
            List<int> list = new List<int>();
            foreach (var t in transfering)
            {
                var tran = db.Transfer.Where(a => a.id == t.id).OrderByDescending(a => a.time).FirstOrDefault();
                if(tran != null && tran.isDone)
                    list.Add(tran.id);
            }
            return list;
        }

        // PUT: api/Transfer/Departure
        /// <summary>
        /// 交通工具从站点出发
        /// </summary>
        /// <param name="x">交通工具id</param>
        /// <remarks>交通工具从站点出发</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Transfer/Departure")]
        public bool Departure(VidClass x)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var vehicle = db.Vehicle.Single(a => a.vId == x.vId);
                if (vehicle == null) return false;
                var packageOnVehicle = GetPackageIdOnVehicle(x);
                foreach(var id in packageOnVehicle)
                {
                    var p = db.Package.SingleOrDefault(a => a.id == id);
                    var tran = db.Transfer.Where(a => a.id == p.id).OrderByDescending(a => a.time).FirstOrDefault();
                    if (p != null)
                    {
                        tran.isDone = true;
                    }
                }
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // PUT: api/Transfer/RevokeTransfer
        /// <summary>
        /// 把包裹从车上卸下
        /// </summary>
        /// <param name="x">包裹id</param>
        /// <remarks>把包裹从车上卸下</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Transfer/RevokeTransfer")]
        public bool RevokeTransfer(IdClass x)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var transfer = db.Transfer.Where(a => a.id == x.id).OrderByDescending(a=>a.id == x.id).FirstOrDefault();
                if (transfer.isDone == true) return false;
                var p = db.Package.SingleOrDefault(a => a.id == x.id);
                p.status = "已扫件";
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

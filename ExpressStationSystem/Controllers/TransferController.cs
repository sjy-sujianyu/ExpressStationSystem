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


        // Post: api/Transfer/Post
        /// <summary>
        /// 添加出站包裹信息
        /// </summary>
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

        // PUT: api/Transfer/Departure
        /// <summary>
        /// 交通工具从站点出发
        /// </summary>
        /// <remarks>添加出站包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Transfer/Departure")]
        public bool Departure(VidClass x)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var vehicle = db.Vehicle.Single(a => a.vId == x.vId);
                if (vehicle == null) return false;
                var transferOnVehicle = db.Transfer.Where(a => a.isDone == false && a.vId == x.vId);
                foreach(var tran in transferOnVehicle)
                {
                    tran.isDone = true;
                    var id = tran.id;
                    var p = db.Package.SingleOrDefault(a => a.id == id);
                }
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

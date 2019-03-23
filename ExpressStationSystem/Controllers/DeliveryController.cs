using ExpressStationSystem.Controllers;
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

        // GET: api/Delivery/GetReadytoDeliveryByCondition
        /// <summary>
        /// 按条件查询要派件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <remarks>按条件查询要派件的包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetReadytoDeliveryByCondition")]
        public dynamic GetReadytoDeliveryByCondition(string str, string type,int page,int pageSize)
        {
            if (str is null || type is null)
            {
                return null;
            }
            db = new DataClasses1DataContext(connstr);
            var a = GetReadytoDelivery(0,0);
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

        // GET: api/Delivery/GetDeliveringByCondition
        /// <summary>
        /// 按条件查询正在派件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <param name="account">员工的mId</param>
        /// <remarks>按条件查询正在派件的包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetDeliveringByCondition")]
        public List<int> GetDeliveringByCondition(string account, string str, string type)
        {
            if (str is null || type is null)
            {
                return null;
            }
            db = new DataClasses1DataContext(connstr);
            var a = GetDelivering(account);
            List<int> list = new List<int>();
            foreach (var x in a)
            {
                var ob = new QueryController().GetAllInfo(x);
                if (type == "单号" && ob.package.id.ToString().StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "姓名" && ob.src.name.StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "电话" && ob.src.phone.StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "街道" && ob.src.street.StartsWith(str))
                {
                    list.Add(x);
                }
            }
            return list;
        }



        // GET: api/Delivery/GetReadytoDelivery
        /// <summary>
        /// 获取已扫件,要派件的包裹ID
        /// </summary>
        /// <remarks>获取待派件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetReadytoDelivery")]
        public dynamic GetReadytoDelivery(int page,int pageSize)
        {
            db = new DataClasses1DataContext(connstr);
            var readytoDelivery = from a in db.Package join b in db.AddressBook on a.sendId equals b.aId join c in db.AddressBook on a.receiverId equals c.aId where a.status == "已扫件" && c.street.Contains("华南农业大学") select new { package = a, src = b, dest = c };

            List<dynamic> list = new List<dynamic>();
            foreach (var x in readytoDelivery)
            {
                list.Add(x);
            }
            return new ToolsController().splitpage(list, page, pageSize);
        }

        // Post: api/Delivery/Post
        /// <summary>
        /// 添加派件中包裹信息
        /// </summary>
        /// <remarks>添加派件中包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Delivery/Post")]
        public bool Post(DeliveryClass x)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var package = db.Package.Single(a => a.id == x.Id);
                if (package.status != "已扫件")
                {
                    return false;
                }
                Delivery del = new Delivery();
                del.id = x.Id;
                del.mId = x.Mid;
                del.time = DateTime.Now;
                del.isDone = false;
                package.time = DateTime.Now;
                db.Delivery.InsertOnSubmit(del);
                package.status = "派件中";
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // GET: api/Delivery/IsRefuse
        /// <summary>
        /// 查看包裹是否为退件包裹
        /// </summary>
        /// <param name="id">包裹id</param>
        /// <remarks>查看包裹是否为退件包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/IsRefuse")]
        public bool IsRefuse(int id)
        {
            db = new DataClasses1DataContext(connstr);
            if (db.Error.SingleOrDefault(a => a.id == id && a.status == "拒签") is null)
            {
                return false;
            }
            else return true;
        }

        // PUT: api/Delivery/RevokeDelivery
        /// <summary>
        /// 包裹的状态从派中件撤销为已扫件
        /// </summary>
        /// <remarks>包裹的状态从派件中撤销为已扫件</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Delivery/RevokeDelivery")]
        public bool RevokeDelivery(IdClass iclass)
        {
            db = new DataClasses1DataContext(connstr);
            var x = db.Package.SingleOrDefault(a => a.id == iclass.id);
            if (x is null || x.status != "派件中")
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

        // PUT: api/Delivery/SwapAddress
        /// <summary>
        /// 交换发件人和收件人地址
        /// </summary>
        /// <remarks>交换发件人和收件人地址</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Delivery/SwapAddress")]
        public bool SwapAddress(IdClass iclass)
        {
            db = new DataClasses1DataContext(connstr);
            var x = db.Package.SingleOrDefault(a => a.id == iclass.id);
            if (x is null) return false;
            int temp = x.receiverId;
            int temp1 = x.sendId;
            x.receiverId = temp1;
            x.sendId = temp;
            return true;
        }

        //PUT: api/Delivery/Refuse
        /// <summary>
        /// 包裹拒签
        /// </summary>
        /// <remarks>包裹拒签，状态从派件中撤销为已扫件</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Delivery/Refuse")]
        public bool Refuse(IdClass iclass)
        {
            db = new DataClasses1DataContext(connstr);
            var x = db.Package.SingleOrDefault(a => a.id == iclass.id);
            if (x is null || x.status != "派件中" || IsRefuse(iclass.id))
            {
                return false;
            }
            else
            {
                RevokeDelivery(iclass);
                SwapAddress(iclass);
                Error error = new Error();
                error.id = iclass.id;
                error.introduction = "客户拒收快递";
                error.status = "拒签";
                error.time = DateTime.Now;
                x.time = DateTime.Now;
                db.Error.InsertOnSubmit(error);
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
            var selectQuery = from a in db.Delivery.GroupBy(p => p.id).Select(g => g.OrderByDescending(t => t.time).First()) join b in db.Package on a.id equals b.id where b.status == "派件中" && a.mId == account && a.isDone == false select b.id;
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
                if (package.status != "派件中")
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
                package.time = DateTime.Now;
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
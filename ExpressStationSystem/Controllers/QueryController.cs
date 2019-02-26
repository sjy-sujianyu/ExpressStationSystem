using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class QueryController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;

        // GET: api/Query/GetRole?account={account}
        /// <summary>
        /// 返回员工角色(揽件员，派件员，出件员)
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回员工角色(揽件员，派件员，出件员)</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetRole")]
        public string GetRole(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var role = db.Member.SingleOrDefault(a => a.mId == account && a.isDelete == false);
            if(role is null)
            {
                return null;
            }
            else
            {
                return role.job;
            }
        }
        // GET: api/Query/GetLogisticsInfo?account={account}
        /// <summary>
        /// 根据包裹得到物流信息
        /// </summary>
        /// <param name="id">包裹ID</param>
        /// <remarks>根据包裹得到物流信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetLogisticsInfo")]
        private List<dynamic> GetLogisticsInfo(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Path join b in db.Branch on a.curId equals b.bId join c in db.Vehicle on a.vId equals c.vId where a.id==id orderby a.time
                              select new { path = a, branch = b,vehicle=c };
            List<dynamic> list = new List<dynamic>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }

        // GET: api/Query/GetAllBillByAccount?account={account}
        /// <summary>
        /// 根据客户ID获取所有订单列表
        /// </summary>
        /// <param name="account">客户ID</param>
        /// <remarks>根据客户ID获取所有订单列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetAllBillByAccount")]
        public List<int> GetAllBillByAccount(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package where a.account == account select a.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }

        // GET: api/Query/GetAllPackagesByAccount?account={account}
        /// <summary>
        /// 根据客户ID获取所有接受的快递
        /// </summary>
        /// <param name="account">客户ID</param>
        /// <remarks>根据客户ID获取所有接受的快递</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetAllPackagesByAccount")]
        public List<int> GetAllPackagesByAccount(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package join b in db.AddressBook on a.receiverId equals b.aId where b.phone==account select a.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }
        // GET: api/Query/GetAllInfo?account={account}
        /// <summary>
        /// 得到包裹全部信息
        /// </summary>
        /// <param name="id">包裹ID</param>
        /// <remarks>得到包裹全部信息 包裹状态有已下单、待揽件、已扫件、运输中、派件中、已签收状态</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetAllInfo")]
        public dynamic GetAllInfo(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var package = db.Package.SingleOrDefault(a => a.id == id);
            if(package is null)
            {
                return null;
            }
            var pickup = db.PickUp.Where(a=>a.id==package.id).OrderByDescending(a=>a.time).Join(db.Member, a => a.mId, b => b.mId, (a, b) => new { pickup = a, member = b });
            var src = db.AddressBook.SingleOrDefault(a => a.aId == package.sendId);
            var dest = db.AddressBook.SingleOrDefault(a => a.aId == package.receiverId);
            var delivery = db.Delivery.Where(a => a.id == package.id).OrderByDescending(a => a.time).Join(db.Member, a => a.mId, b => b.mId, (a, b) => new { delivery = a, member = b });
            var transfer = db.Transfer.Where(a => a.id == package.id).OrderByDescending(a => a.time).Join(db.Member,a=>a.mId,b=>b.mId,(a,b)=>new { transfer = a, member = b });
            var list=GetLogisticsInfo(id);
            return new { package = package, pickup = pickup, src = src, dest = dest, delivery = delivery, transfer = transfer,pathList=list };
        }
        // POST: api/Query
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Query/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Query/5
        public void Delete(int id)
        {
        }
    }
}

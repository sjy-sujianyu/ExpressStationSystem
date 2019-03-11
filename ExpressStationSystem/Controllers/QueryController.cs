using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
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
        /// <remarks>返回员工角色(揽件员，派件员，出件员,经理,待定中)</remarks>
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
        // POST: api/Query/isTel?tb={tb}
        /// <summary>
        /// 验证手机号码是否合法
        /// </summary>
        /// <param name="tb">手机号码</param>
        /// <remarks>验证手机号码是否合法</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/isTel")]
        public bool isTel(string tb)
        {
            string s = @"^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\d{8}$";
            bool flag = true;
            if (!Regex.IsMatch(tb, s))
            {
                flag = false;
            }
            return flag;
        }

        // GET: api/Query/GetMemberAllInfo?account={account}
        /// <summary>
        /// 返回员工信息
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回员工信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetMemberAllInfo")]
        public dynamic GetMemberAllInfo(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var member = db.Member.SingleOrDefault(a => a.mId == account);
            if(member is null)
            {
                return null;
            }
            else
            {
                return member;
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
            var selectQuery = from a in db.Path where a.id==id orderby a.time
                              select new {pId=a.pId,id=a.id,srcPlace=splitPlace(a.srcPlace),destPlace=splitPlace(a.destPlace),curPlace=splitPlace(a.curPlace),isArrival=a.isArrival,time=a.time,vehicle=a.vehicle };
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
        /// <remarks>得到包裹全部信息 包裹状态有已下单、待揽件、已扫件、运输中、派件中、已签收状态
        /// <br>
        /// 错误状态：错件，漏件，拒收，破损
        /// </br>
        /// </remarks>
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
            var error = db.Error.SingleOrDefault(a => a.id == package.id);
            var list=GetLogisticsInfo(id);
            return new { package = package, pickup = pickup, src = src, dest = dest, delivery = delivery, transfer = transfer,pathList=list,error=error };
        }

        

        // GET: api/Query/GetTotalRecordByAccount?account={account}
        /// <summary>
        /// 根据总员工历史任务记录
        /// </summary>
        /// <param name="account">客户ID</param>
        /// <remarks>根据总员工历史任务记录</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetTotalRecordByAccount")]
        public dynamic GetTotalRecordByAccount(string account)
        {
            db = new DataClasses1DataContext(connstr);
            int pCount = 0;
            int dCount = 0;
            int tCount = 0;
            pCount = db.PickUp.Where(a => a.mId == account && a.isDone == true).Count();
            dCount = db.Delivery.Where(a => a.mId == account && a.isDone == true).Count();
            tCount = db.Transfer.Where(a => a.mId == account && a.isDone == true).Count();
            return new { PickUpCount = pCount, DeliveryCount = dCount, TransferCount = tCount };
        }
        // GET: api/Query/GetErrorPackage
        /// <summary>
        /// 根据错误包裹
        /// </summary>
        /// <remarks>根据错误包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetErrorPackage")]
        public List<int> GetErrorPackage()
        {
            db = new DataClasses1DataContext(connstr);
            List<int> list = new List<int>();
            var error = from a in db.Error select a.id;
            foreach (var x in error)
            {
                list.Add(x);
            }
            return list;
        }
        // GET: api/Query/GetErrorPackage
        /// <summary>
        /// 获得某员工被投诉的记录
        /// </summary>
        /// <remarks>获得某员工被投诉的记录</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetErrorByAccount")]
        public List<dynamic> GetErrorByAccount(string account)
        {
            db = new DataClasses1DataContext(connstr);
            List<dynamic> list = new List<dynamic>();
            var error = from a in db.Error join b in db.Delivery on a.id equals b.id where (a.status == "破损" || a.status == "丢件")&&b.isDone==true&&b.mId==account select a;
            foreach(var x in error)
            {
                list.Add(x);
            }
            return list;
        }

        private dynamic splitPlace(string place)
        {
            string[] str = place.Split('-');
            return new { province = str[0],city=str[1],street=str[2] };
        }
    }
}

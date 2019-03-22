using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class QueryController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;

        // GET: api/Query/GetRole?account={account}
        /// <summary>
        /// 返回员工角色(揽件员，派件员，出件员,经理,待定中,休息中)
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回员工角色(揽件员，派件员，出件员,经理,待定中,休息中)</remarks>
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
                if(role.onDuty==false)
                {
                    return "休息中";
                }
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
        // GET: api/Query/GetLogisticsInfo?id={id}
        /// <summary>
        /// 根据包裹得到物流信息
        /// </summary>
        /// <param name="id">包裹ID</param>
        /// <remarks>根据包裹得到物流信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetLogisticsInfo")]
        public List<dynamic> GetLogisticsInfo(int id)
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
        /// 错误状态：错件，漏件，拒收，破损，丢件
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
            var pickup = db.PickUp.Where(a => a.id == package.id).OrderByDescending(a => a.time).Join(db.Member, a => a.mId, b => b.mId, (a, b) => new { pickup = a, member = b }).ToList() ;
            var src = db.AddressBook.SingleOrDefault(a => a.aId == package.sendId);
            var dest = db.AddressBook.SingleOrDefault(a => a.aId == package.receiverId);
            var delivery = db.Delivery.Where(a => a.id == package.id).OrderByDescending(a => a.time).Join(db.Member, a => a.mId, b => b.mId, (a, b) => new { delivery = a, member = b }).ToList();
            var transfer = db.Transfer.Where(a => a.id == package.id).OrderByDescending(a => a.time).Join(db.Member,a=>a.mId,b=>b.mId,(a,b)=>new { transfer = a, member = b }).ToList();
            var error = db.Error.Where(a => a.id == package.id).ToList();
            var list=GetLogisticsInfo(id);
            var acl = new { package = package, pickup = pickup, src = src, dest = dest, delivery = delivery, transfer = transfer, pathList = list, error = error };
            return acl;
        }

        // GET: api/Query/GetAllInfo?account={account}
        /// <summary>
        /// 快速得到时间段内包裹全部信息
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <remarks>快速得到时间段内包裹全部信息 </remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetAllInfoFast")]
        public dynamic GetAllInfoFast(DateTime start, DateTime end)
        {
            List<dynamic> list = new List<dynamic>();
            db = new DataClasses1DataContext(connstr);
            var package = db.Package.Where(a=> DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0)
                .Join(db.AddressBook, a => a.sendId, b => b.aId, (a, b) => new { a = a, b = b })
                .Join(db.AddressBook, a => a.a.receiverId, b => b.aId, (a, b) => new { package = a.a, src = a.b, dest = b })
                .GroupJoin(db.Error, x => x.package.id, y => y.id, (x, y) => y.DefaultIfEmpty().Select(z => new { package = x.package, src = x.src,dest=x.dest,error=y })).SelectMany(x => x).GroupBy(a=>a.package.id).Select(g=>g.First());
            //var tasks = new List<Task<dynamic>>();
            //List<dynamic> errorList = new List<dynamic>();
            //foreach (var x in package)
            //{
            //    var y = new Task<dynamic>(() => new QueryController().getError(x.package.id));
            //    y.Start();
            //    tasks.Add(y);
            //}
            //Task.WaitAll(tasks.ToArray());
            //foreach(var x in tasks)
            //{
            //    errorList.Add(x.Result);
            //}
            //int i = 0;
            //foreach(var x in package)
            //{
            //    var result = new { package = x.package, src = x.src, dest = x.dest, error = errorList[i] };
            //    list.Add(result);
            //    i++;
            //}
            return package;
        }
        public dynamic getError(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var error = db.Error.Where(a => a.id == id).ToList(); ;
            return error;
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
            tCount = db.Transfer.Where(a => a.mId == account).Count();
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
            var error = from a in db.Error join b in db.Delivery on a.id equals b.id where (a.status == "破损" || a.status == "丢件") orderby b.time descending group new { error=a, delivery=b } by b.id into g select g.First();
            foreach(var x in error)
            {
                if(x.delivery.mId==account)
                {
                    list.Add(x);
                }
            }
            return list;
        }
        // GET: api/Query/GetStatistic
        /// <summary>
        /// 获得各种统计量
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <remarks>获得各种统计量</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Query/GetStatistic")]
        public dynamic GetStatistic(DateTime start,DateTime end)
        {
            db = new DataClasses1DataContext(connstr);
            var errorError = from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "错件" select a;
            var errorLeak = from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "漏件" select a;
            var errorDamaged= from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "破损" select a;
            var errorRefused = from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "拒签" select a;
            var errorLose = from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "丢件" select a;
            var delivery = from a in db.Delivery where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDone == true select a;
            var Inbound = from a in db.Package where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0&&a.status=="已扫件" select a;
            var transfer = from a in db.Transfer where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 select a;
            var leave = from a in db.Leave where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == 2 select a;
            var memberEmploy = from a in db.Member where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDelete == false select a;
            var memberFired= from a in db.Member where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDelete == true select a;
            var commission = from a in db.Commission where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 select a;
            var vehicleEmploy = from a in db.Vehicle where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDelete == false select a;
            var vehicleFired = from a in db.Vehicle where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDelete == true select a;
            return new { errorError = new { content = errorError.ToList(), cnt = errorError.Count() }, errorLeak = new { content = errorLeak.ToList(), cnt = errorLeak.Count() }, errorDamaged = new { content = errorDamaged.ToList(), cnt = errorDamaged.Count() }, errorRefused = new { content = errorRefused.ToList(), cnt = errorRefused.Count() }, errorLose = new { content = errorLose.ToList(), cnt = errorLose.Count() }, delivery = new { content = delivery.ToList(), cnt = delivery.Count() }, pickUp = new { content = Inbound.ToList(), cnt = Inbound.Count() }, transfer = new { content = transfer.ToList(), cnt = transfer.Count() }, leave = new { content = leave.ToList(), cnt = leave.Count() }, memberEmploy = new { content = memberEmploy.ToList(), cnt = memberEmploy.Count() }, memberFired = new { content = memberFired.ToList(), cnt = memberFired.Count() }, commission = new { content = commission.ToList(), cnt = commission.Count() }, vehicleEmploy = new { content = vehicleEmploy.ToList(), cnt = vehicleEmploy.Count() }, vehicleFired = new { content = vehicleFired.ToList(), cnt = vehicleFired.Count() } };
        }
        private dynamic splitPlace(string place)
        {
            string[] str = place.Split('-');
            return new { province = str[0],city=str[1],street=str[2] };
        }
    }
}

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
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class QueryController : ApiController
    {

        
        // GET: api/Query/GetRole?account={account}
        /// <summary>
        /// 返回员工角色(揽件员，派件员，出件员,经理,待定中,休息中)
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回员工角色(揽件员，派件员，出件员,经理,待定中,休息中)</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public string GetRole(string account)
        {
            return new Member().GetRole(account);
        }
        // POST: api/Query/isTel?tb={tb}
        /// <summary>
        /// 验证手机号码是否合法
        /// </summary>
        /// <param name="tb">手机号码</param>
        /// <remarks>验证手机号码是否合法</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public bool isTel(string tb)
        {
            return Global.isTel(tb);
        }

        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <remarks>获取公告信息</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public string GetPublishMessage()
        {
            return new Member().GetPublishMessage();
        }
        // GET: api/Query/GetMemberAllInfo?account={account}
        /// <summary>
        /// 返回员工信息
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回员工信息</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetMemberAllInfo(string account)
        {
            return new Member().GetMemberAllInfo(account);
        }
        // GET: api/Query/GetLogisticsInfo?id={id}
        /// <summary>
        /// 根据包裹得到物流信息
        /// </summary>
        /// <param name="id">包裹ID</param>
        /// <remarks>根据包裹得到物流信息</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<dynamic> GetLogisticsInfo(int id)
        {
            return new Path().GetLogisticsInfo(id);
        }

        // GET: api/Query/GetAllBillByAccount?account={account}
        /// <summary>
        /// 根据客户ID获取所有订单列表
        /// </summary>
        /// <param name="account">客户ID</param>
        /// <remarks>根据客户ID获取所有订单列表</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<int> GetAllBillByAccount(string account)
        {
            return new Package().GetAllBillByAccount(account);
        }

        // GET: api/Query/GetAllPackagesByAccount?account={account}
        /// <summary>
        /// 根据客户ID获取所有接受的快递
        /// </summary>
        /// <param name="account">客户ID</param>
        /// <remarks>根据客户ID获取所有接受的快递</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<int> GetAllPackagesByAccount(string account)
        {
            return new Package().GetAllPackagesByAccount(account);
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
        [HttpGet]
        public dynamic GetAllInfo(int id)
        {
            return new Package().GetAllInfo(id);
        }

        // GET: api/Query/GetAllInfo?account={account}
        /// <summary>
        /// 快速得到时间段内包裹全部信息
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <remarks>快速得到时间段内包裹全部信息 </remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetAllInfoFast(DateTime start, DateTime end)
        {
            return new Package().GetAllInfoFast(start, end);
        }
        // GET: api/Query/GetTotalRecordByAccount?account={account}
        /// <summary>
        /// 根据总员工历史任务记录
        /// </summary>
        /// <param name="account">客户ID</param>
        /// <remarks>根据总员工历史任务记录</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetTotalRecordByAccount(string account)
        {
            return new Member().GetTotalRecordByAccount(account);
        }
        // GET: api/Query/GetErrorPackage
        /// <summary>
        /// 根据错误包裹
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <param name="page">当前页数</param>
        /// <param name="pageSize">总共页数</param>
        /// <param name="status">错误信息状态：拒签、错件、漏件、破损、丢件、全部</param>
        /// <remarks>根据错误包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<int> GetErrorPackage(DateTime start, DateTime end,int page,int pageSize,string status)
        {
            return new Error().GetErrorPackage(start,end,page,pageSize,status);
        }
        // GET: api/Query/GetErrorPackage
        /// <summary>
        /// 获得某员工被投诉的记录
        /// </summary>
        /// <remarks>获得某员工被投诉的记录</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<dynamic> GetErrorByAccount(string account)
        {
            return new Error().GetErrorByAccount(account);
        }
        // GET: api/Query/GetStatistic
        /// <summary>
        /// 获得各种统计量
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <remarks>获得各种统计量</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetStatistic(DateTime start, DateTime end)
        {
            return Global.GetStatistic(start, end);
        }

        /// <summary>
        /// [NotToken]根据账号密码获取token
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <remarks>根据账号密码获取token</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public string GetAdminToken(string account, string password)
        {
            return Global.GetAdminToken(account, password);

        }
        /// <summary>
        /// [NotToken]校验最高权限token的合法性
        /// </summary>
        [HttpGet]
        public bool AdminValidateTicket(string encryptTicket)
        {
            return Global.ValidateTicket(encryptTicket);
        }
    }
}

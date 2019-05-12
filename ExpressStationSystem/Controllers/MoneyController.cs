using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Web.Http;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class MoneyController : ApiController
    {
        // GET: api/Money/GetSalary?account={account}
        /// <summary>
        /// 获取员工工资详情
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <param name="time">发工资日</param>
        /// <remarks>获取员工工资详情</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetSalary(string account, DateTime time)
        {
            return new Money().GetSalary(account, time);
        }
        // GET: api/Leave/GetMoneyInfo
        /// <summary>
        /// 获取历史提成价格
        /// </summary>
        /// <remarks>获取历史提成价格</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetCommision()
        {
            return new Commission().GetCommision();
        }
        // GET: api/Leave/GetMoneyInfo
        /// <summary>
        /// 获取奖罚信息
        /// </summary>
        /// <param name="id">奖罚信息id</param>
        /// <remarks>获取奖罚信息</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetMoneyInfo(int id)
        {
            return new Money().GetMoneyInfo(id);
        }
        // PUT: api/Money/GetMoneyHistory
        /// <summary>
        /// 获取金钱条目历史记录
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <remarks>获取金钱条目历史记录</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<int> GetMoneyHistory(DateTime start, DateTime end)
        {
            return new Money().GetMoneyHistory(start, end);
        }

        // PUT: api/Money/ErrorPost
        /// <summary>
        /// 需要定时调用的函数插入漏件，错件
        /// </summary>
        /// <remarks>需要定时调用的函数插入漏件，错件</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public void ErrorPost(object obj)
        {
            new Error().ErrorPost(obj);
        }
        // PUT: api/Money/Complaint
        /// <summary>
        /// 客户投诉
        /// </summary>
        /// <param name="x">罚款实体信息</param>
        /// <remarks>客户投诉</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Complaint(ErrorClass x)
        {
            return new Error().Complaint(x);
        }
        // PUT: api/Money/PostCommission
        /// <summary>
        /// 插入提成价格
        /// </summary>
        /// <param name="x">提成价格实体</param>
        /// <remarks>插入提成价格</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool PostCommission(CommisionClass x)
        {
            return new Commission().PostCommission(x);
        }
        // POST: api/Money/FineorPrize
        /// <summary>
        /// 罚款
        /// </summary>
        /// <param name="x">罚款实体信息</param>
        /// <remarks>罚款</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool FineorPrize(MoneyClass x)
        {
            return new Money().FineorPrize(x);
        }
        // PUT: api/Money/UpdateMoneyItem
        /// <summary>
        /// 更新罚款补贴信息
        /// </summary>
        /// <param name="x">罚款补贴实体信息</param>
        /// <remarks>更新罚款补贴信息</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool UpdateMoneyItem(MoneyClassPlus x)
        {
            return new Money().UpdateMoneyItem(x);
        }
        // DELETE: api/Money/Delete
        /// <summary>
        /// 删除罚款
        /// </summary>
        /// <param name="id">金额项目id</param>
        /// <remarks>删除罚款</remarks>
        /// <returns>返回</returns>
        [HttpDelete]
        public bool Delete(int id)
        {
            return new Money().Delete(id);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class MoneyController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Money/GetSalary?account={account}
        /// <summary>
        /// 获取员工工资详情
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <param name="time">发工资日</param>
        /// <remarks>获取员工工资详情</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Money/GetSalary")]
        public dynamic GetSalary(string account, DateTime time)
        {
            db = new DataClasses1DataContext(connstr);
            var member = db.Member.SingleOrDefault(a => a.mId == account);
            if (member is null)
            {
                return null;
            }
            var record = new QueryController().GetRecordByAccount(account, time.AddMonths(-1), time);
            var value = db.Commission.OrderBy(a => a.time).FirstOrDefault();
            if (value is null)
            {
                return null;
            }
            //提成
            dynamic commission = new { pickUp = new { PickUpCount = record.PickUpCount, value = value.pickUpValue, total = record.PickUpCount * value.pickUpValue }, delivery = new { DeliveryCount = record.DeliveryCount, value = value.deliveryValue, total = record.DeliveryCount * value.deliveryValue }, transfer = new { TransferCount = record.TransferCount, value = value.deliveryValue, total = record.TransferCount * value.deliveryValue } };
            //补贴
            decimal subsidy = 0;
            //罚款
            decimal fine = 0;

            List<dynamic> subsidyList = new List<dynamic>();
            List<dynamic> fineList = new List<dynamic>();
            var money = db.Money.Where(a => a.mId == account && DateTime.Compare(a.time, time.AddMonths(-1)) >= 0 && DateTime.Compare(a.time, time) <= 0);
            foreach (var x in money)
            {
                //补贴
                if (x.subsidy != 0)
                {
                    subsidy += x.subsidy;
                    subsidyList.Add(x);
                }
                //罚款
                else
                {
                    fine += x.fine;
                    fineList.Add(x);
                }
            }
            return new { baseSalary = member.baseSalary, commission = commission, subsidy = new { details = subsidyList, total = subsidy }, fine = new { details = fineList, total = fine } };
        }

        // GET: api/Salary/5
        public string Get(int id)
        {
            return "value";
        }

        // POST: api/Salary
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Salary/Fine
        /// <summary>
        /// 罚款
        /// </summary>
        /// <param name="x">罚款实体信息</param>
        /// <remarks>罚款</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Money/Fine")]
        public bool Fine(dynamic x)
        {
            return false;
        }

        // DELETE: api/Salary/5
        public void Delete(int id)
        {
        }
    }
}

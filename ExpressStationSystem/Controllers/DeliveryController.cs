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

        private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;


        // GET: api/Delivery/GetAllDelivery
        /// <summary>
        /// 获取所有待配送和已配送的快递ID列表
        /// </summary>
        /// <remarks>当前方法获取所有待配送和已配送的快递ID列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetAllDelivery")]
        public IEnumerable<int> GetAllDelivery()
        {
            db = new DataClasses1DataContext(connstr);
            List<int> ids = new List<int>();
            var query =
                from delivery in db.Delivery
                select delivery;
            foreach(var del in query)
            {
                ids.Add(del.id);
            }
            return ids;
        }

        // GET: api/Delivery/IsDelivered?id={id}
        /// <summary>
        /// 根据快递ID查看快递是否已签收
        /// </summary>
        /// <param name="id">快递ID</param>
        /// <remarks>当前方法根据快递ID查看快递是否已签收</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/IsDelivered")]
        public bool IsDelivered(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var query =
                from delivery in db.Delivery
                where delivery.id == id
                select delivery;
            Delivery del = query.FirstOrDefault();
            return del != null && del.status != "待配送";
        }

        // GET: api/Delivery/GetDeliveryById?id={id}
        /// <summary>
        /// 根据快递ID查看快递信息
        /// </summary>
        /// <param name="id">快递ID</param>
        /// <remarks>当前方法根据快递ID查看快递信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetDeliveryById")]
        public Delivery GetDeliveryById(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var query =
                from delivery in db.Delivery
                where delivery.id == id
                select delivery;
            Delivery del = query.FirstOrDefault();
            return del;
        }

        // GET api/<controller>
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET api/<controller>/5
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<controller>
        public void Post([FromBody]string value)
        {
        }

        // PUT api/<controller>/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE api/<controller>/5
        public void Delete(int id)
        {
        }
    }
}
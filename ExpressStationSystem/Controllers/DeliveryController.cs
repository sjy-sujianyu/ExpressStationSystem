using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class DeliveryController : ApiController
    {
        private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;

        // GET api/<controller>
        /// <summary>
        /// 根据客户ID获取快递信息
        /// </summary>
        /// <param name="id">快递ID</param>
        /// <remarks>当前方法根据快递ID返回快递派件信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/Get")]
        public IEnumerable<Delivery> GetAllDelivery(int id)
        {
            List<Delivery> deliveries = new List<Delivery>();
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from Delivery in db.Delivery select Delivery;
            deliveries.AddRange(selectQuery);
            return deliveries;
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
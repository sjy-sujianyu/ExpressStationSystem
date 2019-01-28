using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class CustomerController : ApiController
    {
        private static string connstr=@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Customer/Get?{id}
        /// <summary>
        /// 根据客户ID获取客户的信息
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <remarks>当前方法根据客户ID返回客户的信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Customer/Get")]
        public IHttpActionResult Get(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from customer in db.Customer where customer.cId==id select customer;
            Customer cus = selectQuery.FirstOrDefault();
            if(cus is null)
            {
                return NotFound();
            }
            return Json<Customer>(cus);
        }

        // POST: api/Customer
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Customer/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Customer/5
        public void Delete(int id)
        {
        }
    }
}

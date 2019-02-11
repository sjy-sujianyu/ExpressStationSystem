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
        private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Query/GetLogisticsInfo?account={account}
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
            var selectQuery = from a in db.Path join b in db.Branch on a.curId equals b.bId where a.id==id select new { path = a, branch = b };
            List<dynamic> list = new List<dynamic>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
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

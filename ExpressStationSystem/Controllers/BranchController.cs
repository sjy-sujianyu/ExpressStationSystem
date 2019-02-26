using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class BranchController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        
        // GET: api/Branch/Get
        /// <summary>
        /// 根据地址得到网点ID
        /// </summary>
        /// <param name="province">省份</param>
        /// <param name="city">城市</param>
        /// <param name="street">街道</param>
        /// <remarks>根据地址得到网点ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Branch/Get")]
        public List<int> Get(string province,string city,string street)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from branch in db.Branch where branch.province == province && branch.city==city&&branch.street==street select branch;
            List<int> list = new List<int>();
            foreach(var x in selectQuery)
            {
                list.Add(x.bId);
            }
            return list;
        }

        // POST: api/Branch
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/Branch/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Branch/5
        public void Delete(int id)
        {
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class SalaryController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Salary/Get
        /// <summary>
        /// 获取某月工资信息
        /// </summary>
        /// <remarks>获取某月工资信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Salary/Get")]
        public List<dynamic> Get(DateTime x)
        {
            return null;
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
        [HttpPut, Route("Salary/Fine")]
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

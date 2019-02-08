using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class LoginController : ApiController
    {
        private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Login/Get
        /// <summary>
        /// 检验登陆是否成功
        /// </summary>
        /// <param name="account">账号  可以是openId也可以是普通账户</param>
        /// <param name="password">密码</param>
        /// <remarks>检验登陆是否成功</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Login/Land")]
        public bool Land(string account,string password)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                Login login = db.Login.Single(l => l.account == account&&l.password==password);
                return true;
            }
            catch(Exception)
            {
                return false;
            }
            
        }

        // GET: api/Login/Post
        /// <summary>
        /// 插入账户密码角色
        /// </summary>
        /// <param name="a">账户实体信息</param>
        /// <remarks>插入账户密码角色</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Login/Post")]
        public bool Post(LoginClass a)
        {
            db = new DataClasses1DataContext(connstr);
            Login login = new Login();
            login.account = a.account;
            login.password = a.password;
            login.role = a.role;
            try
            {
                db.Login.InsertOnSubmit(login);
                db.SubmitChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        // PUT: api/Login/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Login/5
        public void Delete(int id)
        {
        }
    }
}

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
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Login/Get
        /// <summary>
        /// 检验登陆是否成功
        /// </summary>
        /// <param name="account">账号  手机号码</param>
        /// <param name="password">密码</param>
        /// <remarks>检验登陆是否成功</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Login/Land")]
        public bool Land(string account,string password)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                Login login = db.Login.Single(l => l.account == account&&l.password==password&&l.isDelete==false);
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
            
        }

        // GET: api/Login/GetAll
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <remarks>获取所有用户</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Login/GetAll")]
        public List<string> GetAll()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Login where a.isDelete == false  select a;
            List<string> list = new List<string>();
            foreach (var x in selectQuery)
            {
                
                list.Add(x.account);
            }
            return list;

        }

        // GET: api/Login/LandOfManager
        /// <summary>
        /// 检验经理登陆
        /// </summary>
        /// <param name="account">账号  手机号码</param>
        /// <param name="password">密码</param>
        /// <remarks>检验经理登陆</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Login/LandOfManager")]
        public bool LandOfManager(string account, string password)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Member join b in db.Login on a.mId equals b.account where b.account == account && a.isDelete == false && b.isDelete == false && a.job.Equals("经理") select b;
            try
            {
                var x = selectQuery.FirstOrDefault();
                if(x is null)
                {
                    return false;
                }
                else
                {
                    if(x.password==password)
                    {
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                return false;
            }
            return false;

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
            login.isDelete = false;
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

        // Put: api/Login/ModifyPassword
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="x">登陆实体</param>
        /// <remarks>修改密码</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Login/ModifyPassword")]
        public bool ModifyPassword(LoginClass x)
        {
            db = new DataClasses1DataContext(connstr);
            var login = db.Login.SingleOrDefault(a => a.account == x.account && a.isDelete == false);
            if(login is null)
            {
                return false;
            }
            else
            {
                login.password = x.password;
                db.SubmitChanges();
                return true;
            }
        }

        // DELETE: api/Login/5
        public void Delete(int id)
        {
        }
    }
}

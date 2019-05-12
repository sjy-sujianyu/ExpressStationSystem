using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class LoginController : ApiController
    {
        // GET: api/Login/Get
        /// <summary>
        /// [NotToken]检验登陆是否成功
        /// </summary>
        /// <param name="account">账号  手机号码</param>
        /// <param name="password">密码</param>
        /// <remarks>检验登陆是否成功</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public bool Land(string account,string password)
        {
            return new Login().Land(account, password);
        }

        // GET: api/Login/GetAll
        /// <summary>
        /// 获取所有用户
        /// </summary>
        /// <remarks>获取所有用户</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<string> GetAll()
        {
            return new Login().GetAll();

        }

        // GET: api/Login/LandOfManager
        /// <summary>
        /// [NotToken]检验经理登陆
        /// </summary>
        /// <param name="account">账号  手机号码</param>
        /// <param name="password">密码</param>
        /// <remarks>检验经理登陆</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public bool LandOfManager(string account, string password)
        {
            return new Login().LandOfManager(account, password);

        }
        // GET: api/Login/Post
        /// <summary>
        /// 插入账户密码角色
        /// </summary>
        /// <param name="a">账户实体信息</param>
        /// <remarks>插入账户密码角色</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Post(LoginClass a)
        {
            return new Login().Post(a);
        }

        // Put: api/Login/ModifyPassword
        /// <summary>
        /// 修改密码
        /// </summary>
        /// <param name="x">登陆实体</param>
        /// <remarks>修改密码</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool ModifyPassword(LoginClass x)
        {
            return new Login().ModifyPassword(x);
        }
    }
}

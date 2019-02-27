﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class ManagerController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Manager
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Manager/5
        public string Get(int id)
        {
            return "value";
        }

        // GET: api/Login/PostMember
        /// <summary>
        /// 新增员工信息
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>新增员工信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Login/PostMember")]
        public bool PostMember(MemberClass x)
        {
            db = new DataClasses1DataContext(connstr);
            var check = db.Member.SingleOrDefault(a => a.mId == x.mId && a.name == x.name);
            var y = db.Login.SingleOrDefault(a => a.account == x.mId);
            if (check !=null&&y!=null)
            {
                check.isDelete = false;
                y.isDelete = false;
                return true;
            }
            Login login = new Login();
            login.account = x.mId;
            login.password = "123";
            Member member = new Member();
            member.mId = x.mId;
            member.job = x.job;
            member.name = x.name;
            member.isDelete = x.isDelete;
            member.imagePath = "无";
            try
            {
                db.Login.InsertOnSubmit(login);
                db.SubmitChanges();
                db.Member.InsertOnSubmit(member);
                db.SubmitChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        // PUT: api/Manager/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/AddressBook/DeleteMember?account={account}
        /// <summary>
        /// 解雇某个员工
        /// </summary>
        /// <param name="account">员工的账号</param>
        /// <remarks>解雇某个员工</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("AddressBook/DeleteMember")]
        public bool DeleteMember(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var x = db.Member.SingleOrDefault(a => a.mId == account);
            var y = db.Login.SingleOrDefault(a => a.account == account);
            if(x is null||y is null)
            {
                return false;
            }
            x.isDelete = true;
            y.isDelete = true;
            db.SubmitChanges();
            return true;
        }
    }
}
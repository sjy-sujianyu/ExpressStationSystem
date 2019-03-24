﻿using System;
using System.Collections.Generic;
using System.Data.SqlClient;
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

        // GET: api/PickUp/GetAllPackage
        /// <summary>
        /// 获取所有包裹记录
        /// </summary>
        /// <remarks>获取所有包裹记录</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Manager/GetAllPackage")]
        public List<int> GetAllPackage(DateTime start, DateTime end)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 orderby a.time descending select a.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }

        // GET: api/PickUp/GetAllMember
        /// <summary>
        /// 获取所有员工
        /// </summary>
        /// <remarks>获取所有员工</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Manager/GetAllMember")]
        public List<string> GetAllMember()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Member  select a.mId;
            List<string> list = new List<string>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            
            return list;
        }
        // GET: api/Manager/PostMember
        /// <summary>
        /// 新增员工信息
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>新增员工信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Manager/PostMember")]
        public bool PostMember(MemberClass x)
        {
            List<string> list = new List<string>() { "派件员", "揽件员", "出件员", "休息中", "经理", "待定中" };
            if(!list.Contains(x.job))
            {
                return false;
            }
            db = new DataClasses1DataContext(connstr);
            var check = db.Member.SingleOrDefault(a => a.mId == x.mId && a.name == x.name);
            var y = db.Login.SingleOrDefault(a => a.account == x.mId);
            if (check !=null&&y!=null)
            {
                check.job = x.job;
                check.baseSalary = x.baseSalary;
                check.imagePath = "无";
                check.onDuty = true;
                check.isDelete = false;
                check.time = DateTime.Now;
                y.isDelete = false;
                db.SubmitChanges();
                return true;
            }
            Login login = new Login();
            login.account = x.mId;
            login.password = "123";
            login.isDelete = false;
            Member member = new Member();
            member.mId = x.mId;
            member.job = x.job;
            member.name = x.name;
            member.isDelete = false;
            member.baseSalary = x.baseSalary;
            member.imagePath = "无";
            member.onDuty = true;
            member.time= DateTime.Now;
            try
            {
                if(y==null)
                {
                    db.Login.InsertOnSubmit(login);
                    db.SubmitChanges();
                }
                db.Member.InsertOnSubmit(member);
                db.SubmitChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        // PUT: api/PickUp/ChangeMemberInfo?account={account}
        /// <summary>
        /// 改变员工职位、名字、底薪
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工职位、名字、底薪
        /// <br>job状态: "派件员","收件员", "出件员","休息中","经理","待定中"</br>
        /// </remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Manager/ChangeMemberInfo")]
        public bool ChangeMemberInfo(MemberClass x)
        {
            db = new DataClasses1DataContext(connstr);
            List<string> list = new List<string>() { "派件员","揽件员", "出件员","休息中","经理","待定中" };
            if(!list.Contains(x.job))
            {
                return false;
            }
            var member = db.Member.SingleOrDefault(a => a.mId == x.mId);
            if(member is null)
            {
                return false;
            }
            else
            {
                if (member.job != x.job)
                {
                    if (!checkDoJob(x.mId))
                    {
                        return false;
                    }
                }
                member.job = x.job;
                member.name = x.name;
                member.baseSalary = x.baseSalary;
                db.SubmitChanges();
                return true;
            }
        }


        // PUT: api/PickUp/ChangeJob
        /// <summary>
        /// 改变员工职位
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工职位
        /// <br>job状态: "派件员","收件员", "出件员","休息中","经理","待定中"</br>
        /// </remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Manager/ChangeJob")]
        public bool ChangeJob(jobClass x)
        {
            if(!checkDoJob(x.mId))
            {
                return false;
            }
            db = new DataClasses1DataContext(connstr);
            List<string> list = new List<string>() { "派件员", "揽件员", "出件员", "休息中", "经理", "待定中" };
            if (!list.Contains(x.job))
            {
                return false;
            }
            var member = db.Member.SingleOrDefault(a => a.mId == x.mId);
            if (member is null)
            {
                return false;
            }
            else
            {
                member.job = x.job;
                db.SubmitChanges();
                return true;
            }
        }
        // PUT: api/PickUp/ChangeDuty
        /// <summary>
        /// 改变员工休息或者上班状态
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工休息或者上班状态</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Manager/ChangeDuty")]
        public bool ChangeDuty(onDutyClass x)
        {
            db = new DataClasses1DataContext(connstr);
            var member = db.Member.SingleOrDefault(a => a.mId == x.mId);
            if (member is null)
            {
                return false;
            }
            else
            {
                member.onDuty = x.onDuty;
                db.SubmitChanges();
                return true;
            }
        }

        // PUT: api/PickUp/ChangeMid?account={account}
        /// <summary>
        /// 改变员工账号
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工账号</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Manager/ChangeMid")]
        public bool ChangeMid(MidChange x)
        {
            SqlConnection conn = new SqlConnection(connstr);
            conn.Open();
            string sql = string.Format("select Login.account from Member join Login on Member.mId=Login.account where Member.isDelete=0 and Member.mId={0}", x.oldMid);
            SqlCommand comm = new SqlCommand(sql, conn);
            if(comm.ExecuteScalar()!=null)
            {
                string update = string.Format("update Login set account={0} where account={1}", x.newMid, x.oldMid);
                SqlCommand updateComm = new SqlCommand(update, conn);
                try
                {
                    int n = updateComm.ExecuteNonQuery();
                    if (n != 0)
                    {
                        conn.Close();
                        return true;
                    }
                    else
                    {
                        conn.Close();
                        return false;
                    }
                }
                catch(Exception)
                {
                    return false;
                }
            }
            conn.Close();
            return false;
        }
        // DELETE: api/Manager/DeleteMember?account={account}
        /// <summary>
        /// 解雇某个员工
        /// </summary>
        /// <param name="aclass">账户实体信息</param>
        /// <remarks>解雇某个员工</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("Manager/DeleteMember")]
        public bool DeleteMember(accountClass aclass)
        {
            if (!checkDoJob(aclass.account))
            {
                return false;
            }
            db = new DataClasses1DataContext(connstr);
            var x = db.Member.SingleOrDefault(a => a.mId == aclass.account);
            if(x is null)
            {
                return false;
            }
            x.isDelete = true;
            x.onDuty = false;
            x.time = DateTime.Now;
            db.SubmitChanges();
            return true;
        }
        // PUT: api/Manager/RevokeDeleteMember?account={account}
        /// <summary>
        /// 撤销解雇某个员工
        /// </summary>
        /// <param name="aclass">账户实体信息</param>
        /// <remarks>撤销解雇某个员工</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("Manager/RevokeDeleteMember")]
        public bool RevokeDeleteMember(accountClass aclass)
        {
            db = new DataClasses1DataContext(connstr);
            var x = db.Member.SingleOrDefault(a => a.mId == aclass.account);
            if (x is null)
            {
                return false;
            }
            x.isDelete = false;
            x.onDuty = true;
            db.SubmitChanges();
            return true;
        }
        private bool checkDoJob(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var pickUpQuery = from a in db.PickUp orderby a.time descending join b in db.Package on a.id equals b.id where b.status == "待揽件"  group a by a.id into g select g.First();
            foreach(var x in pickUpQuery)
            {
                if(x.mId==account&&x.isDone==false)
                {
                    return false;
                }
            }
            var delieryQuery= from a in db.Delivery orderby a.time descending join b in db.Package on a.id equals b.id where b.status == "待揽件"  group a by a.id into g select g.First();
            foreach (var x in delieryQuery)
            {
                if (x.mId == account && x.isDone == false)
                {
                    return false;
                }
            }
            return true;
        }
    }
}

using System;
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

        public string Options()
        {
            return null;
        }
        // GET: api/Manager
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/PickUp/GetReadytoReceive
        /// <summary>
        /// 获取所有包裹记录
        /// </summary>
        /// <remarks>获取所有包裹记录</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Manager/GetAllPackage")]
        public List<int> GetAllPackage()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Package select a.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }

        // GET: api/PickUp/GetAllMemberOnDuty
        /// <summary>
        /// 获取所有在职员工
        /// </summary>
        /// <remarks>获取所有在职员工</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Manager/GetAllMemberOnDuty")]
        public List<string> GetAllMemberOnDuty()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Member where a.isDelete==false select a.mId;
            List<string> list = new List<string>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }

        // GET: api/PickUp/GetAllFiredMember
        /// <summary>
        /// 获取所有被解雇员工
        /// </summary>
        /// <remarks>获取所有被解雇员工</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Manager/GetAllFiredMember")]
        public List<string> GetAllFiredMember()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from a in db.Member where a.isDelete == true select a.mId;
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
            login.isDelete = false;
            Member member = new Member();
            member.mId = x.mId;
            member.job = x.job;
            member.name = x.name;
            member.isDelete = false;
            member.baseSalary = x.baseSalary;
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

        // PUT: api/PickUp/ChangeJob?account={account}
        /// <summary>
        /// 改变员工职位
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工职位</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Manager/ChangeJob")]
        public bool ChangeJob(MemberClass x)
        {
            db = new DataClasses1DataContext(connstr);
            List<string> list = new List<string>() { "派件员","收件员", "出件员","休息中","经理" };
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
                member.job = x.job;
                db.SubmitChanges();
                return true;
            }
        }

        // PUT: api/PickUp/ChangeJob?account={account}
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
        /// <param name="account">员工的账号</param>
        /// <remarks>解雇某个员工</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("Manager/DeleteMember")]
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

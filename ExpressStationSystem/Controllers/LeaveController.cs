 using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class LeaveController : ApiController
    {
        //请假状态  0:初始 1:被拒绝 2:申请成功 3:已销假
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Leave/GetLeaveList
        /// <summary>
        /// 获取员工请假申请列表
        /// </summary>
        /// <remarks>获取员工请假申请列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Leave/GetLeaveList")]
        public List<int> GetLeaveList()
        {
            db = new DataClasses1DataContext(connstr);
            List<int> list = new List<int>();
            var leave = db.Leave.Where(a => a.status == 0);
            foreach(var x in leave)
            {
                list.Add(x.lId);
            }
            return list;
        }

        // GET: api/Leave/GetLeaveListByStatus
        /// <summary>
        /// 获取请假信息
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <param name="status">请假状态  0:审核中 1:被拒绝 2:申请成功 3:已销假</param>
        /// <remarks>获取请假信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Leave/GetLeaveListByStatus")]
        public List<int> GetLeaveListByStatus(DateTime start, DateTime end,short status)
        {
            db = new DataClasses1DataContext(connstr);
            List<int> list = new List<int>();
            var leave = db.Leave.Where(a => DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0&&a.status==status);
            if (leave is null)
            {
                return list;
            }
            else
            {
                foreach (var x in leave)
                {
                    list.Add(x.lId);
                }
            }
            return list;
        }
        // GET: api/Leave/GetLeaveInfo
        /// <summary>
        /// 获取请假信息
        /// </summary>
        /// <param name="lId">请假项目id</param>
        /// <remarks>获取请假信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Leave/GetLeaveInfo")]
        public dynamic GetLeaveInfo(int lId)
        {
            db = new DataClasses1DataContext(connstr);
            var leave = db.Leave.Join(db.Member,a=>a.mId,b=>b.mId,(a,b)=>new { leave=a,member=b}).SingleOrDefault(a => a.leave.lId==lId);
            if(leave is null)
            {
                return null;
            }
            else
            {
                return leave;
            }
        }

        // GET: api/Leave/GetLeaveCount
        /// <summary>
        /// 获取员工请假次数
        /// </summary>
        /// <param name="account">员工账号</param>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <remarks>获取员工请假次数</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Leave/GetLeaveCount")]
        public int GetLeaveCount(string account,DateTime start,DateTime end)
        {
            db = new DataClasses1DataContext(connstr);
            var leaveCount = db.Leave.Where(a => a.mId == account && DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0&&a.status==2).Count();
            return leaveCount;
        }
        // GET: api/Leave/GetLeaveInfoByAccountBystatus
        /// <summary>
        /// 获取请假信息
        /// </summary>
        /// <param name="account">员工账号</param>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <param name="status">请假状态  0:审核中 1:被拒绝 2:申请成功 3:已销假</param>
        /// <remarks>获取请假信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Leave/GetLeaveInfoByAccountBystatus")]
        public List<int> GetLeaveInfoByAccountBystatus(string account,DateTime start,DateTime end,short status)
        {
            db = new DataClasses1DataContext(connstr);
            List<int> list = new List<int>();
            var leave = db.Leave.Where(a => a.mId==account&&DateTime.Compare(a.time,start)>=0&&DateTime.Compare(a.time,end)<=0&&a.status==status);
            if (leave is null)
            {
                return list;
            }
            else
            {
                foreach(var x in leave)
                {
                    list.Add(x.lId);
                }
            }
            return list;
        }
        // POST: api/Leave/Post
        /// <summary>
        /// 添加员工请假信息
        /// </summary>
        /// <param name="x">请假实体</param>
        /// <remarks>添加员工请假信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Leave/Post")]
        public bool Post(LeaveClass x)
        {
            db = new DataClasses1DataContext(connstr);
            Leave leave = new Leave();
            leave.mId = x.mId;
            leave.reason = x.reason;
            leave.status = 0;
            leave.time = DateTime.Now;
            leave.srcTime = x.srcTime;
            leave.endTime = x.endTime;
            try
            {
                db.Leave.InsertOnSubmit(leave);
                db.SubmitChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        // PUT: api/Leave/Deal
        /// <summary>
        /// 处理请假
        /// </summary>
        /// <param name="x">请假项目实体</param>
        /// <remarks>处理请假</remarks>
        /// 
        /// <returns>返回</returns>
        [HttpPut, Route("Leave/Deal")]
        public bool Deal(ConfirmLeaveClass x)
        {
            db = new DataClasses1DataContext(connstr);
            var leave = db.Leave.SingleOrDefault(a => a.lId == x.lId);
            if(leave is null)
            {
                return false;
            }
            else if(leave.status!=0)
            {
                return false;
            }
            else
            {
                leave.status = x.isDone ? (short)2 : (short)1;
                leave.time = DateTime.Now;
                leave.view = x.view;
                leave.person=x.person;
                db.SubmitChanges();
                if(leave.status==2)
                {
                    var member = db.Member.SingleOrDefault(a => a.mId == x.mId);
                    if(member is null)
                    {
                        return false;
                    }
                    else
                    {
                        member.onDuty = false;
                        db.SubmitChanges();
                    }
                }
                return true;
            }
        }


        // GET: api/Leave/UpdateLeave
        /// <summary>
        /// 更新请假申请
        /// </summary>
        /// <param name="x">请假信息实体</param>
        /// <remarks>更新请假申请</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Leave/UpdateLeave")]
        public bool UpdateLeave(UpdateLeaveClass x)
        {
            db = new DataClasses1DataContext(connstr);
            Leave leave = db.Leave.SingleOrDefault(a => a.lId == x.lId);
            if(leave is null)
            {
                return false;
            }
            if(leave.status!=1)
            {
                return false;
            }
            leave.reason = x.reason;
            leave.status = 0;
            leave.mId = x.mId;
            leave.time = DateTime.Now;
            leave.srcTime = x.srcTime;
            leave.endTime = x.endTime;
            try
            {
                db.Leave.InsertOnSubmit(leave);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // PUT: api/Leave/ComeBack
        /// <summary>
        /// 销假
        /// </summary>
        /// <param name="x">请假项目id</param>
        /// <remarks>销假</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Leave/ComeBack")]
        public bool ComeBack(lIdClass x)
        {
            db = new DataClasses1DataContext(connstr);
            var leave = db.Leave.SingleOrDefault(a => a.lId == x.lId);
            if(leave is null)
            {
                return false;
            }
            else if(leave.status!=2)
            {
                return false;
            }
            else
            {
                //罚款
                if(DateTime.Compare(leave.time,leave.endTime)>0)
                {
                    TimeSpan ts = leave.time - leave.endTime;
                    int fine = ts.Hours * 5;
                    Money money = new Money();
                    money.mId = x.mId;
                    money.subsidy = 0;
                    money.fine = fine;
                    money.time = DateTime.Now;
                    money.person = "系统";
                    money.reason = "请假逾期，未按时上班";
                    try
                    {
                        db.Money.InsertOnSubmit(money);
                        db.SubmitChanges();
                        return true;
                    }
                    catch(Exception)
                    {
                        return false;
                    }
                }
                leave.status = 3;
                leave.time = DateTime.Now;
                db.SubmitChanges();
                var member = db.Member.SingleOrDefault(a => a.mId == x.mId);
                if (member is null)
                {
                    return false;
                }
                else
                {
                    member.onDuty = true;
                    db.SubmitChanges();
                }
                return true;
            }
        }
        // GET: api/Leave/Delete
        /// <summary>
        /// 删除未批准的请假记录
        /// </summary>
        /// <param name="x">请假项目id</param>
        /// <remarks>删除未批准的请假记录</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("Leave/Delete")]
        public bool Delete(lIdClass x)
        {
            db = new DataClasses1DataContext(connstr);
            var leave = db.Leave.SingleOrDefault(a => a.lId == x.lId);
            if (leave is null)
            {
                return false;
            }  
            else if(leave.status!=1)
            {
                return false;
            }
            else
            {
                try
                {
                    db.Leave.DeleteOnSubmit(leave);
                    db.SubmitChanges();
                    return true;
                }
                catch(Exception)
                {
                    return false;
                }
                
            }
        }
    }
}

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
        // GET: api/Leave/Get
        /// <summary>
        /// 获取员工请假申请列表
        /// </summary>
        /// <remarks>获取员工请假申请列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Leave/GetLeaveList")]
        public List<dynamic> GetLeaveList()
        {
            db = new DataClasses1DataContext(connstr);
            List<dynamic> list = new List<dynamic>();
            var leave = db.Leave.Where(a => a.status == 0).ToList();
            foreach(var x in leave)
            {
                list.Add(x);
            }
            return list;
        }

        // PUT: api/Leave/Post
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
                leave.status = x.isDone? (short)1 : (short)2;
                leave.time = DateTime.Now;
                leave.view = x.view;
                leave.person=x.person;
                db.SubmitChanges();
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

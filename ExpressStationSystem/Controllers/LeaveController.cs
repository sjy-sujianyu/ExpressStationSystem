using ExpressStationSystem.Models;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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

        // GET: api/Leave/GetLeaveList
        /// <summary>
        /// 获取员工请假申请列表
        /// </summary>
        /// <remarks>获取员工请假申请列表</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<int> GetLeaveList()
        {
            return new Leave().GetLeaveList();
        }

        // GET: api/Leave/GetHistoryLeave
        /// <summary>
        /// 获取请假信息
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <remarks>获取请假信息</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<int> GetHistoryLeave(DateTime start, DateTime end)
        {
            return new Leave().GetHistoryLeave(start, end);
        }
        // GET: api/Leave/GetLeaveInfo
        /// <summary>
        /// 获取请假信息
        /// </summary>
        /// <param name="lId">请假项目id</param>
        /// <remarks>获取请假信息</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetLeaveInfo(int lId)
        {
            return new Leave().GetLeaveInfo(lId);
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
        [HttpGet]
        public int GetLeaveCount(string account,DateTime start,DateTime end)
        {
            return new Leave().GetLeaveCount(account, start, end);
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
        [HttpGet]
        public List<int> GetLeaveInfoByAccountBystatus(string account,DateTime start,DateTime end,short status)
        {
            return new Leave().GetLeaveInfoByAccountBystatus(account, start, end, status);
        }
        // POST: api/Leave/Post
        /// <summary>
        /// 添加员工请假信息
        /// </summary>
        /// <param name="x">请假实体</param>
        /// <remarks>添加员工请假信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Post(LeaveClass x)
        {
            return new Leave().Post(x);
        }

        // PUT: api/Leave/Deal
        /// <summary>
        /// 处理请假
        /// </summary>
        /// <param name="x">请假项目实体</param>
        /// <remarks>处理请假</remarks>
        /// 
        /// <returns>返回</returns>
        [HttpPut]
        public bool Deal(ConfirmLeaveClass x)
        {
            return new Leave().Deal(x);
        }


        // GET: api/Leave/UpdateLeave
        /// <summary>
        /// 更新请假申请
        /// </summary>
        /// <param name="x">请假信息实体</param>
        /// <remarks>更新请假申请</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool UpdateLeave(UpdateLeaveClass x)
        {
            return new Leave().UpdateLeave(x);
        }

        // GET: api/Leave/UpdateLeave
        /// <summary>
        /// 撤销被拒绝或者成功状态的请假项目
        /// </summary>
        /// <param name="x">请假信息实体</param>
        /// <remarks>撤销被拒绝或者成功状态的请假项目</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool RevokeLeave(lIdClass1 x)
        {
            return new Leave().RevokeLeave(x);
        }
        // PUT: api/Leave/ComeBack
        /// <summary>
        /// 销假
        /// </summary>
        /// <param name="x">请假项目id</param>
        /// <remarks>销假</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool ComeBack(lIdClass x)
        {
            return new Leave().ComeBack(x);
        }
        // GET: api/Leave/Delete
        /// <summary>
        /// 删除未批准的请假记录
        /// </summary>
        /// <param name="x">请假项目id</param>
        /// <remarks>删除未批准的请假记录</remarks>
        /// <returns>返回</returns>
        [HttpDelete]
        public bool Delete(lIdClass1 x)
        {
            return new Leave().Delete(x);
        }
    }
}

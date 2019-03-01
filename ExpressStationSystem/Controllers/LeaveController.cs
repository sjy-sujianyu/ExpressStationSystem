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
            return null;
        }

        // PUT: api/Leave/Post
        /// <summary>
        /// 添加员工请假信息
        /// </summary>
        /// <param name="x">请假实体</param>
        /// <remarks>添加员工请假信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Leave/Post")]
        public dynamic Post(dynamic x)
        {
            return new { code = true, message = "" };
        }

        // PUT: api/Leave/Confirm
        /// <summary>
        /// 处理请假
        /// </summary>
        /// <param name="x">请假项目实体</param>
        /// <remarks>处理请假</remarks>
        /// 
        /// <returns>返回</returns>
        [HttpPut, Route("Leave/Deal")]
        public void Deal(dynamic x)
        {
        }


        // GET: api/Leave/UpdateLeave
        /// <summary>
        /// 更新请假申请
        /// </summary>
        /// <param name="x">请假信息实体</param>
        /// <remarks>更新请假申请</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Leave/UpdateLeave")]
        public void UpdateLeave(dynamic x)
        {
        }

        // PUT: api/Leave/ComeBack
        /// <summary>
        /// 销假
        /// </summary>
        /// <param name="x">请假项目id</param>
        /// <remarks>销假</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Leave/ComeBack")]
        public void ComeBack(dynamic x)
        {
        }
        // GET: api/Leave/Delete
        /// <summary>
        /// 删除未批准的请假记录
        /// </summary>
        /// <param name="x">请假项目id</param>
        /// <remarks>删除未批准的请假记录</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("Leave/Delete")]
        public void Delete(dynamic x)
        {
        }
    }
}

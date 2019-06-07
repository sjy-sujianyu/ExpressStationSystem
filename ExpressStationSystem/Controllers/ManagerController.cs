using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class ManagerController : ApiController
    {

        // GET: api/PickUp/GetAllPackage
        /// <summary>
        /// 获取所有包裹记录
        /// </summary>
        /// <remarks>获取所有包裹记录</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<int> GetAllPackage(DateTime start, DateTime end)
        {
            return new Package().GetAllPackage(start, end);
        }

        // GET: api/PickUp/GetAllMember
        /// <summary>
        /// 获取所有员工
        /// </summary>
        /// <remarks>获取所有员工</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<string> GetAllMember()
        {
            return new Member().GetAllMember();
        }
        // GET: api/Manager/PostMember
        /// <summary>
        /// 新增员工信息
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>新增员工信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool PostMember(MemberClass x)
        {
            return new Member().PostMember(x);
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
        [HttpPut]
        public bool ChangeMemberInfo(MemberClass x)
        {
            return new Member().ChangeMemberInfo(x);
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
        [HttpPut]
        public bool ChangeJob(jobClass x)
        {
            return new Member().ChangeJob(x);
        }
        // PUT: api/PickUp/ChangeDuty
        /// <summary>
        /// 改变员工休息或者上班状态
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工休息或者上班状态</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool ChangeDuty(onDutyClass x)
        {
            return new Member().ChangeDuty(x);
        }

        // PUT: api/PickUp/ChangeMid?account={account}
        /// <summary>
        /// 改变员工账号
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工账号</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool ChangeMid(MidChange x)
        {
            return new Member().ChangeMid(x);
        }
        // DELETE: api/Manager/DeleteMember?account={account}
        /// <summary>
        /// 解雇某个员工
        /// </summary>
        /// <param name="aclass">账户实体信息</param>
        /// <remarks>解雇某个员工</remarks>
        /// <returns>返回</returns>
        [HttpDelete]
        public bool DeleteMember(accountClass aclass)
        {
            return new Member().DeleteMember(aclass);
        }
        // PUT: api/Manager/RevokeDeleteMember?account={account}
        /// <summary>
        /// 撤销解雇某个员工
        /// </summary>
        /// <param name="aclass">账户实体信息</param>
        /// <remarks>撤销解雇某个员工</remarks>
        /// <returns>返回</returns>
        [HttpDelete]
        public bool RevokeDeleteMember(accountClass aclass)
        {
            return new Member().RevokeDeleteMember(aclass);
        }
        // POST: api/Leave/GetHistoryLeave
        /// <summary>
        /// 发布经理公告给员工小程序，主题是“经理公告”
        /// </summary>
        /// <param name="x">x</param>
        /// <remarks>获取请假信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool PublishMessage(mqttMessage x)
        {
            return Global.PublishMessage(x);
        }
    }
}

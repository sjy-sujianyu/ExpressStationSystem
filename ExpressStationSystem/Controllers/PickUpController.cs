using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class PickUpController : ApiController
    {

        // GET: api/PickUp/GetReadytoReceiveByCondition
        /// <summary>
        /// 按条件查询要揽件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <remarks>按条件查询要揽件的包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetReadytoReceiveByCondition(string str,string type,int page,int pageSize)
        {
            return new Package().GetReadytoReceiveByCondition(str, type, page, pageSize);
        }

        // GET: api/PickUp/GetReceivingByCondition
        /// <summary>
        /// 按条件查询要揽件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <param name="account">员工的mId</param>
        /// <remarks>按条件查询要揽件的包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetReceivingByCondition(string account,string str, string type,int page,int pageSize)
        {
            return new Package().GetReceivingByCondition(account, str, type, page, pageSize);
        }
        // GET: api/PickUp/GetReadytoScan
        /// <summary>
        /// 获取上个网点转来待扫件的包裹ID
        /// </summary>
        /// <remarks>获取上个网点转来待扫件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetReadytoScan(int page,int pageSize)
        {
            return new Package().GetReadytoScan(page, pageSize);
        }

        // GET: api/PickUp/GetReadytoReceive
        /// <summary>
        /// 获取已下单,要揽件的包裹ID
        /// </summary>
        /// <remarks>获取待揽件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetReadytoReceive(int page,int pageSize)
        {
            return new Package().GetReadytoReceive(page, pageSize);
        }

        // GET: api/PickUp/GetReceiving
        /// <summary>
        /// 获取某员工正在揽件的包裹ID
        /// </summary>
        /// <remarks>获取某员工正在揽件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetReceiving(string account,int page, int pageSize)
        {
            return new Package().GetReceiving(account, page, pageSize);
        }
        // Post: api/PickUp/Post
        /// <summary>
        /// 添加待揽件包裹信息
        /// </summary>
        /// <remarks>添加待揽件包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Post(PickUpClass x)
        {
            return new Package().Post(x);
        }
        // Post: api/PickUp/ConfirmPost
        /// <summary>
        /// 添加确认揽件包裹信息
        /// </summary>
        /// <remarks>添加确认揽件包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool ConfirmPost(PickUpClass x)
        {
            return new Package().ConfirmPost(x);
        }
        // PUT: api/PickUp/RevokeReceive
        /// <summary>
        /// 包裹的状态从待揽件撤销为已下单
        /// </summary>
        /// <remarks>包裹的状态从待揽件撤销为已下单</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool RevokeReceive(IdClass iclass)
        {
            return new Package().RevokeReceive(iclass);
        }


        // PUT: api/PickUp/Scan?id={id}
        /// <summary>
        /// 包裹的状态从运输中状态变为已扫件
        /// </summary>
        /// <remarks>包裹的状态从运输中状态变为已扫件</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool Scan(IdClass iclass)
        {
            return new Package().Scan(iclass);
        }
        
    }
}

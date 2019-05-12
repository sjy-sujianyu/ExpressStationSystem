using ExpressStationSystem.Controllers;
using ExpressStationSystem.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem
{

    public class DeliveryController : ApiController
    {

        // GET: api/Delivery/GetReadytoDeliveryByCondition
        /// <summary>
        /// 按条件查询要派件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <remarks>按条件查询要派件的包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetReadytoDeliveryByCondition(string str, string type,int page,int pageSize)
        {
            return new Delivery().GetReadytoDeliveryByCondition(str, type, page, pageSize);
        }

        // GET: api/Delivery/GetDeliveringByCondition
        /// <summary>
        /// 按条件查询正在派件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <param name="account">员工的mId</param>
        /// <remarks>按条件查询正在派件的包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<int> GetDeliveringByCondition(string account, string str, string type,int page,int pageSize)
        {
            return new Delivery().GetDeliveringByCondition(account, str, type, page, pageSize);
        }



        // GET: api/Delivery/GetReadytoDelivery
        /// <summary>
        /// 获取已扫件,要派件的包裹ID
        /// </summary>
        /// <remarks>获取待派件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetReadytoDelivery(int page,int pageSize)
        {
            return new Delivery().GetReadytoDelivery(page, pageSize);
        }

        // Post: api/Delivery/Post
        /// <summary>
        /// 添加派件中包裹信息
        /// </summary>
        /// <remarks>添加派件中包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Post(DeliveryClass x)
        {
            return new Delivery().Post(x);
        }

        // GET: api/Delivery/IsRefuse
        /// <summary>
        /// 查看包裹是否为退件包裹
        /// </summary>
        /// <param name="id">包裹id</param>
        /// <remarks>查看包裹是否为退件包裹</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public bool IsRefuse(int id)
        {
            return new Delivery().IsRefuse(id);
        }

        // PUT: api/Delivery/RevokeDelivery
        /// <summary>
        /// 包裹的状态从派中件撤销为已扫件
        /// </summary>
        /// <remarks>包裹的状态从派件中撤销为已扫件</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool RevokeDelivery(IdClass iclass)
        {
            return new Delivery().RevokeDelivery(iclass);
        }

        // PUT: api/Delivery/SwapAddress
        /// <summary>
        /// 交换发件人和收件人地址
        /// </summary>
        /// <remarks>交换发件人和收件人地址</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool SwapAddress(IdClass iclass)
        {
            return new Delivery().SwapAddress(iclass);
        }

        //PUT: api/Delivery/Refuse
        /// <summary>
        /// 包裹拒签
        /// </summary>
        /// <remarks>包裹拒签，状态从派件中撤销为已扫件</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool Refuse(IdClass iclass)
        {
            return new Delivery().Refuse(iclass);
        }

        // GET: api/Delivery/GetDelivering
        /// <summary>
        /// 获取某员工正在派件的包裹ID
        /// </summary>
        /// <remarks>获取某员工正在派件的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetDelivering(string account,int page,int pageSize)
        {
            return new Delivery().GetDelivering(account, page, pageSize);
        }

        // Post: api/Delivery/ConfirmPost
        /// <summary>
        /// 添加已签收包裹信息
        /// </summary>
        /// <remarks>添加已签收包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool ConfirmPost(DeliveryClass x)
        {
            return new Delivery().ConfirmPost(x);
        }

    }
}
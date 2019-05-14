using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Xml;
using ExpressStationSystem.Models;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{

    public class TransferController : ApiController
    {

        // GET: api/Transfer/GetReadyToTransfer
        /// <summary>
        /// 获取已扫件,要出站的包裹ID
        /// </summary>
        /// <remarks>获取待出站的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetReadyToTransfer(int page,int pageSize)
        {
            return new Transfer().GetReadyToTransfer(page, pageSize);
        }

        // GET: api/Transfer/GetNext
        /// <summary>
        /// 获取包裹下一站去哪
        /// </summary>
        /// <remarks>获取包裹下一站去哪</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public string GetNext(int id)
        {
            return new Transfer().GetNext(id);
        }

        // Post: api/Transfer/Post
        /// <summary>
        /// 添加出站包裹信息
        /// </summary>
        /// <param name="x">出站包裹信息</param>
        /// <remarks>添加出站包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Post(TransferClass x)
        {
            return new Transfer().Post(x);
        }



        // PUT: api/Transfer/Departure
        /// <summary>
        /// 交通工具从站点出发
        /// </summary>
        /// <param name="x">交通工具id</param>
        /// <remarks>交通工具从站点出发</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool Departure(IdClass x)
        {
            return new Transfer().Departure(x);
        }

        // PUT: api/Transfer/RevokeTransfer
        /// <summary>
        /// 把包裹从车上卸下
        /// </summary>
        /// <param name="x">包裹id</param>
        /// <remarks>把包裹从车上卸下</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool RevokeTransfer(IdClass x)
        {
            return new Transfer().RevokeTransfer(x);
        }





    }


}

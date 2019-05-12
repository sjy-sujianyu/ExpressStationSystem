using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class ImageController : ApiController
    {
        // GET: api/Query/GetImageByAccount?account={account}
        /// <summary>
        /// 返回图片路径
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回图片路径</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public string Get(string account)
        {
            return new Member().Get(account);
        }

        // GET: api/Image/Post
        /// <summary>
        /// [upload]上传文件
        /// </summary>
        /// <remarks>[upload]上传文件</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Post(string account)
        {
            return new Member().Post(account);
        } 
    }
}

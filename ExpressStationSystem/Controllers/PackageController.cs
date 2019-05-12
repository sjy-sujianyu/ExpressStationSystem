using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class PackageController : ApiController
    {

        // GET: api/Package/GetValue
        /// <summary>
        /// 根据起始地点、目的地点和重量计算包裹价格
        /// </summary>
        /// <param name="srcProvince">源省份</param>
        /// <param name="destProvince">目的省份 </param>
        /// <param name="weight">重量kg  1.0</param>
        /// <remarks>根据起始地点、目的地点和重量计算包裹价格</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public double GetValue(string srcProvince,string destProvince,double weight)
        {
            return new Package().GetValue(srcProvince, destProvince, weight);
        }

        // POST: api/Package/Post
        /// <summary>
        /// 插入包裹记录
        /// </summary>
        /// <param name="packageclass">包裹对象 vId字段可选 Remarks字段可选</param>
        /// <remarks>插入包裹记录</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Post(PackageClass packageclass)
        {
            return new Package().Post(packageclass);
        }

        // DELETE: api/Package/Delete?id={id}
        /// <summary>
        /// 删除指定ID的包裹信息
        /// </summary>
        /// <param name="id">包裹ID</param>
        /// <remarks>删除指定ID的包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpDelete]
        public bool Delete(int id)
        {
            return new Package().Delete(id);
        }
    }
}

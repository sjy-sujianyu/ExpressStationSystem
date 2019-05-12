using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class VehicleController : ApiController
    {

        // GET: api/Vehicle/GetPackageOnVehicle
        /// <summary>
        /// 获取已上车的包裹ID
        /// </summary>
        /// <param name="vId">交通工具id</param>
        /// <remarks>获取已上车的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetPackageOnVehicle(int vId)
        {
            return new Vehicle().GetPackageOnVehicle(vId);
        }
        // GET: api/Vehicle/GetAllVehicle
        /// <summary>
        /// 返回所有车辆信息(被占用的，空闲的，被删除的)
        /// </summary>
        /// <remarks>返回所有车辆信息</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<dynamic> GetAllVehicle()
        {
            return new Vehicle().GetAllVehicle();
        }
        // GET: api/Vehicle/GetAllVehicleOnDuty
        /// <summary>
        /// 返回空闲车辆信息
        /// </summary>
        /// <remarks>返回空闲车辆信息</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<dynamic> GetAllVehicleOnDuty()
        {
            return new Vehicle().GetAllVehicleOnDuty();
        }

        // POST: api/Vehicle/Post
        /// <summary>
        /// 插入车辆信息
        /// </summary>
        /// <param name="x">车辆实体信息</param>
        /// <remarks>插入车辆信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Post(VehicleClass x)
        {
            return new Vehicle().Post(x);
        }

        // Put api/Vehicle/ChangeStatus
        /// <summary>
        /// 更新车辆是否被占用的状态
        /// </summary>
        /// <param name="x">车辆实体信息</param>
        /// <remarks>更新车辆是否被占用的状态</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool ChangeStatus(VehicleStatus x)
        {
            return new Vehicle().ChangeStatus(x);
        }

        // DELETE api/Vehicle/Delete?id={id}
        /// <summary>
        /// 删除车辆信息
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <remarks>删除车辆信息</remarks>
        /// <returns>返回</returns>
        [HttpDelete]
        public bool Delete(int id)
        {
            return new Vehicle().Delete(id);
        }
    }
}

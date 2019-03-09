using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class VehicleController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Vehicle/GetVehicleOnDuty
        /// <summary>
        /// 返回空闲车辆信息
        /// </summary>
        /// <remarks>返回空闲车辆信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Vehicle/GetAllVehicle")]
        public List<dynamic> GetAllVehicle()
        {
            db = new DataClasses1DataContext(connstr);
            List<dynamic> list = new List<dynamic>();
            var vehicle = db.Vehicle.Where(a => a.isDelete == false);
            foreach(var x in vehicle)
            {
                list.Add(x);
            }
            return list;
        }

        // POST: api/Vehicle/Post
        /// <summary>
        /// 插入车辆信息
        /// </summary>
        /// <param name="x">车辆实体信息</param>
        /// <remarks>插入车辆信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Vehicle/Post")]
        public bool Post(VehicleClass x)
        {
            db = new DataClasses1DataContext(connstr);
            var y = db.Vehicle.SingleOrDefault(a => a.plateNumber == x.plateNumber);
            if(y!=null)
            {
                y.isDelete = false;
                db.SubmitChanges();
                return true;
            }
            else
            {
                Vehicle vehicle = new Vehicle();
                vehicle.type = x.type;
                vehicle.plateNumber = x.plateNumber;
                vehicle.isDelete = false;
                vehicle.onDuty = false;
                try
                {
                    db.Vehicle.InsertOnSubmit(vehicle);
                    db.SubmitChanges();
                    return true;
                }
                catch(Exception)
                {
                    return false;
                }
            }
        }

        // Put api/Vehicle/ChangeStatus
        /// <summary>
        /// 更新车辆是否被占用的状态
        /// </summary>
        /// <param name="x">车辆实体信息</param>
        /// <remarks>更新车辆是否被占用的状态</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Vehicle/ChangeStatus")]
        public bool ChangeStatus(VehicleStatus x)
        {
            db = new DataClasses1DataContext(connstr);
            var vehicle = db.Vehicle.SingleOrDefault(a => a.vId == x.vId&&a.isDelete==false);
            if(vehicle is null)
            {
                return false;
            }
            else
            {
                vehicle.onDuty = x.onDuty;
                db.SubmitChanges();
                return true;
            }
        }

        // DELETE api/Vehicle/Delete?id={id}
        /// <summary>
        /// 删除车辆信息
        /// </summary>
        /// <param name="id">车辆id</param>
        /// <remarks>删除车辆信息</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("Vehicle/Delete")]
        public bool Delete(int id)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                var vehicle = db.Vehicle.SingleOrDefault(a => a.vId == id);
                if(vehicle is null)
                {
                    return false;
                }
                else
                {
                    vehicle.isDelete = true;
                    db.SubmitChanges();
                    return true;
                }
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}

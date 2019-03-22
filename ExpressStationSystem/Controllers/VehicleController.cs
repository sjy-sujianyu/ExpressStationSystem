using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class VehicleController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;

        // GET: api/Vehicle/GetPackageOnVehicle
        /// <summary>
        /// 获取已上车的包裹ID
        /// </summary>
        /// <param name="vId">交通工具id</param>
        /// <remarks>获取已上车的包裹ID</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Vehicle/GetPackageOnVehicle")]
        public dynamic GetPackageOnVehicle(int vId)
        {
            List<dynamic> list = new List<dynamic>();
            db = new DataClasses1DataContext(connstr);
            var package = db.Package
                .Join(db.Transfer.Where(a => a.vId == vId), a => a.id, b => b.id, (a, b) => new { a = a })
                .Join(db.AddressBook, a => a.a.sendId, b => b.aId, (a, b) => new { a = a.a, b = b })
                .Join(db.AddressBook, a => a.a.receiverId, b => b.aId, (a, b) => new { package = a.a, src = a.b, dest = b });

            var paclist = new List<dynamic>();
            foreach (var x in package)
            {
                var path = db.Path.OrderByDescending(a => a.time).Where(a => a.id == x.package.id).FirstOrDefault();
                if (path != null && path.isArrival == false && path.curPlace.Contains("华南农业大学"))
                {
                    paclist.Add(x);
                }
            }
            var tasks = new List<Task<dynamic>>();
            List<dynamic> errorList = new List<dynamic>();
            foreach (var x in paclist)
            {
                var y = new Task<dynamic>(() => new QueryController().getError(x.package.id));
                y.Start();
                tasks.Add(y);
            }
            Task.WaitAll(tasks.ToArray());
            foreach (var x in tasks)
            {
                errorList.Add(x.Result);
            }
            int i = 0;
            foreach (var x in paclist)
            {
                var result = new { package = x.package, src = x.src, dest = x.dest, error = errorList[i] };
                list.Add(result);
                i++;
            }
            return list;
        }
        // GET: api/Vehicle/GetAllVehicle
        /// <summary>
        /// 返回所有车辆信息(被占用的，空闲的，被删除的)
        /// </summary>
        /// <remarks>返回所有车辆信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Vehicle/GetAllVehicle")]
        public List<dynamic> GetAllVehicle()
        {
            db = new DataClasses1DataContext(connstr);
            List<dynamic> list = new List<dynamic>();
            var vehicle = from a in db.Vehicle select a;
            foreach(var x in vehicle)
            {
                list.Add(x);
            }
            return list;
        }
        // GET: api/Vehicle/GetAllVehicleOnDuty
        /// <summary>
        /// 返回空闲车辆信息
        /// </summary>
        /// <remarks>返回空闲车辆信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Vehicle/GetAllVehicleOnDuty")]
        public List<dynamic> GetAllVehicleOnDuty()
        {
            db = new DataClasses1DataContext(connstr);
            List<dynamic> list = new List<dynamic>();
            var vehicle = from a in db.Vehicle where a.isDelete == false&&a.onDuty==false select a;
            foreach (var x in vehicle)
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
                vehicle.time = DateTime.Now;
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
                    vehicle.onDuty = false;
                    vehicle.time = DateTime.Now;
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

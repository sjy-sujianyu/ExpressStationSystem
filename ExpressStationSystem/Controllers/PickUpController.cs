using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class PickUpController : ApiController
    {
        //private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        //private DataClasses1DataContext db;
        //// GET: api/PickUp/GetReadytoScan
        ///// <summary>
        ///// 获取待扫件的包裹
        ///// </summary>
        ///// <remarks>获取待扫件的包裹</remarks>
        ///// <returns>返回</returns>
        //[HttpGet, Route("PickUp/GetReadytoScan")]
        //public List<PackageClass> GetReadytoScan()
        //{
        //    db = new DataClasses1DataContext(connstr);
        //    var selectQuery = from package1 in db.Package where package1.vId !=null&&!(from p in db.PickUp select p.id).Contains(package1.id) select package1;
        //    List<PackageClass> list = new List<PackageClass>();
        //    foreach(var packageclass in selectQuery)
        //    {
        //        PackageClass package = new PackageClass();


        //        package.id = packageclass.id;
        //        package.weight = packageclass.weight;
        //        package.price = packageclass.price;
        //        package.sendId = packageclass.sendId;
        //        package.receiverId = packageclass.receiverId;

        //        package.srcId = packageclass.srcId;
        //        package.destId = packageclass.destId;
        //        package.vId = packageclass.vId;
        //        package.Remarks = packageclass.Remarks;
        //        package.account = packageclass.account;
        //        list.Add(package);
        //    }
            
        //    return list;
        //}

        //// GET: api/PickUp/GetReadytoReceive
        ///// <summary>
        ///// 获取待揽件的包裹
        ///// </summary>
        ///// <remarks>获取待揽件的包裹</remarks>
        ///// <returns>返回</returns>
        //[HttpGet, Route("PickUp/GetReadytoReceive")]
        //public List<dynamic> GetReadytoReceive()
        //{
        //    db = new DataClasses1DataContext(connstr);
        //    var selectQuery = from a in db.Package join b in db.AddressBook on a.receiverId equals b.aId  where a.vId == null && !(from p in db.PickUp select p.id).Contains(a.id) select new { object1 = a, object2 = b };
        //    List<dynamic> list = new List<dynamic>();
        //    foreach (var packageclass in selectQuery)
        //    {
        //        PackageClass package = new PackageClass();


        //        package.id = packageclass.object1.id;
        //        package.weight = packageclass.object1.weight;
        //        package.price = packageclass.object1.price;
        //        package.sendId = packageclass.object1.sendId;
        //        package.receiverId = packageclass.object1.receiverId;

        //        package.srcId = packageclass.object1.srcId;
        //        package.destId = packageclass.object1.destId;
        //        package.vId = packageclass.object1.vId;
        //        package.Remarks = packageclass.object1.Remarks;
        //        package.account = packageclass.object1.account;

        //        AddressBookClass aclass = new AddressBookClass();
        //        aclass.aId = packageclass.object2.aId;
        //        aclass.account = packageclass.object2.account;
        //        aclass.province = packageclass.object2.province;
        //        aclass.city = packageclass.object2.city;
        //        aclass.street = packageclass.object2.street;
        //        aclass.phone = packageclass.object2.phone;
        //        aclass.name = packageclass.object2.name;

        //        list.Add(new {object1=package,object2=aclass });
        //    }

        //    return list;
        //}

        //// GET: api/PickUp/GetReceiving
        ///// <summary>
        ///// 获取某员工正在揽件的包裹信息
        ///// </summary>
        ///// <remarks>获取待揽件的包裹</remarks>
        ///// <returns>返回</returns>
        //[HttpGet, Route("PickUp/GetReceiving")]
        //public List<PackageClass> GetReceiving(string account)
        //{
        //    db = new DataClasses1DataContext(connstr);
        //    var selectQuery = from package1 in db.Package where package1.vId == null && !(from p in db.PickUp select p.id).Contains(package1.id) select package1;
        //    List<PackageClass> list = new List<PackageClass>();
        //    foreach (var packageclass in selectQuery)
        //    {
        //        PackageClass package = new PackageClass();


        //        package.id = packageclass.id;
        //        package.weight = packageclass.weight;
        //        package.price = packageclass.price;
        //        package.sendId = packageclass.sendId;
        //        package.receiverId = packageclass.receiverId;

        //        package.srcId = packageclass.srcId;
        //        package.destId = packageclass.destId;
        //        package.vId = packageclass.vId;
        //        package.Remarks = packageclass.Remarks;
        //        package.account = packageclass.account;
        //        list.Add(package);
        //    }

        //    return list;
        //}
        // POST: api/PickUp
        public void Post([FromBody]string value)
        {
        }

        // PUT: api/PickUp/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/PickUp/5
        public void Delete(int id)
        {
        }
    }
}

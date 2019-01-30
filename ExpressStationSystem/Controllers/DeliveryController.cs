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

        private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;


        // GET: api/Delivery/GetAllDelivery
        /// <summary>
        /// 通过员工ID获取所有待配送和已配送的快递ID列表
        /// </summary>
        /// <remarks>当前方法通过员工ID获取所有待配送和已配送的快递ID列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetAllDelivery")]
        public IEnumerable<int> GetAllDelivery(int mid)
        {
            db = new DataClasses1DataContext(connstr);
            List<int> ids = new List<int>();
            var query =
                from delivery in db.Delivery
                where delivery.mId == mid
                select delivery;
            foreach(var del in query)
            {
                ids.Add(del.id);
            }
            return ids;
        }

        // GET: api/Delivery/IsDelivered?id={id}
        /// <summary>
        /// 根据快递ID查看快递是否已签收
        /// </summary>
        /// <param name="id">快递ID</param>
        /// <remarks>当前方法根据快递ID查看快递是否已签收</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/IsDelivered")]
        public bool IsDelivered(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var query =
                from delivery in db.Delivery
                where delivery.id == id
                select delivery;
            Delivery del = query.FirstOrDefault();
            return del != null && del.status != "待派送";
        }

        // GET: api/Delivery/GetDeliveryById?id={id}
        /// <summary>
        /// 根据快递ID查看快递信息
        /// </summary>
        /// <param name="id">快递ID</param>
        /// <remarks>当前方法根据快递ID查看快递信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetDeliveryById")]
        public MyDelivery GetDeliveryById(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var query =
                from delivery in db.Delivery
                where delivery.id == id
                select delivery;
            Delivery del = query.FirstOrDefault();
            if (del == null) return null;
            MyDelivery mydel = new MyDelivery();
            mydel.Time = del.time;
            mydel.Id = del.id;
            mydel.Status = del.status;
            var query1 =
                from Package in db.Package
                where Package.id == id
                select Package;
            int cid = query1.FirstOrDefault().receiverId;
            var query2 =
                from Customer in db.Customer
                where Customer.cId == cid
                select Customer;
            Customer cus = query2.FirstOrDefault();
            mydel.City = cus.city;
            mydel.Province = cus.province;
            mydel.Street = mydel.Street;
            mydel.Name = cus.name;
            mydel.Phone = cus.phone;
            return mydel;
        }

        public class MyDelivery
        {
            public MyDelivery() { }

            private int id;

            private string name;

            private string phone;

            private string province;

            private string city;

            private string street;

            private string status;

            private DateTime time;

            public int Id { get => id; set => id = value; }
            public string Name { get => name; set => name = value; }
            public string Phone { get => phone; set => phone = value; }
            public string Province { get => province; set => province = value; }
            public string City { get => city; set => city = value; }
            public string Street { get => street; set => street = value; }
            public string Status { get => status; set => status = value; }
            public DateTime Time { get => time; set => time = value; }
        }
    }
}
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

        // GET: api/Delivery/GetNotInDelivery
        /// <summary>
        /// 通过网点ID获取未接单待派送的快递
        /// </summary>
        /// <param name="bid">网点ID</param>
        /// <remarks>当前方法通过网点ID获取未接单待派送的快递</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetNotInDelivery")]
        public IEnumerable<int> GetNotInDelivery(int bid){
            List<int> ids = new List<int>();
            db = new DataClasses1DataContext(connstr);
            var query = from Package in db.Package
                        where Package.destId == bid
                        select Package.id;
            foreach(var id in query)
            {
                var query2 = from Delivery in db.Delivery
                             where Delivery.id == id
                             select Delivery;
                if (query2.FirstOrDefault() == null) ids.Add(id);
            }
            return ids;
        }

        //POST: api/Delivery/AddDelivery
        /// <summary>
        /// 员工接单
        /// </summary>
        /// <param name="id">快递ID</param>
        /// <param name="mid">员工ID</param>
        /// <remarks>当前进行员工接单</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Delivery/AddDelivery")]
        public bool AddDelivery(int id, int mid)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                Delivery del = new Delivery();
                del.id = id;
                del.mId = mid;
                del.time = DateTime.Now;
                del.status = "待派送";
                db.Delivery.InsertOnSubmit(del);
                db.SubmitChanges();
                return true;
            }
            catch(Exception e)
            {
                return false;
            }
        }

        // GET: api/Delivery/GetUnDelivered
        /// <summary>
        /// 通过员工ID获取未签收的快递ID列表
        /// </summary>
        /// <param name="mid">员工ID</param>
        /// <remarks>当前方法通过员工ID获取未签收快递ID列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetUnDelivered")]
        public IEnumerable<int> GetUnDelivered(int mid)
        {
            db = new DataClasses1DataContext(connstr);
            List<int> ids = new List<int>();
            var query =
                from delivery in db.Delivery
                where delivery.mId == mid
                select delivery;
            foreach(var del in query)
            {
                if(del.status == "待派送")
                    ids.Add(del.id);
            }
            return ids;
        }

        // GET: api/Delivery/GetDelivered
        /// <summary>
        /// 通过员工ID获取已签收的快递ID列表
        /// </summary>
        /// <param name="mid">员工ID</param>
        /// <remarks>当前方法通过员工ID获取已签收快递ID列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Delivery/GetDelivered")]
        public IEnumerable<int> GetDelivered(int mid)
        {
            db = new DataClasses1DataContext(connstr);
            List<int> ids = new List<int>();
            var query =
                from delivery in db.Delivery
                where delivery.mId == mid
                select delivery;
            foreach (var del in query)
            {
                if (del.status != "待派送")
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
            string receiverPhone = query1.FirstOrDefault().receiverPhone;
            var query2 =
                from Customer in db.Customer
                where Customer.phone == receiverPhone
                select Customer;
            Customer cus = query2.FirstOrDefault();
            mydel.City = cus.city;
            mydel.Province = cus.province;
            mydel.Street = mydel.Street;
            mydel.Name = cus.name;
            mydel.Phone = cus.phone;
            return mydel;
        }

        // PUT: api/Delivery/UpdateDelivered
        /// <summary>
        /// 将快递修改为已签收，并将时间修改为当前时间
        /// </summary>
        /// <param name="id">快递ID</param>
        /// <remarks>将快递修改为已签收，并将时间修改为当前时间</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Delivery/UpdateDelivered")]
        public bool UpdateDelivered(int id)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                Delivery del = db.Delivery.Single(d => d.id == id);
                if (del.status == "已签收") throw new Exception();
                del.status = "已签收";
                del.time = DateTime.Now;
                db.SubmitChanges();
                return true;
            }
            catch (Exception e)
            {
                return false;
            }
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
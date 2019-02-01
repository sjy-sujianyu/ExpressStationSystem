using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class CustomerController : ApiController
    {
        private static string connstr=@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Customer/Get?phone={phone}
        /// <summary>
        /// 根据客户手机号码获取客户的信息
        /// </summary>
        /// <param name="phone">客户手机号码</param>
        /// <remarks>根据客户手机号码获取客户的信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Customer/Get")]
        public Customer Get(string phone)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from customer in db.Customer where customer.phone==phone select customer;
            Customer cus = selectQuery.FirstOrDefault();
            return cus;
        }

        // POST: api/Customer/isTel?tb={tb}
        /// <summary>
        /// 验证手机号码是否合法
        /// </summary>
        /// <param name="tb">手机号码</param>
        /// <remarks>验证手机号码是否合法</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Customer/isTel")]
        public bool isTel(string tb)
        {
            string s = @"^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\d{8}$";
            bool flag = true;
            if (!Regex.IsMatch(tb, s))
            {
                flag = false;
            }
            return flag;
        }
        // POST: api/Customer/Post
        /// <summary>
        /// 向数据库插入客户信息
        /// </summary>
        /// <param name="customerclass">客户实体信息</param>
        /// <remarks>向数据库插入客户信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Customer/Post")]
        public bool Post(CustomerClass customerclass)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                Customer customer = new Customer();
                customer.phone = customerclass.phone;
                customer.province = customerclass.province;
                customer.city = customerclass.city;
                customer.street = customerclass.street;
                customer.name = customerclass.name;
                db.Customer.InsertOnSubmit(customer);

                db.SubmitChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }

        // Put: api/Customer/Update
        /// <summary>
        /// 更新指定手机号码的客户
        /// </summary>
        /// <param name="customer">客户对象</param>
        /// <remarks>更新指定手机号码的客户对象</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Customer/Update")]
        public bool Update(CustomerClass customerclass)
        {
            db = new DataClasses1DataContext(connstr);

            try
            {
                Customer cus = db.Customer.Single(c => c.phone == customerclass.phone);
                cus.name = customerclass.name;
                cus.phone = customerclass.phone;
                cus.province = customerclass.province;
                cus.city = customerclass.city;
                cus.street = customerclass.street;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // DELETE: api/Customer/Delete?phone={phone}
        /// <summary>
        /// 删除指定手机号码的客户
        /// </summary>
        /// <param name="phone">客户手机号码</param>
        /// <remarks>删除指定ID的客户</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("Customer/Delete")]
        public bool Delete(string phone)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                Customer customer = db.Customer.Single(c => c.phone == phone);
                db.Customer.DeleteOnSubmit(customer);
                db.SubmitChanges();
                return true;
            }
            catch(Exception)
            {
                return false;
            }
        }
    }
}

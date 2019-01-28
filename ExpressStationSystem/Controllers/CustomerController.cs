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
        // GET: api/Customer/Get?id={id}
        /// <summary>
        /// 根据客户ID获取客户的信息
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <remarks>当前方法根据客户ID返回客户的信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Customer/Get")]
        public IHttpActionResult Get(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from customer in db.Customer where customer.cId==id select customer;
            Customer cus = selectQuery.FirstOrDefault();
            if(cus is null)
            {
                return NotFound();
            }
            return Json<Customer>(cus);
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
        /// <param name="customer">客户实体信息</param>
        /// <remarks>向数据库插入客户信息
        /// <br>例子：</br>
        /// <br> {</br>
        /// <br>"cId": 0,</br>
        /// <br>"name": "string",</br>
        /// <br>"phone": "string",</br>
        /// <br> "province": "string",</br>
        /// <br>"city": "string",</br>
        /// <br>"street": "string"</br>
        /// <br>}</br>
        /// </remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Customer/Post")]
        public bool Post(Customer customer)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
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
        /// 更新指定ID的客户
        /// </summary>
        /// <param name="customer">客户对象</param>
        /// <remarks>更新指定ID的客户对象
        /// <br>例子：</br>
        /// <br> {</br>
        /// <br>"cId": 0,</br>
        /// <br>"name": "string",</br>
        /// <br>"phone": "string",</br>
        /// <br> "province": "string",</br>
        /// <br>"city": "string",</br>
        /// <br>"street": "string"</br>
        ///<br>}</br>
        /// </remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("Customer/Update")]
        public bool Update(Customer customer)
        {
            db = new DataClasses1DataContext(connstr);

            try
            {
                Customer cus = db.Customer.Single(c => c.cId == customer.cId);
                cus.name = customer.name;
                cus.phone = customer.phone;
                cus.province = customer.province;
                cus.city = customer.city;
                cus.street = customer.street;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // DELETE: api/Customer/Delete?id={id}
        /// <summary>
        /// 删除指定ID的客户
        /// </summary>
        /// <param name="id">客户ID</param>
        /// <remarks>删除指定ID的客户</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("Customer/Delete")]
        public bool Delete(int id)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                Customer customer = db.Customer.Single(c => c.cId == id);
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

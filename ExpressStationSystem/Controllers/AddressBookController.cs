using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.RegularExpressions;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class AddressBookController : ApiController
    {
        private static string connstr=@"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/AddressBook/Get?account={account}
        /// <summary>
        /// 根据客户账号获取地址簿列表
        /// </summary>
        /// <param name="account">客户账号</param>
        /// <remarks>根据客户账号获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("AddressBook/Get")]
        public List<AddressBookClass> Get(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from addressbook in db.AddressBook where addressbook.account==account select addressbook;
            List<AddressBookClass> list = new List<AddressBookClass>();
            foreach(var x in selectQuery)
            {
                AddressBookClass aclass = new AddressBookClass();
                aclass.aId = x.aId;
                aclass.account = x.account;
                aclass.province = x.province;
                aclass.city = x.city;
                aclass.street = x.street;
                aclass.phone = x.phone;
                aclass.name = x.name;
                list.Add(aclass);
            }
            return list;
        }

        // GET: api/AddressBook/GetWithName
        /// <summary>
        /// 根据特定客户账号获取地址簿列表
        /// </summary>
        /// <param name="account">客户账号</param>
        /// <param name="name">客户名字</param>
        /// <remarks>根据特定客户账号获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("AddressBook/GetWithName")]
        public List<AddressBookClass> GetWithName(string account,string name)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from addressbook in db.AddressBook where addressbook.account == account&&addressbook.name==name select addressbook;
            List<AddressBookClass> list = new List<AddressBookClass>();
            foreach (var x in selectQuery)
            {
                AddressBookClass aclass = new AddressBookClass();
                aclass.aId = x.aId;
                aclass.account = x.account;
                aclass.province = x.province;
                aclass.city = x.city;
                aclass.street = x.street;
                aclass.phone = x.phone;
                aclass.name = x.name;
                list.Add(aclass);
            }
            return list;
        }

        // GET: api/AddressBook/GetWithPhone
        /// <summary>
        /// 根据特定客户账号获取地址簿列表
        /// </summary>
        /// <param name="account">客户账号</param>
        /// <param name="phone">客户电话</param>
        /// <remarks>根据特定客户账号获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("AddressBook/GetWithPhone")]
        public List<AddressBookClass> GetWithPhone(string account, string phone)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from addressbook in db.AddressBook where addressbook.account == account && addressbook.phone == phone select addressbook;
            List<AddressBookClass> list = new List<AddressBookClass>();
            foreach (var x in selectQuery)
            {
                AddressBookClass aclass = new AddressBookClass();
                aclass.aId = x.aId;
                aclass.account = x.account;
                aclass.province = x.province;
                aclass.city = x.city;
                aclass.street = x.street;
                aclass.phone = x.phone;
                aclass.name = x.name;
                list.Add(aclass);
            }
            return list;
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
        // POST: api/AddressBook/Post
        /// <summary>
        /// 向数据库插入地址信息
        /// </summary>
        /// <param name="x">地址簿信息实体</param>
        /// <remarks>向数据库插入地址信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("AddressBook/Post")]
        public bool Post(AddressBookClass x)
        {

            db = new DataClasses1DataContext(connstr);
            try
            {
                AddressBook aclass = new AddressBook();
                aclass.aId = x.aId;
                aclass.account = x.account;
                aclass.province = x.province;
                aclass.city = x.city;
                aclass.street = x.street;
                aclass.phone = x.phone;
                aclass.name = x.name;
                db.AddressBook.InsertOnSubmit(aclass);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // Put: api/AddressBook/Update
        /// <summary>
        /// 更新指定地址簿id的地址项信息
        /// </summary>
        /// <param name="x">地址项对象</param>
        /// <remarks>更新指定地址簿id的地址项信息</remarks>
        /// <returns>返回</returns>
        [HttpPut, Route("AddressBook/Update")]
        public bool Update(AddressBookClass x)
        {
            db = new DataClasses1DataContext(connstr);

            try
            {
                AddressBook aclass = db.AddressBook.Single(a => a.aId == x.aId);
                aclass.aId = x.aId;
                aclass.account = x.account;
                aclass.province = x.province;
                aclass.city = x.city;
                aclass.street = x.street;
                aclass.phone = x.phone;
                aclass.name = x.name;

                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // DELETE: api/AddressBook/Delete?aId={aId}
        /// <summary>
        /// 删除指定地址项的记录
        /// </summary>
        /// <param name="aId">地址项id</param>
        /// <remarks>删除指定地址项的记录</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("AddressBook/Delete")]
        public bool Delete(int aId)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                AddressBook ab = db.AddressBook.Single(a => a.aId == aId);
                db.AddressBook.DeleteOnSubmit(ab);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}

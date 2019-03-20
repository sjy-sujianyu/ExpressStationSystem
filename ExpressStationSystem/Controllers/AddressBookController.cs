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
        private static string connstr= @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/AddressBook/Get?account={account}
        /// <summary>
        /// 根据客户账号获取地址簿列表
        /// </summary>
        /// <Example>haha</Example>
        /// <param name="account">客户账号</param>
        /// <remarks>根据客户账号获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("AddressBook/Get")]
        public List<AddressBookClass> Get(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from addressbook in db.AddressBook where addressbook.account==account&&addressbook.isDelete==false select addressbook;
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

        // GET: api/AddressBook/GetAll
        /// <summary>
        /// 获取所有地址簿列表
        /// </summary>
        /// <remarks>根据客户账号获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("AddressBook/GetAll")]
        public List<AddressBookClass> GetAll()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from addressbook in db.AddressBook where addressbook.isDelete == false select addressbook;
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

        // GET: api/AddressBook/GetAllSCAU
        /// <summary>
        /// 获取所有在华南农业大学的地址簿列表
        /// </summary>
        /// <remarks>获取所有在华南农业大学的地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("AddressBook/GetAllSCAU")]
        public List<AddressBookClass> GetAllSCAU()
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from addressbook in db.AddressBook where addressbook.isDelete == false && addressbook.street == "华南农业大学" select addressbook;
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

        // GET: api/AddressBook/GetWithName
        /// <summary>
        /// 根据特定条件获取地址簿列表
        /// </summary>
        /// <param name="account">客户账号</param>
        /// <param name="name">客户名字</param>
        /// <remarks>根据特定条件获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("AddressBook/GetWithName")]
        public List<AddressBookClass> GetWithName(string account,string name)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from addressbook in db.AddressBook where addressbook.account == account&&addressbook.name.StartsWith(name) && addressbook.isDelete == false select addressbook;
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
        /// 根据特定条件获取地址簿列表
        /// </summary>
        /// <param name="account">客户账号</param>
        /// <param name="phone">客户电话</param>
        /// <remarks>根据特定条件获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("AddressBook/GetWithPhone")]
        public List<AddressBookClass> GetWithPhone(string account, string phone)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from addressbook in db.AddressBook where addressbook.account == account && addressbook.phone.StartsWith(phone) && addressbook.isDelete == false select addressbook;
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

        // GET: api/AddressBook/GetWithAddresss
        /// <summary>
        /// 根据特定条件获取地址簿列表
        /// </summary>
        /// <param name="account">客户账号</param>
        /// <param name="address">客户地址</param>
        /// <remarks>根据特定条件获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("AddressBook/GetWithAddresss")]
        public List<AddressBookClass> GetWithAddresss(string account, string address)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from addressbook in db.AddressBook where addressbook.account == account && addressbook.isDelete == false && ( addressbook.province.StartsWith(address)||addressbook.city.StartsWith(address)||addressbook.street.StartsWith(address))  select addressbook;
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
        
        // POST: api/AddressBook/Post
        /// <summary>
        /// 向数据库插入地址信息
        /// </summary>
        /// <param name="x">地址簿信息实体  aId没用</param>
        /// <remarks>向数据库插入地址信息</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("AddressBook/Post")]
        public bool Post(AddressBookClass x)
        {
            db = new DataClasses1DataContext(connstr);

            try
            {
                AddressBook aclass = db.AddressBook.Single(a=>a.account==x.account&&a.phone==x.phone&&a.province==x.province&&a.city==x.city&&a.street==x.street&&a.name==x.name);
                if(aclass.isDelete==false)
                {
                    return true;
                }
                aclass.isDelete = false;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                try
                {
                    AddressBook aclass = new AddressBook();
                    aclass.account = x.account;
                    aclass.province = x.province;
                    aclass.city = x.city;
                    aclass.street = x.street;
                    aclass.phone = x.phone;
                    aclass.name = x.name;
                    aclass.isDelete = false;
                    db.AddressBook.InsertOnSubmit(aclass);
                    db.SubmitChanges();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
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
                AddressBook aclassNew = db.AddressBook.Single(a => a.aId == x.aId);
                
                bool flag = Post(x);
                if (flag==false)
                {
                    return false;
                }
                Delete(aclassNew.aId);
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
                ab.isDelete = true;
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

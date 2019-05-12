using ExpressStationSystem.Models;
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
        // GET: api/AddressBook/Get?account={account}
        /// <summary>
        /// 根据客户账号获取地址簿列表
        /// </summary>
        /// <Example>haha</Example>
        /// <param name="account">客户账号</param>
        /// <remarks>根据客户账号获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<AddressBookClass> Get(string account)
        {
            return new AddressBook().Get(account);
        }

        // GET: api/AddressBook/GetAll
        /// <summary>
        /// 获取所有地址簿列表
        /// </summary>
        /// <remarks>根据客户账号获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<AddressBookClass> GetAll()
        {
            return new AddressBook().GetAll();
        }

        // GET: api/AddressBook/GetAllSCAU
        /// <summary>
        /// 获取所有在华南农业大学的地址簿列表
        /// </summary>
        /// <remarks>获取所有在华南农业大学的地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<AddressBookClass> GetAllSCAU()
        {
            return new AddressBook().GetAllSCAU();
        }

        // GET: api/AddressBook/GetAllNotInSCAU
        /// <summary>
        /// 获取所有不在华南农业大学的地址簿列表
        /// </summary>
        /// <remarks>获取所有不在华南农业大学的地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<AddressBookClass> GetAllNotInSCAU()
        {
            return new AddressBook().GetAllNotInSCAU();
        }

        // GET: api/AddressBook/GetWithName
        /// <summary>
        /// 根据特定条件获取地址簿列表
        /// </summary>
        /// <param name="account">客户账号</param>
        /// <param name="name">客户名字</param>
        /// <remarks>根据特定条件获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<AddressBookClass> GetWithName(string account,string name)
        {
            return new AddressBook().GetWithName(account, name);
        }

        // GET: api/AddressBook/GetWithPhone
        /// <summary>
        /// 根据特定条件获取地址簿列表
        /// </summary>
        /// <param name="account">客户账号</param>
        /// <param name="phone">客户电话</param>
        /// <remarks>根据特定条件获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<AddressBookClass> GetWithPhone(string account, string phone)
        {
            return new AddressBook().GetWithPhone(account, phone);
        }

        // GET: api/AddressBook/GetWithAddresss
        /// <summary>
        /// 根据特定条件获取地址簿列表
        /// </summary>
        /// <param name="account">客户账号</param>
        /// <param name="address">客户地址</param>
        /// <remarks>根据特定条件获取地址簿列表</remarks>
        /// <returns>返回</returns>
        [HttpGet]
        public List<AddressBookClass> GetWithAddresss(string account, string address)
        {
            return new AddressBook().GetWithAddresss(account, address);
        }
        
        // POST: api/AddressBook/Post
        /// <summary>
        /// 向数据库插入地址信息
        /// </summary>
        /// <param name="x">地址簿信息实体  aId没用</param>
        /// <remarks>向数据库插入地址信息</remarks>
        /// <returns>返回</returns>
        [HttpPost]
        public bool Post(AddressBookClass x)
        {
            return new AddressBook().Post(x);
        }

        // Put: api/AddressBook/Update
        /// <summary>
        /// 更新指定地址簿id的地址项信息
        /// </summary>
        /// <param name="x">地址项对象</param>
        /// <remarks>更新指定地址簿id的地址项信息</remarks>
        /// <returns>返回</returns>
        [HttpPut]
        public bool Update(AddressBookClass x)
        {
            return new AddressBook().Update(x);
        }

        // DELETE: api/AddressBook/Delete?aId={aId}
        /// <summary>
        /// 删除指定地址项的记录
        /// </summary>
        /// <param name="aId">地址项id</param>
        /// <remarks>删除指定地址项的记录</remarks>
        /// <returns>返回</returns>
        [HttpDelete]
        public bool Delete(int aId)
        {
            return new AddressBook().Delete(aId);
        }
    }
}

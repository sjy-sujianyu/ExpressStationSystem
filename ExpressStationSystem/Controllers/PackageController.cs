using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class PackageController : ApiController
    {
        private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;

        // GET: api/Package/GetValue
        /// <summary>
        /// 根据起始地点、目的地点和重量计算包裹价格
        /// </summary>
        /// <param name="srcProvince">源省份</param>
        /// <param name="srcCity">源城市 </param>
        /// <param name="srcStreet">源街道 </param>
        /// <param name="destProvince">目的省份 </param>
        /// <param name="destCity">目的城市 </param>
        /// <param name="destStreet">目的街道 </param>
        /// <param name="weight">重量kg  1.0</param>
        /// <remarks>根据起始地点、目的地点和重量计算包裹价格</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Package/GetValue")]
        public double GetValue(string srcProvince,string srcCity,string srcStreet,string destProvince,string destCity,string destStreet,double weight)
        {
            return 10.0;
        }
        // POST: api/Package/Post
        /// <summary>
        /// 插入包裹记录
        /// </summary>
        /// <param name="packageclass">包裹对象 vId字段可选 Remarks字段可选</param>
        /// <remarks>插入包裹记录</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Package/Post")]
        public bool Post(PackageClass packageclass)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                Package package = new Package();
                package.id = packageclass.id;
                package.weight = packageclass.weight;
                package.price = packageclass.price;
                package.sendId = packageclass.sendId;
                package.receiverId = packageclass.receiverId;
                package.Remarks = packageclass.Remarks;
                package.account = packageclass.account;
                package.time = DateTime.Now;
                package.status = "已下单";
                package.isDelete = false;
                db.Package.InsertOnSubmit(package);

                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        //// PUT: api/Package/Delete
        //public void Put(int id, [FromBody]string value)
        //{
        //}

        // DELETE: api/Package/Delete?id={id}
        /// <summary>
        /// 删除指定ID的包裹信息
        /// </summary>
        /// <param name="id">包裹ID</param>
        /// <remarks>删除指定ID的包裹信息</remarks>
        /// <returns>返回</returns>
        [HttpDelete, Route("Package/Delete")]
        public bool Delete(int id)
        {
            db = new DataClasses1DataContext(connstr);
            try
            {
                Package package = db.Package.Single(c => c.id == id);
                package.isDelete = true;
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

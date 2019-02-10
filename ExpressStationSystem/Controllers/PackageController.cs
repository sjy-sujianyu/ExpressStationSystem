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
        // GET: api/Package/Get?id={id}
        /// <summary>
        /// 根据包裹ID获取包裹的信息
        /// </summary>
        /// <param name="id">包裹ID</param>
        /// <remarks>根据包裹ID获取包裹的信息</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Package/Get")]
        public PackageClass Get(int id)
        {
            db = new DataClasses1DataContext(connstr);
            var selectQuery = from package1 in db.Package where package1.id == id select package1;
            Package packageclass = selectQuery.FirstOrDefault();
            PackageClass package = new PackageClass();

            if (packageclass is null)
            {
                return null;
            }
            package.id = packageclass.id;
            package.weight = packageclass.weight;
            package.price = packageclass.price;
            package.sendId = packageclass.sendId;
            package.receiverId = packageclass.receiverId;
            package.Remarks = packageclass.Remarks;
            package.account = packageclass.account;
            return package;
        }

        // GET: api/Package/GetValue
        /// <summary>
        /// 根据起始地点、目的地点和重量计算包裹价格
        /// </summary>
        /// <param name="srcProvince">源省份 A省</param>
        /// <param name="srcCity">源城市 A市</param>
        /// <param name="srcStreet">源街道 A街道</param>
        /// <param name="destProvince">目的省份 A省</param>
        /// <param name="destCity">目的城市 A市</param>
        /// <param name="destStreet">目的街道 A街道</param>
        /// <param name="weight">重量kg  1.0</param>
        /// <remarks>根据起始地点、目的地点和重量计算包裹价格</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Package/GetValue")]
        public double GetValue(string srcProvince,string srcCity,string srcStreet,string destProvince,string destCity,string destStreet,double weight)
        {
            if(weight==0)
            {
                return 0;
            }
            Address src = new Address(srcProvince,srcCity,srcStreet);
            Address dest =new Address(destProvince,destCity,destStreet);
            double a = Math.Abs(src.Province[0] - dest.Province[0])*10;
            double b = Math.Abs(src.City[0] - dest.City[0]) * 5;
            double c = 0;
            if(weight<1)
            {
                c = 10;
            }
            else
            {
                c = (weight - 1) * 5+10;
            }
            return a+b+c;
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
                db.Package.DeleteOnSubmit(package);
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

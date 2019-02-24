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
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
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
        public double GetValue(string srcProvince,string destProvince,double weight)
        {
            double value = 0.0;
            string[] value5 = { "江苏省", "浙江省", "上海市" };
            int[] v1 = { 2,2,2 };
            string[] value9 = { "福建省", "江西省", "湖南省", "湖北省" };
            int[] v2 = { 6,6,6 };
            string []value10 = { "广东省", "广西省", "安徽省", "北京市", "天津市" };
            int[] v3 = { 9, 6, 6, 6, 6 };
            string []value12 = { "河南省", "山东省", "山西省", "四川省", "重庆市" };
            int[] v4 = { 9, 10, 10, 8, 8 };
            if(value5.ToList().IndexOf(destProvince)!=-1)
            {
                if(weight<=1)
                {
                    value = 5;
                }
                else
                {
                    value = 5 + (weight - 1) * v1[value5.ToList().IndexOf(destProvince)];
                }
            }
            else if(value9.ToList().IndexOf(destProvince) != -1)
            {
                if(weight<=1)
                {
                    value = 9;
                }
                else
                {
                    value = 9 + (weight - 1) * v2[value9.ToList().IndexOf(destProvince)];
                }
                    
            }
            else if(value10.ToList().IndexOf(destProvince) != -1)
            {
                if(weight<=1)
                {
                    value = 10;
                }
                else
                {
                    value=10+ (weight - 1) * v3[value10.ToList().IndexOf(destProvince)];
                }
                    
            }
            else
            {
                if(weight<=1)
                {
                    value = 12;
                }
                else
                {
                    value=12+ (weight - 1) * v4[value12.ToList().IndexOf(destProvince)];
                }
            }
            return value;
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

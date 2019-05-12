using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressStationSystem.Models
{
    public partial class Package
    {
        private DataClasses1DataContext db = new DataClasses1DataContext(Global.connstr);
        // GET: api/PickUp/GetAllPackage
        /// <summary>
        /// 获取所有包裹记录
        /// </summary>
        /// <remarks>获取所有包裹记录</remarks>
        /// <returns>返回</returns>
        public List<int> GetAllPackage(DateTime start, DateTime end)
        {
            var selectQuery = from a in db.Package where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 orderby a.time descending select a.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }
        // GET: api/Package/GetValue
        /// <summary>
        /// 根据起始地点、目的地点和重量计算包裹价格
        /// </summary>
        /// <param name="srcProvince">源省份</param>
        /// <param name="destProvince">目的省份 </param>
        /// <param name="weight">重量kg  1.0</param>
        /// <remarks>根据起始地点、目的地点和重量计算包裹价格</remarks>
        /// <returns>返回</returns>
        public double GetValue(string srcProvince, string destProvince, double weight)
        {
            double value = 0.0;
            string[] value5 = { "江苏省", "浙江省", "上海市" };
            int[] v1 = { 2, 2, 2 };
            string[] value9 = { "福建省", "江西省", "湖南省", "湖北省" };
            int[] v2 = { 6, 6, 6, 6 };
            string[] value10 = { "广东省", "广西省", "安徽省", "北京市", "天津市" };
            int[] v3 = { 9, 6, 6, 6, 6 };
            string[] value12 = { "河南省", "山东省", "山西省", "四川省", "重庆市" };
            int[] v4 = { 9, 10, 10, 8, 8 };
            if (value5.ToList().IndexOf(destProvince) != -1)
            {
                if (weight <= 1)
                {
                    value = 5;
                }
                else
                {
                    value = 5 + (weight - 1) * v1[value5.ToList().IndexOf(destProvince)];
                }
            }
            else if (value9.ToList().IndexOf(destProvince) != -1)
            {
                if (weight <= 1)
                {
                    value = 9;
                }
                else
                {
                    value = 9 + (weight - 1) * v2[value9.ToList().IndexOf(destProvince)];
                }


            }
            else if (value10.ToList().IndexOf(destProvince) != -1)
            {
                if (weight <= 1)
                {
                    value = 10;
                }
                else
                {
                    value = 10 + (weight - 1) * v3[value10.ToList().IndexOf(destProvince)];
                }

            }
            else if (value12.ToList().IndexOf(destProvince) != -1)
            {
                if (weight <= 1)
                {
                    value = 12;
                }
                else
                {
                    value = 12 + (weight - 1) * v4[value12.ToList().IndexOf(destProvince)];
                }
            }
            else
            {
                if (weight <= 1)
                {
                    value = 12;
                }
                else
                    value = 12 * weight;
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
        public bool Post(PackageClass packageclass)
        {
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
                package.initialTime = DateTime.Now;
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
        public bool Delete(int id)
        {
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
        // GET: api/PickUp/GetReadytoReceiveByCondition
        /// <summary>
        /// 按条件查询要揽件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <remarks>按条件查询要揽件的包裹</remarks>
        /// <returns>返回</returns>
        public dynamic GetReadytoReceiveByCondition(string str, string type, int page, int pageSize)
        {
            if (str is null || type is null)
            {
                return null;
            }
            var a = GetReadytoReceive(0, 0);
            List<dynamic> list = new List<dynamic>();
            foreach (var x in a)
            {
                if (type == "单号" && x.package.id.ToString().StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "姓名" && x.src.name.StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "电话" && x.src.phone.StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "街道" && x.src.street.StartsWith(str))
                {
                    list.Add(x);
                }
            }
            return Global.splitpage(list, page, pageSize);
        }

        // GET: api/PickUp/GetReceivingByCondition
        /// <summary>
        /// 按条件查询要揽件的包裹
        /// </summary>
        /// <param name="str">关键字</param>
        /// <param name="type">类型 "单号、姓名、电话、街道"其中一种</param>
        /// <param name="account">员工的mId</param>
        /// <remarks>按条件查询要揽件的包裹</remarks>
        /// <returns>返回</returns>
        public dynamic GetReceivingByCondition(string account, string str, string type, int page, int pageSize)
        {
            if (str is null || type is null)
            {
                return null;
            }
            var a = GetReceiving(account, 0, 0);
            List<dynamic> list = new List<dynamic>();
            foreach (var x in a)
            {
                if (type == "单号" && x.package.id.ToString().StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "姓名" && x.src.name.StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "电话" && x.src.phone.StartsWith(str))
                {
                    list.Add(x);
                }
                else if (type == "街道" && x.src.street.StartsWith(str))
                {
                    list.Add(x);
                }
            }
            return Global.splitpage(list, page, pageSize);
        }
        // GET: api/PickUp/GetReadytoScan
        /// <summary>
        /// 获取上个网点转来待扫件的包裹ID
        /// </summary>
        /// <remarks>获取上个网点转来待扫件的包裹ID</remarks>
        /// <returns>返回</returns>
        public dynamic GetReadytoScan(int page, int pageSize)
        {
            var selectQuery = from a in db.Package join b in db.Path on a.id equals b.id orderby b.time descending where a.status == "运输中" group b by b.id into g select g.First();
            List<dynamic> list = new List<dynamic>();
            foreach (var x in selectQuery)
            {
                if (Global.splitPlace(x.curPlace).street.Contains("华南农业大学") && x.isArrival == true)
                {
                    list.Add(x);
                }
            }
            return Global.splitpage(list, page, pageSize);
        }

        // GET: api/PickUp/GetReadytoReceive
        /// <summary>
        /// 获取已下单,要揽件的包裹ID
        /// </summary>
        /// <remarks>获取待揽件的包裹ID</remarks>
        /// <returns>返回</returns>
        public dynamic GetReadytoReceive(int page, int pageSize)
        {
            var selectQuery = from a in db.Package join b in db.AddressBook on a.sendId equals b.aId join c in db.AddressBook on a.receiverId equals c.aId where a.status == "已下单" && b.street.Contains("华南农业大学") orderby a.time descending select new { package = a, src = b, dest = c };
            List<dynamic> list = new List<dynamic>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return Global.splitpage(list, page, pageSize);
        }

        // GET: api/PickUp/GetReceiving
        /// <summary>
        /// 获取某员工正在揽件的包裹ID
        /// </summary>
        /// <remarks>获取某员工正在揽件的包裹ID</remarks>
        /// <returns>返回</returns>
        public dynamic GetReceiving(string account, int page, int pageSize)
        {
            var selectQuery = from a in db.PickUp.GroupBy(p => p.id).Select(g => g.OrderByDescending(t => t.time).First()) join b in db.Package on a.id equals b.id where b.status == "待揽件" && a.mId == account && a.isDone == false join c in db.AddressBook on b.sendId equals c.aId join d in db.AddressBook on b.receiverId equals d.aId select new { package = b, src = c, dest = d };
            List<dynamic> list = new List<dynamic>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }

            return Global.splitpage(list, page, pageSize);
        }
        // Post: api/PickUp/Post
        /// <summary>
        /// 添加待揽件包裹信息
        /// </summary>
        /// <remarks>添加待揽件包裹信息</remarks>
        /// <returns>返回</returns>
        public bool Post(PickUpClass x)
        {
            try
            {
                var package = db.Package.Single(a => a.id == x.id);
                if (package.status != "已下单")
                {
                    return false;
                }
                PickUp pk = new PickUp();
                pk.id = x.id;
                pk.mId = x.mId;
                pk.time = DateTime.Now;
                pk.isDone = false;
                db.PickUp.InsertOnSubmit(pk);
                package.status = "待揽件";
                package.time = DateTime.Now;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        // Post: api/PickUp/ConfirmPost
        /// <summary>
        /// 添加确认揽件包裹信息
        /// </summary>
        /// <remarks>添加确认揽件包裹信息</remarks>
        /// <returns>返回</returns>
        public bool ConfirmPost(PickUpClass x)
        {
            try
            {
                var package = db.Package.Single(a => a.id == x.id);
                if (package.status != "待揽件")
                {
                    return false;
                }
                PickUp pk = new PickUp();
                pk.id = x.id;
                pk.mId = x.mId;
                pk.time = DateTime.Now;
                pk.isDone = true;
                db.PickUp.InsertOnSubmit(pk);
                package.status = "已扫件";
                package.time = DateTime.Now;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        // PUT: api/PickUp/RevokeReceive
        /// <summary>
        /// 包裹的状态从待揽件撤销为已下单
        /// </summary>
        /// <remarks>包裹的状态从待揽件撤销为已下单</remarks>
        /// <returns>返回</returns>
        public bool RevokeReceive(IdClass iclass)
        {
            var x = db.Package.SingleOrDefault(a => a.id == iclass.id);
            if (x is null)
            {
                return false;
            }
            else
            {
                x.status = "已下单";
                db.SubmitChanges();
                return true;

            }
        }


        // PUT: api/PickUp/Scan?id={id}
        /// <summary>
        /// 包裹的状态从运输中状态变为已扫件
        /// </summary>
        /// <remarks>包裹的状态从运输中状态变为已扫件</remarks>
        /// <returns>返回</returns>
        public bool Scan(IdClass iclass)
        {
            var x = db.Package.SingleOrDefault(a => a.id == iclass.id);
            if (x is null)
            {
                return false;
            }
            else
            {
                if (x.status != "运输中")
                {
                    return false;
                }
                x.status = "已扫件";
                x.time = DateTime.Now;
                var receiver = db.AddressBook.SingleOrDefault(a => a.aId == x.receiverId);
                if (receiver != null && !receiver.street.Contains("华南农业大学"))
                {
                    Error error = new Error();
                    error.id = x.id;
                    error.introduction = "目的地不在此网点附近";
                    error.status = "错件";
                    error.time = DateTime.Now;
                    x.time = DateTime.Now;
                    try
                    {
                        db.Error.InsertOnSubmit(error);
                    }
                    catch
                    {

                    }
                }
                db.SubmitChanges();
                return true;
            }
        }
        // GET: api/Query/GetAllBillByAccount?account={account}
        /// <summary>
        /// 根据客户ID获取所有订单列表
        /// </summary>
        /// <param name="account">客户ID</param>
        /// <remarks>根据客户ID获取所有订单列表</remarks>
        /// <returns>返回</returns>
        public List<int> GetAllBillByAccount(string account)
        {
            var selectQuery = from a in db.Package where a.account == account select a.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }
        // GET: api/Query/GetAllPackagesByAccount?account={account}
        /// <summary>
        /// 根据客户ID获取所有接受的快递
        /// </summary>
        /// <param name="account">客户ID</param>
        /// <remarks>根据客户ID获取所有接受的快递</remarks>
        /// <returns>返回</returns>
        public List<int> GetAllPackagesByAccount(string account)
        {
            var selectQuery = from a in db.Package join b in db.AddressBook on a.receiverId equals b.aId where b.phone == account select a.id;
            List<int> list = new List<int>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }
        // GET: api/Query/GetAllInfo?account={account}
        /// <summary>
        /// 得到包裹全部信息
        /// </summary>
        /// <param name="id">包裹ID</param>
        /// <remarks>得到包裹全部信息 包裹状态有已下单、待揽件、已扫件、运输中、派件中、已签收状态
        /// <br>
        /// 错误状态：错件，漏件，拒收，破损，丢件
        /// </br>
        /// </remarks>
        /// <returns>返回</returns>
        public dynamic GetAllInfo(int id)
        {
            var package = db.Package.SingleOrDefault(a => a.id == id);
            if (package is null)
            {
                return null;
            }
            var pickup = db.PickUp.Where(a => a.id == package.id).OrderByDescending(a => a.time).Join(db.Member, a => a.mId, b => b.mId, (a, b) => new { pickup = a, member = b }).ToList();
            var src = db.AddressBook.SingleOrDefault(a => a.aId == package.sendId);
            var dest = db.AddressBook.SingleOrDefault(a => a.aId == package.receiverId);
            var delivery = db.Delivery.Where(a => a.id == package.id).OrderByDescending(a => a.time).Join(db.Member, a => a.mId, b => b.mId, (a, b) => new { delivery = a, member = b }).ToList();
            var transfer = db.Transfer.Where(a => a.id == package.id).OrderByDescending(a => a.time).Join(db.Member, a => a.mId, b => b.mId, (a, b) => new { transfer = a, member = b }).ToList();
            var error = db.Error.Where(a => a.id == package.id).ToList();
            var list = new Path().GetLogisticsInfo(id);
            var acl = new { package = package, pickup = pickup, src = src, dest = dest, delivery = delivery, transfer = transfer, pathList = list, error = error };
            return acl;
        }

        // GET: api/Query/GetAllInfo?account={account}
        /// <summary>
        /// 快速得到时间段内包裹全部信息
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <remarks>快速得到时间段内包裹全部信息 </remarks>
        /// <returns>返回</returns>
        public dynamic GetAllInfoFast(DateTime start, DateTime end)
        {
            List<dynamic> list = new List<dynamic>();
            var package = db.Package.Where(a => DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0).OrderByDescending(a => a.time)
                .Join(db.AddressBook, a => a.sendId, b => b.aId, (a, b) => new { a = a, b = b })
                .Join(db.AddressBook, a => a.a.receiverId, b => b.aId, (a, b) => new { package = a.a, src = a.b, dest = b })
                .GroupJoin(db.Error, x => x.package.id, y => y.id, (x, y) => y.DefaultIfEmpty().Select(z => new { package = x.package, src = x.src, dest = x.dest, error = y.ToList() })).SelectMany(x => x).ToList();
            HashSet<int> set = new HashSet<int>();
            List<dynamic> result = new List<dynamic>();
            foreach (var x in package)
            {
                if (set.Add(x.package.id))
                {
                    result.Add(x);
                }
            }
            return result;
        }
    }
}
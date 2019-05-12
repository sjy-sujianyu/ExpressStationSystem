using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml;

namespace ExpressStationSystem.Models
{
    public partial class Transfer
    {
        private DataClasses1DataContext db = new DataClasses1DataContext(Global.connstr);
        // GET: api/Transfer/GetReadyToTransfer
        /// <summary>
        /// 获取已扫件,要出站的包裹ID
        /// </summary>
        /// <remarks>获取待出站的包裹ID</remarks>
        /// <returns>返回</returns>
        public List<int> GetReadyToTransfer(int page, int pageSize)
        {
            var readytoDelivery = db.Package.Where(a => a.status == "已扫件").Join(db.AddressBook.Where(a => !a.street.Contains("华南农业大学")), a => a.receiverId, b => b.aId, (a, b) => a.id);
            List<dynamic> list = new List<dynamic>();
            foreach (var x in readytoDelivery)
            {
                list.Add(x);
            }
            return Global.splitpage(list, page, pageSize);
        }

        // GET: api/Transfer/GetNext
        /// <summary>
        /// 获取包裹下一站去哪
        /// </summary>
        /// <remarks>获取包裹下一站去哪</remarks>
        /// <returns>返回</returns>
        public string GetNext(int id)
        {
            var p = db.Package.SingleOrDefault(a => a.id == id);
            if (p == null) return null;
            AddressBook a1 = db.AddressBook.SingleOrDefault(a => a.aId == p.sendId);
            AddressBook a2 = db.AddressBook.SingleOrDefault(a => a.aId == p.receiverId);
            if (p.status == "运输中" || (p.status == "已下单" && !a1.street.Contains("华南农业大学")))
            {
                var prepath = db.Path.Where(a => a.id == id).OrderByDescending(a => a.time).FirstOrDefault();
                AddressBookClass a3 = new AddressBookClass();
                if (prepath == null)
                {
                    a3.province = a1.province;
                    a3.city = a1.city;
                    a3.street = a1.street;
                }
                else
                {
                    string x = prepath.curPlace;
                    List<string> y = x.Split('-').ToList();
                    a3.province = y[0];
                    a3.city = y[1];
                    a3.street = y[2];
                }
                if (a2.street == a3.street) return null;
                if (a2.city == a3.city)
                {
                    return a3.province + "-" + a3.city + "-" + a2.street;
                }
                else if (a2.province == a3.province)
                {
                    return a3.province + "-" + a2.city + "-" + a2.city + "分拣中心";
                }
                else
                {
                    XmlDocument doc = new XmlDocument();
                    doc.Load(AppDomain.CurrentDomain.BaseDirectory + "province.xml");
                    string city1 = Simulation.Instance.getCity(doc, a3.province)[0];
                    string city2 = Simulation.Instance.getCity(doc, a2.province)[0];
                    if (a3.city == city1 && a3.street == a3.city + "分拣中心")
                    {
                        return a2.province + "-" + city2 + "-" + city2 + "分拣中心";
                    }
                    else if (a3.street == a3.city + "分拣中心")
                    {
                        return a3.province + "-" + city1 + "-" + city1 + "分拣中心";
                    }
                    else
                    {
                        return a3.province + "-" + a3.city + "-" + a3.city + "分拣中心";
                    }
                }
            }
            return null;
        }

        // Post: api/Transfer/Post
        /// <summary>
        /// 添加出站包裹信息
        /// </summary>
        /// <param name="x">出站包裹信息</param>
        /// <remarks>添加出站包裹信息</remarks>
        /// <returns>返回</returns>
        public bool Post(TransferClass x)
        {
            try
            {
                var package = db.Package.Single(a => a.id == x.id);
                if (package.status != "已扫件")
                {
                    return false;
                }
                Transfer tran = new Transfer();
                tran.id = x.id;
                tran.mId = x.mid;
                tran.vId = x.vid;
                tran.time = DateTime.Now;
                db.Transfer.InsertOnSubmit(tran);
                package.status = "运输中";
                package.time = DateTime.Now;
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }



        // PUT: api/Transfer/Departure
        /// <summary>
        /// 交通工具从站点出发
        /// </summary>
        /// <param name="x">交通工具id</param>
        /// <remarks>交通工具从站点出发</remarks>
        /// <returns>返回</returns>
        public bool Departure(IdClass x)
        {
            try
            {
                var vehicle = db.Vehicle.Single(a => a.vId == x.id);
                if (vehicle == null) return false;
                var packageOnVehicle = db.Transfer.Where(a => a.vId == x.id && a.isDone == false);
                foreach (var tran in packageOnVehicle)
                {
                    tran.isDone = true;
                }
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        // PUT: api/Transfer/RevokeTransfer
        /// <summary>
        /// 把包裹从车上卸下
        /// </summary>
        /// <param name="x">包裹id</param>
        /// <remarks>把包裹从车上卸下</remarks>
        /// <returns>返回</returns>
        public bool RevokeTransfer(IdClass x)
        {
            try
            {
                var transfer = db.Transfer.SingleOrDefault(a => a.id == x.id);
                if (transfer.isDone == true) return false;
                var p = db.Package.SingleOrDefault(a => a.id == x.id);
                db.Transfer.DeleteOnSubmit(transfer);
                p.status = "已扫件";
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
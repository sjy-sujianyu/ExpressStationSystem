using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;

namespace ExpressStationSystem.Models
{
    public partial class Error
    {
        private DataClasses1DataContext db = new DataClasses1DataContext(Global.connstr);
        // PUT: api/Money/ErrorPost
        /// <summary>
        /// 需要定时调用的函数插入漏件，错件
        /// </summary>
        /// <remarks>需要定时调用的函数插入漏件，错件</remarks>
        /// <returns>返回</returns>
        public void ErrorPost(object obj)
        {
            while (true)
            {
                try
                {
                    int time = (int)obj;
                    Thread.Sleep(time);
                }
                catch
                {
                }
                var errorLeak = new Package().GetReadytoScan(0, 0);
                foreach (var x in errorLeak)
                {
                    int id = (int)x.id;
                    var check = db.Error.Where(a => a.id == id && a.status == "漏件");
                    if (check.Count() != 0)
                    {
                        continue;
                    }
                    Error error = new Error();
                    error.introduction = "包裹丢失,没有进站";
                    error.status = "漏件";
                    error.time = DateTime.Now;
                    error.id = x.id;
                    var package = db.Package.SingleOrDefault(a => a.id == error.id);
                    if (package != null)
                    {
                        package.time = DateTime.Now;
                    }
                    Console.WriteLine("包裹id" + x.id + "   " + "status:" + error.status);
                    try
                    {

                        db.Error.InsertOnSubmit(error);
                        db.SubmitChanges();
                    }
                    catch (Exception)
                    {

                    }
                }

            }
        }
        // PUT: api/Money/Complaint
        /// <summary>
        /// 客户投诉
        /// </summary>
        /// <param name="x">罚款实体信息</param>
        /// <remarks>客户投诉</remarks>
        /// <returns>返回</returns>
        public bool Complaint(ErrorClass x)
        {
            Error error = new Error();
            error.id = x.id;
            error.introduction = x.introduction;
            error.status = x.status;
            error.time = DateTime.Now;
            var package = db.Package.SingleOrDefault(a => a.id == error.id);
            if (package != null)
            {
                package.time = DateTime.Now;
            }
            try
            {
                db.Error.InsertOnSubmit(error);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        public dynamic getError(int id)
        {
            var error = db.Error.Where(a => a.id == id).ToList(); ;
            return error;
        }
        // GET: api/Query/GetErrorPackage
        /// <summary>
        /// 根据错误包裹
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <param name="page">当前页数</param>
        /// <param name="pageSize">总共页数</param>
        /// <param name="status">错误信息状态：拒签、错件、漏件、破损、丢件、全部</param>
        /// <remarks>根据错误包裹</remarks>
        /// <returns>返回</returns>
        public List<int> GetErrorPackage(DateTime start, DateTime end, int page, int pageSize, string status)
        {
            if (status == "全部")
            {
                List<dynamic> list = new List<dynamic>();
                var error = from a in db.Error join b in db.Package on a.id equals b.id join c in db.AddressBook on b.sendId equals c.aId join d in db.AddressBook on b.receiverId equals d.aId where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 select new { package = b, src = c, dest = d };
                foreach (var x in error)
                {
                    list.Add(x);
                }
                return Global.splitpage(list, page, pageSize);
            }
            else
            {
                List<dynamic> list = new List<dynamic>();
                var error = from a in db.Error join b in db.Package on a.id equals b.id join c in db.AddressBook on b.sendId equals c.aId join d in db.AddressBook on b.receiverId equals d.aId where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == status select new { package = b, src = c, dest = d };
                foreach (var x in error)
                {
                    list.Add(x);
                }
                return Global.splitpage(list, page, pageSize);
            }
        }
        // GET: api/Query/GetErrorPackage
        /// <summary>
        /// 获得某员工被投诉的记录
        /// </summary>
        /// <remarks>获得某员工被投诉的记录</remarks>
        /// <returns>返回</returns>
        public List<dynamic> GetErrorByAccount(string account)
        {
            List<dynamic> list = new List<dynamic>();
            var error = from a in db.Error join b in db.Delivery on a.id equals b.id where (a.status == "破损" || a.status == "丢件") orderby b.time descending group new { error = a, delivery = b } by b.id into g select g.First();
            foreach (var x in error)
            {
                if (x.delivery.mId == account)
                {
                    list.Add(x);
                }
            }
            return list;
        }
    }
}
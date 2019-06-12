using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Web;

namespace ExpressStationSystem.Models
{
    public partial class Member
    {
        private DataClasses1DataContext db = new DataClasses1DataContext(Global.connstr);
        // GET: api/Query/GetImageByAccount?account={account}
        /// <summary>
        /// 返回图片路径
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回图片路径</remarks>
        /// <returns>返回</returns>
        public string Get(string account)
        {
            var member = db.Member.SingleOrDefault(a => a.mId == account);
            if (member is null)
            {
                return null;
            }
            else
            {

                return member.imagePath;
            }
        }
        
        /// <summary>
        /// 获取公告信息
        /// </summary>
        /// <remarks>获取公告信息</remarks>
        /// <returns>返回</returns>
        public string GetPublishMessage(string account)
        {
            var member = db.Member.SingleOrDefault(a => a.mId == account);
            if (member == null) return null;
            if (member.onDuty == false)
            {
                return Global.pulibshLeaveMessage;
            }
            else
            {
                return Global.publishMessage;
            }
        }
        // GET: api/Image/Post
        /// <summary>
        /// [upload]上传文件
        /// </summary>
        /// <remarks>[upload]上传文件</remarks>
        /// <returns>返回</returns>
        public bool Post(string account)
        {
            if (!Directory.Exists(HttpContext.Current.Server.MapPath("/image")))
            {
                Directory.CreateDirectory(HttpContext.Current.Server.MapPath("/image"));
            }
            HttpFileCollection files = HttpContext.Current.Request.Files;

            foreach (string key in files.AllKeys)
            {
                HttpPostedFile file = files[key];//file.ContentLength文件长度  
                if (string.IsNullOrEmpty(file.FileName) == false)
                {
                    string[] x = file.FileName.Split('.');
                    TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                    int timestamp = (int)t.TotalSeconds;
                    string path = HttpContext.Current.Server.MapPath("/image") + "\\" + timestamp.ToString() + "." + x[x.Length - 1];
                    var member = db.Member.SingleOrDefault(a => a.mId == account);
                    if (member is null)
                    {
                        return false;
                    }
                    else
                    {
                        if (!member.imagePath.Equals("无"))
                        {
                            string[] y = member.imagePath.Split('/');
                            string oldPath = HttpContext.Current.Server.MapPath("/image") + "\\" + y[y.Length - 1];
                            File.Delete(oldPath);
                        }
                        string[] str = path.Split('\\');
                        member.imagePath = "172.16.33.125/image/" + str[str.Length - 1];
                        db.SubmitChanges();
                        file.SaveAs(path);
                        return true;
                    }
                }

            }

            return false;
        }
        // GET: api/PickUp/GetAllMember
        /// <summary>
        /// 获取所有员工
        /// </summary>
        /// <remarks>获取所有员工</remarks>
        /// <returns>返回</returns>
        public List<string> GetAllMember()
        {
            var selectQuery = from a in db.Member select a.mId;
            List<string> list = new List<string>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }

            return list;
        }
        // GET: api/Manager/PostMember
        /// <summary>
        /// 新增员工信息
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>新增员工信息</remarks>
        /// <returns>返回</returns>
        public bool PostMember(MemberClass x)
        {
            List<string> list = new List<string>() { "派件员", "揽件员", "出件员", "休息中", "经理", "待定中" };
            if (!list.Contains(x.job))
            {
                return false;
            }
            var check = db.Member.SingleOrDefault(a => a.mId == x.mId && a.name == x.name);
            var y = db.Login.SingleOrDefault(a => a.account == x.mId);
            if (check != null && y != null)
            {
                check.job = x.job;
                check.baseSalary = x.baseSalary;
                check.imagePath = "无";
                check.onDuty = true;
                check.isDelete = false;
                check.time = DateTime.Now;
                y.isDelete = false;
                db.SubmitChanges();
                return true;
            }
            Login login = new Login();
            login.account = x.mId;
            login.password = "123";
            login.isDelete = false;
            Member member = new Member();
            member.mId = x.mId;
            member.job = x.job;
            member.name = x.name;
            member.isDelete = false;
            member.baseSalary = x.baseSalary;
            member.imagePath = "无";
            member.onDuty = true;
            member.time = DateTime.Now;
            try
            {
                if (y == null)
                {
                    db.Login.InsertOnSubmit(login);
                    db.SubmitChanges();
                }
                db.Member.InsertOnSubmit(member);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
        // PUT: api/PickUp/ChangeMemberInfo?account={account}
        /// <summary>
        /// 改变员工职位、名字、底薪
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工职位、名字、底薪
        /// <br>job状态: "派件员","收件员", "出件员","休息中","经理","待定中"</br>
        /// </remarks>
        /// <returns>返回</returns>
        public bool ChangeMemberInfo(MemberClass x)
        {
            List<string> list = new List<string>() { "派件员", "揽件员", "出件员", "休息中", "经理", "待定中" };
            if (!list.Contains(x.job))
            {
                return false;
            }
            var member = db.Member.SingleOrDefault(a => a.mId == x.mId);
            if (member is null)
            {
                return false;
            }
            else
            {
                if (member.job != x.job)
                {
                    if (!checkDoJob(x.mId))
                    {
                        return false;
                    }
                }
                member.job = x.job;
                member.name = x.name;
                member.baseSalary = x.baseSalary;
                db.SubmitChanges();
                return true;
            }
        }


        // PUT: api/PickUp/ChangeJob
        /// <summary>
        /// 改变员工职位
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工职位
        /// <br>job状态: "派件员","收件员", "出件员","休息中","经理","待定中"</br>
        /// </remarks>
        /// <returns>返回</returns>
        public bool ChangeJob(jobClass x)
        {
            if (!checkDoJob(x.mId))
            {
                return false;
            }
            List<string> list = new List<string>() { "派件员", "揽件员", "出件员", "休息中", "经理", "待定中" };
            if (!list.Contains(x.job))
            {
                return false;
            }
            var member = db.Member.SingleOrDefault(a => a.mId == x.mId);
            if (member is null)
            {
                return false;
            }
            else
            {
                member.job = x.job;
                db.SubmitChanges();
                return true;
            }
        }
        // PUT: api/PickUp/ChangeDuty
        /// <summary>
        /// 改变员工休息或者上班状态
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工休息或者上班状态</remarks>
        /// <returns>返回</returns>
        public bool ChangeDuty(onDutyClass x)
        {
            var member = db.Member.SingleOrDefault(a => a.mId == x.mId);
            if (member is null)
            {
                return false;
            }
            else
            {
                member.onDuty = x.onDuty;
                db.SubmitChanges();
                return true;
            }
        }

        // PUT: api/PickUp/ChangeMid?account={account}
        /// <summary>
        /// 改变员工账号
        /// </summary>
        /// <param name="x">员工实体</param>
        /// <remarks>改变员工账号</remarks>
        /// <returns>返回</returns>
        public bool ChangeMid(MidChange x)
        {
            SqlConnection conn = new SqlConnection(Global.connstr);
            conn.Open();
            string sql = string.Format("select Login.account from Member join Login on Member.mId=Login.account where Member.isDelete=0 and Member.mId={0}", x.oldMid);
            SqlCommand comm = new SqlCommand(sql, conn);
            if (comm.ExecuteScalar() != null)
            {
                string update = string.Format("update Login set account={0} where account={1}", x.newMid, x.oldMid);
                SqlCommand updateComm = new SqlCommand(update, conn);
                try
                {
                    int n = updateComm.ExecuteNonQuery();
                    if (n != 0)
                    {
                        conn.Close();
                        return true;
                    }
                    else
                    {
                        conn.Close();
                        return false;
                    }
                }
                catch (Exception)
                {
                    return false;
                }
            }
            conn.Close();
            return false;
        }
        // DELETE: api/Manager/DeleteMember?account={account}
        /// <summary>
        /// 解雇某个员工
        /// </summary>
        /// <param name="aclass">账户实体信息</param>
        /// <remarks>解雇某个员工</remarks>
        /// <returns>返回</returns>
        public bool DeleteMember(accountClass aclass)
        {
            if (!checkDoJob(aclass.account))
            {
                return false;
            }
            var x = db.Member.SingleOrDefault(a => a.mId == aclass.account);
            if (x is null)
            {
                return false;
            }
            x.isDelete = true;
            x.onDuty = false;
            x.time = DateTime.Now;
            db.SubmitChanges();
            return true;
        }
        // PUT: api/Manager/RevokeDeleteMember?account={account}
        /// <summary>
        /// 撤销解雇某个员工
        /// </summary>
        /// <param name="aclass">账户实体信息</param>
        /// <remarks>撤销解雇某个员工</remarks>
        /// <returns>返回</returns>
        public bool RevokeDeleteMember(accountClass aclass)
        {
            var x = db.Member.SingleOrDefault(a => a.mId == aclass.account);
            if (x is null)
            {
                return false;
            }
            x.isDelete = false;
            x.onDuty = true;
            db.SubmitChanges();
            return true;
        }
        private bool checkDoJob(string account)
        {
            var pickUpQuery = from a in db.PickUp orderby a.time descending join b in db.Package on a.id equals b.id where b.status == "待揽件" group a by a.id into g select g.First();
            foreach (var x in pickUpQuery)
            {
                if (x.mId == account && x.isDone == false)
                {
                    return false;
                }
            }
            var delieryQuery = from a in db.Delivery orderby a.time descending join b in db.Package on a.id equals b.id where b.status == "待揽件" group a by a.id into g select g.First();
            foreach (var x in delieryQuery)
            {
                if (x.mId == account && x.isDone == false)
                {
                    return false;
                }
            }
            return true;
        }
        //获取员工的派件，揽件，出件的数量
        public dynamic GetRecordByAccount(string account, DateTime start, DateTime end)
        {
            int pCount = 0;
            int dCount = 0;
            int tCount = 0;
            pCount = db.PickUp.Where(a => a.mId == account && a.isDone == true && DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0).Count();
            dCount = db.Delivery.Where(a => a.mId == account && a.isDone == true && DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0).Count();
            tCount = db.Transfer.Where(a => a.mId == account && DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0).Count();
            return new { PickUpCount = pCount, DeliveryCount = dCount, TransferCount = tCount };
        }
        // GET: api/Query/GetRole?account={account}
        /// <summary>
        /// 返回员工角色(揽件员，派件员，出件员,经理,待定中,休息中)
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回员工角色(揽件员，派件员，出件员,经理,待定中,休息中)</remarks>
        /// <returns>返回</returns>
        public string GetRole(string account)
        {
            var role = db.Member.SingleOrDefault(a => a.mId == account && a.isDelete == false);
            if (role is null)
            {
                return null;
            }
            else
            {
                if (role.onDuty == false)
                {
                    return "休息中";
                }
                return role.job;
            }
        }
        // GET: api/Query/GetMemberAllInfo?account={account}
        /// <summary>
        /// 返回员工信息
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回员工信息</remarks>
        /// <returns>返回</returns>
        public dynamic GetMemberAllInfo(string account)
        {
            var member = db.Member.SingleOrDefault(a => a.mId == account);
            if (member is null)
            {
                return null;
            }
            else
            {
                return member;
            }
        }
        // GET: api/Query/GetTotalRecordByAccount?account={account}
        /// <summary>
        /// 根据总员工历史任务记录
        /// </summary>
        /// <param name="account">客户ID</param>
        /// <remarks>根据总员工历史任务记录</remarks>
        /// <returns>返回</returns>
        public dynamic GetTotalRecordByAccount(string account)
        {
            int pCount = 0;
            int dCount = 0;
            int tCount = 0;
            pCount = db.PickUp.Where(a => a.mId == account && a.isDone == true).Count();
            dCount = db.Delivery.Where(a => a.mId == account && a.isDone == true).Count();
            tCount = db.Transfer.Where(a => a.mId == account).Count();
            return new { PickUpCount = pCount, DeliveryCount = dCount, TransferCount = tCount };
        }
    }
}
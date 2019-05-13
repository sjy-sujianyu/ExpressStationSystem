using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;

namespace ExpressStationSystem.Models
{
    public class Global
    {
        public static string connstr = @"Data Source=172.16.33.125;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private static DataClasses1DataContext db = new DataClasses1DataContext(connstr);
        public static dynamic splitpage(List<dynamic> list, int page, int pagesize)
        {
            if (page == 0 && pagesize == 0)
            {
                return list;
            }
            //总页码数
            int totalpages = list.Count() / pagesize;
            if (list.Count() % pagesize > 0) totalpages++;

            if (page > totalpages)
            {
                return null;
            }

            List<dynamic> result = new List<dynamic>();
            int curpage = 1;
            for (int i = 0; i < list.Count(); i++)
            {
                if (i != 0 && i % pagesize == 0)
                {
                    curpage++;
                }
                if (curpage == page)
                {
                    result.Add(list[i]);
                }

            }
            return new { content = result, curpage = page, pageSize = pagesize, totalpages = totalpages };
        }
        // POST: api/Query/isTel?tb={tb}
        /// <summary>
        /// 验证手机号码是否合法
        /// </summary>
        /// <param name="tb">手机号码</param>
        /// <remarks>验证手机号码是否合法</remarks>
        /// <returns>返回</returns>
        public static bool isTel(string tb)
        {
            string s = @"^((13[0-9])|(14[5|7])|(15([0-3]|[5-9]))|(18[0,5-9]))\d{8}$";
            bool flag = true;
            if (!Regex.IsMatch(tb, s))
            {
                flag = false;
            }
            return flag;
        }
        public static dynamic splitPlace(string place)
        {
            string[] str = place.Split('-');
            return new { province = str[0], city = str[1], street = str[2] };
        }
        // GET: api/Query/GetStatistic
        /// <summary>
        /// 获得各种统计量
        /// </summary>
        /// <param name="start">起始时间</param>
        /// <param name="end">终止时间</param>
        /// <remarks>获得各种统计量</remarks>
        /// <returns>返回</returns>
        public static dynamic GetStatistic(DateTime start, DateTime end)
        {
            var errorError = from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "错件" select a;
            var errorLeak = from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "漏件" select a;
            var errorDamaged = from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "破损" select a;
            var errorRefused = from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "拒签" select a;
            var errorLose = from a in db.Error where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "丢件" select a;
            var delivery = from a in db.Delivery where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDone == true select a;
            var Inbound = from a in db.Package where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == "已扫件" select a;
            var transfer = from a in db.Transfer where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 select a;
            var leave = from a in db.Leave where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.status == 2 select a;
            var memberEmploy = from a in db.Member where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDelete == false select a;
            var memberFired = from a in db.Member where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDelete == true select a;
            var commission = from a in db.Commission where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 select a;
            var vehicleEmploy = from a in db.Vehicle where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDelete == false select a;
            var vehicleFired = from a in db.Vehicle where DateTime.Compare(a.time, start) >= 0 && DateTime.Compare(a.time, end) <= 0 && a.isDelete == true select a;
            return new { errorError = new { content = errorError.ToList(), cnt = errorError.Count() }, errorLeak = new { content = errorLeak.ToList(), cnt = errorLeak.Count() }, errorDamaged = new { content = errorDamaged.ToList(), cnt = errorDamaged.Count() }, errorRefused = new { content = errorRefused.ToList(), cnt = errorRefused.Count() }, errorLose = new { content = errorLose.ToList(), cnt = errorLose.Count() }, delivery = new { content = delivery.ToList(), cnt = delivery.Count() }, pickUp = new { content = Inbound.ToList(), cnt = Inbound.Count() }, transfer = new { content = transfer.ToList(), cnt = transfer.Count() }, leave = new { content = leave.ToList(), cnt = leave.Count() }, memberEmploy = new { content = memberEmploy.ToList(), cnt = memberEmploy.Count() }, memberFired = new { content = memberFired.ToList(), cnt = memberFired.Count() }, commission = new { content = commission.ToList(), cnt = commission.Count() }, vehicleEmploy = new { content = vehicleEmploy.ToList(), cnt = vehicleEmploy.Count() }, vehicleFired = new { content = vehicleFired.ToList(), cnt = vehicleFired.Count() } };
        }
        // GET: api/Tools/CheckPhone
        /// <summary>
        /// 检查手机号码是否本人，并返回短信验证码
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns>返回</returns>
        public static string CheckPhone(string phone)
        {
            const String host = "http://dingxin.market.alicloudapi.com";
            const String path = "/dx/sendSms";
            const String method = "POST";
            const String appcode = "e11ebfb7a2b644b69c2d0a12f39833cf";
            String code = (new Random().Next(1000, 10000)).ToString();
            String querys = "mobile=" + phone + "&param=code:" + code + "&tpl_id=TP1711063";
            String bodys = "";
            String url = host + path;
            HttpWebRequest httpRequest = null;
            HttpWebResponse httpResponse = null;

            if (0 < querys.Length)
            {
                url = url + "?" + querys;
            }

            if (host.Contains("https://"))
            {
                ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(CheckValidationResult);
                httpRequest = (HttpWebRequest)WebRequest.CreateDefault(new Uri(url));
            }
            else
            {
                httpRequest = (HttpWebRequest)WebRequest.Create(url);
            }
            httpRequest.Method = method;
            httpRequest.Headers.Add("Authorization", "APPCODE " + appcode);
            if (0 < bodys.Length)
            {
                byte[] data = Encoding.UTF8.GetBytes(bodys);
                using (Stream stream = httpRequest.GetRequestStream())
                {
                    stream.Write(data, 0, data.Length);
                }
            }
            try
            {
                httpResponse = (HttpWebResponse)httpRequest.GetResponse();
            }
            catch (WebException ex)
            {
                httpResponse = (HttpWebResponse)ex.Response;
            }

            Console.WriteLine(httpResponse.StatusCode);
            Console.WriteLine(httpResponse.Method);
            Console.WriteLine(httpResponse.Headers);
            Stream st = httpResponse.GetResponseStream();
            StreamReader reader = new StreamReader(st, Encoding.GetEncoding("utf-8"));
            JObject jo = (JObject)JsonConvert.DeserializeObject(reader.ReadToEnd());
            string return_code = jo["return_code"].ToString();
            Console.WriteLine(reader.ReadToEnd());
            Console.WriteLine("\n");
            if (return_code != "00000")
            {
                return null;
            }
            else
            {
                return code;
            }


        }

        private static bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }
        /// <summary>
        /// 获取普通用户token
        /// </summary>
        /// <remarks>获取普通用户token</remarks>
        /// <returns>返回</returns>
        public static string GetUserToken(string account,string password)
        {
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, "cookie", DateTime.Now,
                        DateTime.Now.AddHours(24), true, string.Format("{0}&{1}", account, password),
                        FormsAuthentication.FormsCookiePath);
            return FormsAuthentication.Encrypt(ticket);
        }
        /// <summary>
        /// 根据经理账号密码获取最高权限token
        /// </summary>
        /// <param name="account">账号</param>
        /// <param name="password">密码</param>
        /// <remarks>根据经理账号密码获取最高权限token</remarks>
        /// <returns>返回</returns>
        public static string GetAdminToken(string account, string password)
        {
            if(!new Login().ValidateLandOfManager(account,password))
            {
                return null;
            }
            FormsAuthenticationTicket ticket = new FormsAuthenticationTicket(0, "cookie", DateTime.Now,
                        DateTime.Now.AddHours(24), true, string.Format("{0}&{1}", account, password),
                        FormsAuthentication.FormsCookiePath);
            return FormsAuthentication.Encrypt(ticket);
            

        }
        /// <summary>
        /// 校验token的合法性
        /// </summary>
        public static bool ValidateTicket(string encryptTicket)
        {
            try
            {
                if (DateTime.Compare(DateTime.Now, FormsAuthentication.Decrypt(encryptTicket).Expiration) > 0)
                {
                    return false;
                }
                //解密Ticket
                var strTicket = FormsAuthentication.Decrypt(encryptTicket).UserData;

                //从Ticket里面获取用户名和密码
                var str = strTicket.Split('&');
                string account = str[0];
                string password = str[1];
                if (!new Login().ValidateLand(account, password))
                {
                    return false;
                }
                return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
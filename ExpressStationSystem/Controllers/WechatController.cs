using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class WechatController : ApiController
    {
        private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Wechat/GetOpenId
        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="appId">开发者ID</param>
        /// <param name="secret">密钥</param>
        /// <param name="code">用户登陆后的code</param>
        /// <remarks>获取OpenId</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Wechat/GetOpenId")]
        public string GetOpenId(string appId,string secret,string code)
        {
            string html = string.Empty;
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appId + "&secret="+secret+"&js_code=" + code + "&grant_type=authorization_code";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream ioStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
            html = sr.ReadToEnd();
            sr.Close();
            ioStream.Close();
            response.Close();

            JObject jo = (JObject)JsonConvert.DeserializeObject(html);
            if(jo is null)
            {
                return null;
            }
            string openid = jo["openid"].ToString();
            if(openid is null)
            {
                return null;
            }
            try
            {
                db = new DataClasses1DataContext(connstr);
                Login login = new Login();
                login.account = openid;
                login.password = "123456";
                login.role = "客户";
                db.Login.InsertOnSubmit(login);
                db.SubmitChanges();
                return html;
            }
            catch(Exception)
            {
                return html;
            }
                

            
        }
    }
}

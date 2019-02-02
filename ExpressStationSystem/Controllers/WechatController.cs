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
        // GET: api/Wechat/Get
        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="appId">开发者ID</param>
        /// <param name="secret">密钥</param>
        /// <param name="code">用户登陆后的code</param>
        /// <remarks>获取OpenId</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Wechat/Get")]
        public string Get(string appId,string secret,string code)
        {
            string html = string.Empty;
            string url = "https://api.weixin.qq.com/sns/oauth2/access_token?appid="+ appId + "&secret="+secret+"&code=" + code + "&grant_type=authorization_code";
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream ioStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
            html = sr.ReadToEnd();
            sr.Close();
            ioStream.Close();
            response.Close();

            string key = "\"openid\":\"";
            int startIndex = html.IndexOf(key);
            if (startIndex != -1)
            {
                int endIndex = html.IndexOf("\",", startIndex);
                string openid = html.Substring(startIndex + key.Length, endIndex - startIndex - key.Length);
             
                try
                {
                    db = new DataClasses1DataContext(connstr);
                    Login login = new Login();
                    login.account = openid;
                    login.password = "123456";
                    db.Login.InsertOnSubmit(login);
                    db.SubmitChanges();
                    return openid;
                }
                catch(Exception)
                {
                    return openid;
                }
                

            }
            else
            {
                return null;
            }
        }
    }
}

using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class WechatController : ApiController
    {
        private static string connstr = @"Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=Express;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;
        // GET: api/Wechat/GetPhoneNumber
        /// <summary>
        /// 获取OpenId
        /// </summary>
        /// <param name="appId">开发者ID</param>
        /// <param name="secret">密钥</param>
        /// <param name="code">用户登陆后的code</param>
        /// <param name="encryptedDataStr">加密数据</param>
        /// <param name="iv">加密算法初始向量</param>
        /// <remarks>获取OpenId</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Wechat/GetPhoneNumber")]
        private string GetPhoneNumber(string appId, string secret, string code, string encryptedDataStr, string iv)
        {
            string html = string.Empty;
            string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appId + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
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
            if (jo is null)
            {
                return null;
            }
            string session_key = jo["session_key"].ToString();
            if (session_key is null)
            {
                return null;
            }
            string phone= AES_decrypt(encryptedDataStr, session_key, iv);
            try
            {
                Login login = new Login();
                login.account = phone;
                login.password = "123";
                db.Login.InsertOnSubmit(login);
                db.SubmitChanges();
                return phone;
            }
            catch(Exception)
            {
                return phone;
            }

        }
        private string AES_decrypt(string encryptedDataStr, string key, string iv)
        {
            RijndaelManaged rijalg = new RijndaelManaged();
            //-----------------    
            //设置 cipher 格式 AES-128-CBC    

            rijalg.KeySize = 128;

            rijalg.Padding = PaddingMode.PKCS7;
            rijalg.Mode = CipherMode.CBC;

            rijalg.Key = Convert.FromBase64String(key);
            rijalg.IV = Convert.FromBase64String(iv);


            byte[] encryptedData = Convert.FromBase64String(encryptedDataStr);
            //解密    
            ICryptoTransform decryptor = rijalg.CreateDecryptor(rijalg.Key, rijalg.IV);

            string result;

            using (MemoryStream msDecrypt = new MemoryStream(encryptedData))
            {
                using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                {
                    using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                    {

                        result = srDecrypt.ReadToEnd();
                    }
                }
            }

            return result;
        }
    }

        
}

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
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using System.Device.Location;
using ExpressStationSystem.Models;

namespace ExpressStationSystem.Controllers
{
    public class ToolsController : ApiController
    {
        
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
        /// 
        /// <returns>返回</returns>
        //[HttpGet]
        //private string GetPhoneNumber(string appId, string secret, string code, string encryptedDataStr, string iv)
        //{
        //    string html = string.Empty;
        //    string url = "https://api.weixin.qq.com/sns/jscode2session?appid=" + appId + "&secret=" + secret + "&js_code=" + code + "&grant_type=authorization_code";
        //    HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(url);
        //    request.Method = "GET";
        //    HttpWebResponse response = request.GetResponse() as HttpWebResponse;
        //    Stream ioStream = response.GetResponseStream();
        //    StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
        //    html = sr.ReadToEnd();
        //    sr.Close();
        //    ioStream.Close();
        //    response.Close();

        //    JObject jo = (JObject)JsonConvert.DeserializeObject(html);
        //    if (jo is null)
        //    {
        //        return null;
        //    }
        //    string session_key = jo["session_key"].ToString();
        //    if (session_key is null)
        //    {
        //        return null;
        //    }
        //    string phone = AES_decrypt(encryptedDataStr, session_key, iv);
        //    try
        //    {
        //        Login login = new Login();
        //        login.account = phone;
        //        login.password = "123";
        //        db.Login.InsertOnSubmit(login);
        //        db.SubmitChanges();
        //        return phone;
        //    }
        //    catch (Exception)
        //    {
        //        return phone;
        //    }

        //}
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
        // GET: api/Tools/CheckPhone
        /// <summary>
        /// 检查手机号码是否本人，并返回短信验证码
        /// </summary>
        /// <param name="phone">手机号码</param>
        /// <returns>返回</returns>
        [HttpGet]
        public string CheckPhone(string phone)
        {
            return Global.CheckPhone(phone);
            
        }
        // GET: api/Tools/GetLocationProperty
        /// <summary>
        /// 获取经纬度
        /// </summary>
        /// <returns>返回</returns>
        [HttpGet]
        private dynamic GetLocationProperty()
        {
            GeoCoordinateWatcher watcher = new GeoCoordinateWatcher();

            // Do not suppress prompt, and wait 1000 milliseconds to start.
            watcher.TryStart(false, TimeSpan.FromMilliseconds(1000));

            GeoCoordinate coord = watcher.Position.Location;

            if (coord.IsUnknown != true)
            {
                return new { Lat = coord.Latitude, Long = coord.Longitude };
            }
            else
            {
                return null;
            }
        }
        public TOut TransReflection<TIn, TOut>(TIn tIn)
        {
            TOut tOut = Activator.CreateInstance<TOut>();
            var tInType = tIn.GetType();
            foreach (var itemOut in tOut.GetType().GetProperties())
            {
                var itemIn = tInType.GetProperty(itemOut.Name); ;
                if (itemIn != null)
                {
                    itemOut.SetValue(tOut, itemIn.GetValue(tIn));
                }
            }
            return tOut;
        }

        // GET: api/Tools/GetLocationProperty
        /// <summary>
        /// 获取接口里的数据
        /// </summary>
        /// <returns>返回</returns>
        [HttpGet]
        public dynamic GetData(string api)
        {
            string html = string.Empty;
            HttpWebRequest request = (HttpWebRequest)HttpWebRequest.Create(api);
            request.Method = "GET";
            HttpWebResponse response = request.GetResponse() as HttpWebResponse;
            Stream ioStream = response.GetResponseStream();
            StreamReader sr = new StreamReader(ioStream, Encoding.UTF8);
            html = sr.ReadToEnd();
            sr.Close();
            ioStream.Close();
            response.Close();
            return html;
        }

        
    }


}

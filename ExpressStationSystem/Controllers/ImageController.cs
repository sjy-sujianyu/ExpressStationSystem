using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;

namespace ExpressStationSystem.Controllers
{
    public class ImageController : ApiController
    {
        private static string connstr = @"Data Source=172.16.34.153;Initial Catalog=Express;User ID=sa;Password=123456;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
        private DataClasses1DataContext db;

        // GET: api/Query/GetImageByAccount?account={account}
        /// <summary>
        /// 返回图片路径
        /// </summary>
        /// <param name="account">员工账户</param>
        /// <remarks>返回图片路径</remarks>
        /// <returns>返回</returns>
        [HttpGet, Route("Image/GetImageByAccount")]
        public string Get(string account)
        {
            db = new DataClasses1DataContext(connstr);
            var member = db.Member.SingleOrDefault(a => a.mId == account);
            if(member is null)
            {
                return null;
            }
            else
            {
                
                return member.imagePath;
            }
        }

        // GET: api/Image/Post
        /// <summary>
        /// [upload]上传文件
        /// </summary>
        /// <remarks>[upload]上传文件</remarks>
        /// <returns>返回</returns>
        [HttpPost, Route("Image/Post")]
        public bool Post(string account)
        {
            db = new DataClasses1DataContext(connstr);
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
                    string path = HttpContext.Current.Server.MapPath("/image") + "\\"+ timestamp.ToString()+"."+x[x.Length-1];
                    var member = db.Member.SingleOrDefault(a => a.mId == account);
                    if(member is null)
                    {
                        return false;
                    }
                    else
                    {
                        if(!member.imagePath.Equals("无"))
                        {
                            string []y = member.imagePath.Split('/');
                            string oldPath = HttpContext.Current.Server.MapPath("/image") + "\\"+y[y.Length - 1];
                            File.Delete(oldPath);
                        }
                        string[] str = path.Split('\\');
                        member.imagePath = "172.16.34.153:60062/image/" + str[str.Length - 1];
                        db.SubmitChanges();
                        file.SaveAs(path);
                        return true;
                    }
                }
                    
            }

            return false;
        }  

        // PUT: api/Image/5
        public void Put(int id, [FromBody]string value)
        {
        }

        // DELETE: api/Image/5
        public void Delete(int id)
        {
        }
    }
}

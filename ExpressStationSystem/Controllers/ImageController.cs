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
        // GET: api/Image
        public IEnumerable<string> Get()
        {
            return new string[] { "value1", "value2" };
        }

        // GET: api/Image/5
        public string Get(int id)
        {
            return "value";
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
            if (!Directory.Exists("D:\\image"))
            {
                Directory.CreateDirectory("D:\\image");
            }
            HttpFileCollection files = HttpContext.Current.Request.Files;

            foreach (string key in files.AllKeys)
            {
                HttpPostedFile file = files[key];//file.ContentLength文件长度  
                if (string.IsNullOrEmpty(file.FileName) == false)
                {
                    string path = "D:\\image\\" + file.FileName;
                    var member = db.Member.SingleOrDefault(a => a.mId == account);
                    if(member is null)
                    {
                        return false;
                    }
                    else
                    {
                        member.imagePath = path;
                        db.SubmitChanges();
                        file.SaveAs(path);
                    }
                }
                    
            }

            return true;
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

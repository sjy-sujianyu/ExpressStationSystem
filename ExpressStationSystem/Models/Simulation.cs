using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Web;
using System.Xml;
using ExpressStationSystem.Controllers;

namespace ExpressStationSystem.Models
{
    public class Simulation
    {
        private static readonly Simulation instance = new Simulation();

        public static Simulation Instance => instance;

        private string[] telStarts = "134,135,136,137,138,139,150,151,152,157,158,159,130,131,132,155,156,133,153,180,181,182,183,185,186,176,187,188,189,177,178".Split(',');
        string NewPhone()
        {
            Random ran = new Random();
            int n = ran.Next(10, 1000);
            int index = ran.Next(0, telStarts.Length - 1);
            string first = telStarts[index];
            string second = (ran.Next(100, 888) + 10000).ToString().Substring(1);
            string thrid = (ran.Next(1, 9100) + 10000).ToString().Substring(1);
            return first + second + thrid;
        }

        public void AddNewLogin()
        {
            while (true)
            {
                LoginClass login = new LoginClass();
                login.account = NewPhone();
                login.password = "123";
                new LoginController().Post(login);
                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {

                }
            }
        }

        public static List<String> getProvince(XmlDocument doc)
        {
            List<String> provincelist = new List<string>();
            XmlNode provinces = doc.SelectSingleNode("/root");
            foreach (XmlNode province1 in provinces.ChildNodes)
            {
                provincelist.Add(province1.Attributes["name"].Value);
            }
            return provincelist;
        }

        public static List<String> getCity(XmlDocument doc, String provincestr)
        {
            List<String> citylist = new List<string>();
            string xpath = string.Format("/root/province[@name='{0}']/city", provincestr);
            XmlNodeList cities = doc.SelectNodes(xpath);
            foreach (XmlNode city1 in cities)
            {
                citylist.Add(city1.Attributes["name"].Value);
            }
            return citylist;
        }

        public static List<String> getCounty(XmlDocument doc, String provincestr, String citystr)
        {
            List<String> qulist = new List<string>();
            string xpath = string.Format("/root/province[@name='{0}']/city[@name='{1}']/district", provincestr, citystr);
            XmlNodeList area = doc.SelectNodes(xpath);
            foreach (XmlNode area1 in area)
            {
                qulist.Add(area1.Attributes["name"].Value);
            }
            return qulist;

        }

        public string NewName()
        {
            Random rand = new Random();

            string[] nameS3 = new string[] { "赵", "钱", "孙", "李", "周", "吴", "郑", "王", "冯",
 "陈", "褚", "卫", "蒋", "沈", "韩", "杨", "朱", "秦", "尤", "许", "何", "吕", "施",
 "张", "孔", "曹", "严", "华", "金", "魏", "陶", "姜", "戚", "谢", "邹", "喻", "柏",
 "水", "窦", "章", "云", "苏", "潘", "葛", "奚", "范", "彭", "郎" };

            string[] nameS2 = new string[] {"鲁","韦","昌","马","苗","凤","花","方","俞","任","袁"
 ,"柳","酆","鲍","史","唐","费","廉","岑","薛","雷","贺","倪","汤","滕","殷","罗",
 "毕","郝","邬","安","常","乐","于","时","傅","皮","卞","齐","康","伍","余","元",
 "卜","顾","孟","平","黄"};

            string[] nameS1 = new string[] { "梅", "盛", "林", "刁", "锺", "徐", "邱", "骆", "高",
 "夏", "蔡", "樊", "胡", "凌", "霍", "虞", "万", "支", "柯", "昝", "管", "卢", "莫",
 "经", "房", "裘", "缪", "干", "解", "应", "宗", "丁", "宣", "贲", "邓", "郁", "单",
 "杭", "洪", "包", "诸", "左", "石", "崔", "吉", "钮", "龚", "程", "嵇", "邢", "滑",
 "裴", "陆", "荣", "翁", "荀", "羊", "於", "惠", "甄", "麴", "家", "封", "芮", "羿",
 "储", "靳", "汲", "邴", "糜", "松", "井" };

            string s1 = nameS1[rand.Next(0, nameS1.Length - 1)];
            string s2 = nameS2[rand.Next(0, nameS2.Length - 1)];
            string s3 = nameS3[rand.Next(0, nameS3.Length - 1)];
            string name = s1 + s2 + s3;
           
            return name;
        }

        public void AddNewAddress()
        {
            Random rand = new Random();
            while (true)
            {
                try
                {
                    int x = rand.Next(1000);
                    AddressBookClass address = new AddressBookClass();
                    var alllogin = new LoginController().GetAll();
                    address.account = alllogin[rand.Next(alllogin.Count)];
                    address.name = NewName();
                    address.phone = address.account;
                    if (x%2 == 0)
                    {
                        address.province = "广东省";
                        address.city = "广州市";
                        address.street = "华南农业大学";
                    }
                    else
                    {
                        XmlDocument doc = new XmlDocument();
                        doc.Load(AppDomain.CurrentDomain.BaseDirectory + "province.xml");
                        List<string> allprovince = getProvince(doc);
                        string province = allprovince[rand.Next(allprovince.Count)];
                        List<string> allcity = getCity(doc, province);
                        string city = allcity[rand.Next(allcity.Count)];
                        List<string> allstreet = getCounty(doc, province, city);
                        string street = allstreet[rand.Next(allstreet.Count)];
                        
                        address.province = province;
                        address.city = city;
                        address.street = street;
                        
                        
                    }
                    new AddressBookController().Post(address);
                }
                catch (Exception e)
                {

                }
                try
                {
                    Thread.Sleep(1000);
                }
                catch (Exception e)
                {

                }
            }
        }

        public void OrderNewPackage()
        {
            Random rand = new Random();
            while (true)
            {

                var addresslist1 = new AddressBookController().GetAll();
                var addresslist2 = new AddressBookController().GetAllSCAU();

                var address1 = addresslist1[rand.Next(addresslist1.Count())];
                var address2 = addresslist2[rand.Next(addresslist2.Count())];

                if (rand.Next(2) == 0)
                {
                    var temp = address1;
                    address1 = address2;
                    address2 = temp;
                }

                PackageClass p = new PackageClass();
                p.account = address1.account;
                TimeSpan t = (DateTime.UtcNow - new DateTime(1970, 1, 1));
                p.id = (int)t.TotalSeconds;
                p.weight = Convert.ToDecimal(rand.NextDouble() * 10);
                p.sendId = address1.aId;
                p.receiverId = address2.aId;
                p.price = Convert.ToDecimal(new PackageController().GetValue(address1.province, address2.province, Convert.ToDouble(p.weight)));

                new PackageController().Post(p);
                try
                {
                    Thread.Sleep(10000);
                }catch(Exception e)
                {

                }
            }

        }
    }
}
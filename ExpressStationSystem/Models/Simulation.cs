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
            string name = "";
            for(int i = 0; i <= 4; ++i)
            {
                name += Convert.ToChar(rand.Next(26) + 'a');
            }
            return name;
        }

        public void AddNewAddress()
        {
            Random rand = new Random();
            while (true)
            {
                try
                {
                    int x = rand.Next(2);
                    AddressBookClass address = new AddressBookClass();
                    var alllogin = new LoginController().GetAll();
                    address.account = alllogin[rand.Next(alllogin.Count)];
                    address.name = NewName();
                    if (x == 0)
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
                        
                        address.phone = address.account;
                    }
                    new AddressBookController().Post(address);
                }
                catch (Exception e)
                {

                }
                try
                {
                    Thread.Sleep(10000);
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
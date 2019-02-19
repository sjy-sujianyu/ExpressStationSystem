using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressStationSystem
{
    public class PackageClass
    {
        public int id;

        public decimal weight;

        public decimal price;

        public int sendId;

        public int receiverId;

        public string Remarks;

        public string account;
    }
    public class AddressBookClass
    {
        public int aId;

        public string phone;

        public string name;

        public string province;

        public string city;

        public string street;

        public string account;
    }
    public class LoginClass
    {
        public string account;
        public string password;
    }
    public class Address
    {
        private string province;
        private string city;
        private string street;

        public string Province { get => province; set => province = value; }
        public string City { get => city; set => city = value; }
        public string Street { get => street; set => street = value; }

        public Address()
        {
            province = "A省";
            city = "A市";
            street = "A街道";
        }
        public Address(string province, string city, string street)
        {
            Province = province;
            City = city;
            Street = street;
        }
        
    }
    public class PickUpClass
    {
        public int id;
        public string mId;
    }
    public class PickUpClassPlus
    {
        public int id;
        public string mId;
        public DateTime time;
    }

    public class IdClass
    {
        public int id;
    }
}
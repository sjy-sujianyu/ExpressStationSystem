using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressStationSystem
{
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
        public Address(string province,string city,string street)
        {
            Province = province;
            City = city;
            Street = street;
        }
    }
}
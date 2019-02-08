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

        public int srcId;

        public int destId;

        public System.Nullable<int> vId;

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
        public string role;
    }
}
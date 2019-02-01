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

        public string sendPhone;

        public string receiverPhone;

        public int srcId;

        public int destId;

        public System.Nullable<int> vId;

        public string Remarks;
    }
}
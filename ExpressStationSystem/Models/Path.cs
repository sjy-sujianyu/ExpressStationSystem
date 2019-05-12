using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressStationSystem.Models
{
    public partial class Path
    {
        private DataClasses1DataContext db = new DataClasses1DataContext(Global.connstr);
        // GET: api/Query/GetLogisticsInfo?id={id}
        /// <summary>
        /// 根据包裹得到物流信息
        /// </summary>
        /// <param name="id">包裹ID</param>
        /// <remarks>根据包裹得到物流信息</remarks>
        /// <returns>返回</returns>
        public List<dynamic> GetLogisticsInfo(int id)
        {
            var selectQuery = from a in db.Path
                              where a.id == id
                              orderby a.time descending
                              select new { pId = a.pId, id = a.id, srcPlace = Global.splitPlace(a.srcPlace), destPlace = Global.splitPlace(a.destPlace), curPlace = Global.splitPlace(a.curPlace), isArrival = a.isArrival, time = a.time };
            List<dynamic> list = new List<dynamic>();
            foreach (var x in selectQuery)
            {
                list.Add(x);
            }
            return list;
        }
    }
}
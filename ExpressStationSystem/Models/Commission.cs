using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace ExpressStationSystem.Models
{
    public partial class Commission
    {
        private DataClasses1DataContext db = new DataClasses1DataContext(Global.connstr);
        // GET: api/Leave/GetMoneyInfo
        /// <summary>
        /// 获取历史提成价格
        /// </summary>
        /// <remarks>获取历史提成价格</remarks>
        /// <returns>返回</returns>
        public dynamic GetCommision()
        {
            var commision = from a in db.Commission select a;
            if (commision is null)
            {
                return null;
            }
            else
            {
                return commision.ToList();
            }
        }
        // PUT: api/Money/PostCommission
        /// <summary>
        /// 插入提成价格
        /// </summary>
        /// <param name="x">提成价格实体</param>
        /// <remarks>插入提成价格</remarks>
        /// <returns>返回</returns>
        public bool PostCommission(CommisionClass x)
        {
            Commission com = new Commission();
            try
            {
                com.pickUpValue = x.pickUpValue;
                com.deliveryValue = x.deliveryValue;
                com.transferValue = x.transferValue;
                com.time = DateTime.Now;
                db.Commission.InsertOnSubmit(com);
                db.SubmitChanges();
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }
}
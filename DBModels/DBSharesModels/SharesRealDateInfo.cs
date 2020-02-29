using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels.DBSharesModels
{
    public class SharesRealDateInfo
    {
        /// <summary>
        /// 
        /// </summary>
        public string ShareType { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public string ShareCode { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public DateTime ShareDate { get; set; }
        /// <summary>
        ///  开盘 
        /// </summary>
        public double OpeningQuotation { get; set; }
        /// <summary>
        /// 收盘 
        /// </summary>
        public double ClosingQuotation { get; set; }
        /// <summary>
        /// 涨跌 
        /// </summary>
        public string UpsDowns { get; set; }
        /// <summary>
        /// 涨幅 
        /// </summary>
        public string Gain { get; set; }
        public double StockRate { get; set; }
        /// <summary>
        /// 最低
        /// </summary>
        public string Minimum { get; set; }
        /// <summary>
        /// 最高
        /// </summary>
        public string Highest { get; set; }
        /// <summary>
        /// 成交量 
        /// </summary>
        public string Volume { get; set; }
        /// <summary>
        /// 成交额
        /// </summary>
        public string Turnover { get; set; }
        /// <summary>
        /// 换手 
        /// </summary>
        public string ChangeHands { get; set; }
        public string HashCode { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels
{
    public class SharesBasicInfo
    {
        /// <summary>
        /// 股票所属类型
        /// </summary>
        public string ShareType { get; set; }
        /// <summary>
        /// 股票代码
        /// </summary>
        public string ShareCode { get; set; }
        /// <summary>
        /// 股票名字
        /// </summary>
        public string ShareName { get; set; }
        /// <summary>
        /// 昨收
        /// </summary>
        public string YesterdayHarvest { get; set; }
        /// <summary>
        /// 今开
        /// </summary>
        public string TodayOpens { get; set; }
        /// <summary>
        /// 成交量（手）
        /// </summary>
        public string VolumeHand { get; set; }
        /// <summary>
        /// 外盘
        /// </summary>
        public string OuterDisc { get; set; }
        /// <summary>
        /// 内盘
        /// </summary>
        public string Invol { get; set; }
        /// <summary>
        /// 买入价
        /// </summary>
        public string BuyingPrice { get; set; }
        public string BuyingVolume { get; set; }
        /// <summary>
        /// 卖出价
        /// </summary> 
        public string SellingPrice { get; set; }
        public string SellQuantity { get; set; }
        /// <summary>
        /// 最近逐笔成交
        /// </summary>    
        public string RecentDeal { get; set; }
        /// <summary>
        /// 时间
        /// </summary>
        public DateTime ShareTime { get; set; }
        /// <summary>
        /// 涨跌
        /// </summary>
        public string RiseFall { get; set; }
        /// <summary>
        /// 涨跌比例
        /// </summary>
        public string RiseFallRate { get; set; }
        /// <summary>
        /// 最高
        /// </summary>
        public string Highest { get; set; }
        /// <summary>
        /// 最低
        /// </summary>
        public string Minimum { get; set; }
        /// <summary>
        /// 成交额（万）
        /// </summary>
        public string Turnover { get; set; }
        /// <summary>
        /// 市盈率
        /// </summary>
        public string PERatio { get; set; }
        /// <summary>
        /// 振幅
        /// </summary>
        public string Amplitude { get; set; }
        /// <summary>
        /// 流通市值
        /// </summary>
        public string MarketValueCirculation { get; set; }
        /// <summary>
        /// 总市值
        /// </summary>
        public string TotalMarketValue { get; set; }
        /// <summary>
        /// 市净率
        /// </summary>
        public string MarketRate { get; set; }
        /// <summary>
        /// 涨停价
        /// </summary>
        public string HighLimit { get; set; }
        /// <summary>
        /// 跌停价
        /// </summary>
        public string LimitPrice { get; set; }
        public DateTime DownloadDate { get; set; }
        public DateTime CreateTime { get; set; }
    }

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
        public string ShareDate { get; set; }
        /// <summary>
        ///  开盘 
        /// </summary>
        public string OpeningQuotation { get; set; }
        /// <summary>
        /// 收盘 
        /// </summary>
        public string ClosingQuotation { get; set; }
        /// <summary>
        /// 涨跌 
        /// </summary>
        public string UpsDowns { get; set; }
        /// <summary>
        /// 涨幅 
        /// </summary>
        public string Gain { get; set; }
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
    }
}

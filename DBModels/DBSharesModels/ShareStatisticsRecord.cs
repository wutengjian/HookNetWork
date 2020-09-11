using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels.DBSharesModels
{
    public class ShareStatisticsRecord
    {
        /// <summary>
        /// 自增长ID
        /// </summary>
        public int ID { get; set; }
        public string HashCode { get; set; }
        public string StockNo { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Daily { get; set; }
        /// <summary>
        /// 处理方式
        /// </summary>
        public string MethodType { get; set; }
        /// <summary>
        /// 开始日期
        /// </summary>
        public DateTime StartDate { get; set; }
        /// <summary>
        /// 结束日期
        /// </summary>
        public DateTime EndDate { get; set; }
        /// <summary>
        /// 统计结果
        /// </summary>
        public double Statistics { get; set; }
    }
    public class StatisticalResults
    {
        public int ID { get; set; }
        public string HashCode { get; set; }
        public string StockNo { get; set; }
        public string MethodType { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public int LevelMap { get; set; }
        public double MaxValue { get; set; }
        public double MinValue { get; set; }
        public double AvgValue { get; set; }
        /// <summary>
        /// 众数
        /// </summary>
        public double MostValue { get; set; }
    }
}

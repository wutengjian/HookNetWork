using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnalysis.DBModels
{
    /// <summary>
    /// 推荐方式得到的推荐结果记录(由RMMonitor负责管理数据)
    /// </summary>
    public abstract class RecommendMethod
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// AnalysisMethod表的ID
        /// </summary>
        public int MethodID { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Daily { get; set; }
        /// <summary>
        /// 股票ID
        /// </summary>
        public string StockID { get; set; }
        /// <summary>
        /// 编码
        /// </summary>
        public string StockNo { get; set; }
        /// <summary>
        /// 评分
        /// </summary>
        public int Score { get; set; }
        /// <summary>
        /// 预测趋势, -1：下跌、0：持平、1：上涨
        /// </summary>
        public int TrendType { get; set; }
        /// <summary>
        /// 预测可信度（预测）
        /// </summary>
        public double Reliability { get; set; }

        /// <summary>
        /// 实际趋势, -1：下跌、0：持平、1：上涨
        /// </summary>
        public int ActualTrendType { get; set; }
    }
}

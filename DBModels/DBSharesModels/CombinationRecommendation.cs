using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnalysis.DBModels
{
    /// <summary>
    /// 组合推荐(最终推荐)
    /// </summary>
    public class CombinationRecommendation
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Daily { get; set; }
        public int StockID { get; set; }
        public string StockNo { get; set; }
        /// <summary>
        /// 评分/推荐力度
        /// </summary>
        public double Score { get; set; }
        /// <summary>
        /// 准确率，回馈计算
        /// </summary>
        public double AccuracyRate { get; set; }
    }

    /// <summary>
    /// 各种方式推荐的组合项
    /// </summary>
    public class CombinatorialItem
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 组合推荐（CombinationRecommendation表）ID
        /// </summary>
        public int CRID { get; set; }
        /// <summary>
        /// 推荐方式得到的推荐结果记录（RecommendMethod表）ID
        /// </summary>
        public int RMID { get; set; }
    }
}

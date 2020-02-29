using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnalysis.DBModels
{
    /// <summary>
    /// 分析方式
    /// </summary>
    public class AnalysisMethod
    {
        /// <summary>
        /// 主键
        /// </summary>
        public int ID { get; set; }
        /// <summary>
        /// 类型
        /// </summary>
        public string ClassName { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// 详细说明
        /// </summary>
        public string DetailedExplanation { get; set; }
    }
}

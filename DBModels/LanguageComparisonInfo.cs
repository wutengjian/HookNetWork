using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels
{
    /// <summary>
    /// 语言对照信息
    /// </summary>
    public class LanguageComparisonInfo
    {
        /// <summary>
        /// 数据类型：单词、语句等
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 原文
        /// </summary>
        public string OriginalText { get; set; }
        /// <summary>
        /// 译文
        /// </summary>
        public string Translation { get; set; }
        /// <summary>
        /// 原文语言
        /// </summary>
        public string OriginalLang { get; set; }
        /// <summary>
        /// 译文语言
        /// </summary>
        public string TranslationLang { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public int DataState { get; set; }
        /// <summary>
        /// 单词出现次数
        /// </summary>
        public int WordNum { get; set; }
    }
}

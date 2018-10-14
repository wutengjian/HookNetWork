using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels
{
    /// <summary>
    /// 单词切分
    /// </summary>
    public class ArticleWordDivisionInfo
    {
        /// <summary>
        /// 单词
        /// </summary>
        public string Word { get; set; }
        /// <summary>
        /// 单词来源的HashCode(Article表)
        /// </summary>
        public string HashCode { get; set; }
        /// <summary>
        /// 出现次数
        /// </summary>
        public int AppearNum { get; set; }
        /// <summary>
        /// 数据类型:标题、正文、副标题 等
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 数据状态
        /// </summary>
        public int DataState { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBModels
{
    /// <summary>
    /// 文章信息
    /// </summary>
    public class ArticleInfo
    {
        /// <summary>
        /// 唯一值(32位的MD5值)
        /// </summary>
        public string HashCode { get; set; }
        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime CreateTime { get; set; }
        /// <summary>
        /// 标题
        /// </summary>
        public string DataTitle { get; set; }
        /// <summary>
        /// 内容
        /// </summary>
        [Column(TypeName = "text")]
        public string DataContent { get; set; }
        /// <summary>
        /// 数据类型:新闻、文章、词句 等
        /// </summary>
        public string DataType { get; set; }
        /// <summary>
        /// 分词：用途
        /// </summary>
        public string KeyWordSort { get; set; }
        /// <summary>
        /// 数据来源（网站等）
        /// </summary>
        public string DataSource { get; set; }
        /// <summary>
        /// 数据连接
        /// </summary>
        public string DataSourceLink { get; set; }
        /// <summary>
        /// 文章发布时间
        /// </summary>
        public DateTime ArticleTime { get; set; }
    }
}

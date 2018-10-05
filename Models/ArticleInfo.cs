using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ArticleInfo
    {
        public string HashCode { get; set; }
            public DateTime CreateTime { get; set; }
            public string DataTitle { get; set; }
            public string DataContent { get; set; }
            public string DataType { get; set; }
            public string KeyWordSort { get; set; }
            public string DataSource { get; set; }
            public string DataSourceLink { get; set; }
            public DateTime ArticleTime { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models
{
    public class ArticleWordDivisionInfo
    {
        public string Word { get; set; }
        public string HashCode { get; set; }
        public int AppearNum { get; set; }
        public string DataType { get; set; }
        public DateTime CreateTime { get; set; }
        public int DataState { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels
{
    [Table("ArticleFiles")]
    public class ArticleFileInfo
    {
        [Key]
        public string HashCode { get; set; }
        public string FileName { get; set; }
        public string HttpUrl { get; set; }
        public string DataSource { get; set; }
        public DateTime DataTime { get; set; }
    }
    public class ArticleFileModel
    {
        public string HashCode { get; set; }
        public string FileName { get; set; }
        public string HttpUrl { get; set; }
    }
}

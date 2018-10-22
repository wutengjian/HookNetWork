using DBModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepertory
{
    public class ArticleFileSQLite
    {
        public List<ArticleFileModel> AFMList { get; set; }
        public void SaveList(List<ArticleFileInfo> data)
        {
            if (AFMList != null && AFMList.Count > 0)
            {
                var HashCodeArr = AFMList.Select(x => x.HashCode).ToArray();
                var FileNameArr = AFMList.Select(x => x.FileName).ToArray();
                var HttpUrlArr = AFMList.Select(x => x.HttpUrl).ToArray();
                data = data.Where(x => (HashCodeArr.Contains(x.HashCode) == false || FileNameArr.Contains(x.FileName) == false || HttpUrlArr.Contains(x.HttpUrl) == false)).ToList();
            }
            else
            {
                AFMList = new List<ArticleFileModel>(data.Count);
            }
            if (data == null || data.Count < 1)
            {
                return;
            }
            try
            {
                using (SQLiteDbContext dbContext = new SQLiteDbContext())
                {
                    dbContext.ArticleFiles.AddRange(data);
                    dbContext.SaveChangesAsync();
                }
                AFMList.AddRange(data.Select(x => new ArticleFileModel()
                {
                    HashCode = x.HashCode,
                    FileName = x.FileName,
                    HttpUrl = x.HttpUrl
                }));
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQLite " + ex.Message);
            }
        }
        public List<ArticleFileModel> GetList(string FileType)
        {
            try
            {
                using (SQLiteDbContext dbContext = new SQLiteDbContext())
                {
                    return dbContext.ArticleFiles.Where(x => x.DataSource == FileType).Select(x => new ArticleFileModel()
                    {
                        HashCode = x.HashCode,
                        FileName = x.FileName,
                        HttpUrl = x.HttpUrl
                    }).ToList();
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("SQLite " + ex.Message);
                return null;
            }
        }
        public void ArticleFiles_CreateTable()
        {
            Dictionary<string, string> ColDic = new Dictionary<string, string>();
            ColDic.Add("HashCode", "VARCHAR(50)");
            ColDic.Add("FileName", "NVARCHAR(250)");
            ColDic.Add("HttpUrl", "NVARCHAR(250)");
            ColDic.Add("DataSource", "NVARCHAR(50)");
            ColDic.Add("DataTime", "DATE");            
            using (SQLiteDbContext dbContext = new SQLiteDbContext())
            {
                dbContext.CreateTable("ArticleFiles", ColDic);
            }
        }
    }
}

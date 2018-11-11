using Dapper;
using DBModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepertory
{
    public class ArticleWordDivisionDal
    {
        string ConnStr = "Data Source=DESKTOP-WUTENGJ;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian123";
        public void SaveList(List<ArticleWordDivisionInfo> DataList)
        {
            DataList = DataList.Where(x => x.Word.Length < 25).ToList();
            if (DataList == null || DataList.Count < 1)
            {
                return;
            }
            Dictionary<string, string> SqlMapping = new Dictionary<string, string>();
            SqlMapping.Add("Word", "Word");
            SqlMapping.Add("HashCode", "HashCode");
            SqlMapping.Add("AppearNum", "AppearNum");
            SqlMapping.Add("DataType", "DataType");
            SqlMapping.Add("CreateTime", "CreateTime");
            SqlMapping.Add("DataState", "DataState");
            SqlServerBulkCopy.SqlBulkMapping(SqlMapping);
            SqlServerBulkCopy.ConnStr = ConnStr;
            if (DataList.Count > 1000)
            {
                int num = DataList.Count / 1000;
                num = DataList.Count % 10000 > 0 ? num + 1 : num;
                for (int i = 0; i < num; i++)
                {
                    var list = DataList.Skip(i * 1000).Take(1000).ToList();
                    var data = SqlServerBulkCopy.ToDataTable<ArticleWordDivisionInfo>(list);
                    SqlServerBulkCopy.SqlBulkCopyToServer(data, "ArticleWordDivision");
                }
            }
            else
            {
                var data = SqlServerBulkCopy.ToDataTable<ArticleWordDivisionInfo>(DataList);
                SqlServerBulkCopy.SqlBulkCopyToServer(data, "ArticleWordDivision");
            }
        }
        public List<string> GetListWord()
        {

            List<string> List = new List<string>();
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                List = conn.Query<string>(@" SELECT Word
 FROM   ( SELECT   DISTINCT Word ,
                    SUM(AppearNum) OVER ( PARTITION BY Word ) AS RowSum ,
                    DataState
          FROM      [dbo].[ArticleWordDivision] WITH ( NOLOCK )
        ) AS T
 WHERE  DataState = 1
        AND LEN(T.Word) > 1
        AND NOT EXISTS ( SELECT TOP 1
                                1
                         FROM   [dbo].[LanguageComparison] AS LC WITH ( NOLOCK )
                         WHERE  LC.OriginalText = T.Word )
 ORDER BY RowSum DESC ", commandTimeout: 300).ToList();
                conn.Close();
            }
            return List;

        }
    }
}

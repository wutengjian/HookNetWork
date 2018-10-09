using Dapper;
using Models;
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
            DataList = DataList.Where(x => x.Word.Length < 50).ToList();
            Dictionary<string, string> SqlMapping = new Dictionary<string, string>();
            SqlMapping.Add("Word", "Word");
            SqlMapping.Add("HashCode", "HashCode");
            SqlMapping.Add("AppearNum", "AppearNum");
            SqlMapping.Add("DataType", "DataType");
            SqlMapping.Add("CreateTime", "CreateTime");
            SqlMapping.Add("DataState", "DataState");
            SqlServerBulkCopy.SqlBulkMapping(SqlMapping);
            SqlServerBulkCopy.ConnStr = ConnStr;
            if (DataList.Count > 10000)
            {
                int num = DataList.Count / 10000;
                num = DataList.Count % 10000 > 0 ? num + 1 : num;
                for (int i = 0; i < num; i++)
                {
                    var list = DataList.Skip(i * 10000).Take(10000).ToList();
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
                List = conn.Query<string>(@"SELECT TOP 6000 Word FROM(
	SELECT DISTINCT Word,SUM(AppearNum) OVER(PARTITION BY Word) AS RowSum 
	FROM [dbo].[ArticleWordDivision] WITH(NOLOCK) 
	WHERE DataState=1) AS T 
			WHERE NOT EXISTS(SELECT TOP 1 1  FROM [dbo].[LanguageComparison] AS LC WITH(NOLOCK) WHERE LC.OriginalText=T.Word)
ORDER BY RowSum DESC", commandTimeout: 300).ToList();
                conn.Close();
            }
            return List;

        }
    }
}

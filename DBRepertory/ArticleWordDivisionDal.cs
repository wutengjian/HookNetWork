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
            var data = SqlServerBulkCopy.ToDataTable<ArticleWordDivisionInfo>(DataList);
            Dictionary<string, string> SqlMapping = new Dictionary<string, string>();
            SqlMapping.Add("Word", "Word");
            SqlMapping.Add("HashCode", "HashCode");
            SqlMapping.Add("AppearNum", "AppearNum");
            SqlMapping.Add("DataType", "DataType");
            SqlMapping.Add("CreateTime", "CreateTime");
            SqlMapping.Add("DataState", "DataState");
            SqlServerBulkCopy.SqlBulkMapping(SqlMapping);
            SqlServerBulkCopy.ConnStr = ConnStr;
            SqlServerBulkCopy.SqlBulkCopyToServer(data, "ArticleWordDivision");
        }
        public List<string> GetListWord()
        {

            List<string> List = new List<string>();
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                List = conn.Query<string>(@"SELECT  Word FROM(
	SELECT DISTINCT Word,SUM(AppearNum) OVER(PARTITION BY Word) AS RowSum 
	FROM [dbo].[ArticleWordDivision] WITH(NOLOCK) 
	WHERE DataState=1) AS T 
			WHERE NOT EXISTS(SELECT TOP 1 1  FROM [dbo].[LanguageComparison] AS LC WITH(NOLOCK) WHERE LC.OriginalText=T.Word)
ORDER BY RowSum DESC").ToList();
                conn.Close();
            }
            return List;

        }
    }
}

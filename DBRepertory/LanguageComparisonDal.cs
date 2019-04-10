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
    public class LanguageComparisonDal
    {
        string ConnStr = "Data Source=JiannyWu;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian123";
        public void SaveList(List<LanguageComparisonInfo> list)
        {
            List<string> HashList = GetWordlist();
            list = list.Where(x => HashList.Contains(x.OriginalText) == false).ToList();
            var data = SqlServerBulkCopy.ToDataTable<LanguageComparisonInfo>(list);
            Dictionary<string, string> SqlMapping = new Dictionary<string, string>();
            SqlMapping.Add("DataType", "DataType");
            SqlMapping.Add("OriginalText", "OriginalText");
            SqlMapping.Add("Translation", "Translation");
            SqlMapping.Add("OriginalLang", "OriginalLang");
            SqlMapping.Add("TranslationLang", "TranslationLang");
            SqlMapping.Add("CreateTime", "CreateTime");
            SqlMapping.Add("DataState", "DataState");
            SqlMapping.Add("WordNum", "WordNum");
            SqlServerBulkCopy.SqlBulkMapping(SqlMapping);
            SqlServerBulkCopy.ConnStr = ConnStr;
            SqlServerBulkCopy.SqlBulkCopyToServer(data, "LanguageComparison");
        }

        public List<string> GetWordlist()
        {
            List<string> List = new List<string>();
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                List = conn.Query<string>("SELECT DISTINCT [OriginalText]  FROM [dbo].[LanguageComparison] WITH(NOLOCK) ", commandTimeout: 300).ToList();
                conn.Close();
            }
            return List;
        }
        /// <summary>
        /// 跟新word单词出现的次数
        /// </summary>
        public void UpdateWordNum()
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                conn.Execute(@"UPDATE  T
SET     [WordNum] = WordNumNew
FROM    ( SELECT    T.[WordNum] ,
                    D.WordNum AS WordNumNew
          FROM      [dbo].[LanguageComparison] AS T WITH ( NOLOCK )
                    INNER JOIN ( SELECT DISTINCT
                                        SUM([AppearNum]) OVER ( PARTITION BY [Word] ) AS [WordNum] ,
                                        [Word]
                                 FROM   [dbo].[ArticleWordDivision] WITH ( NOLOCK )
                               ) AS D ON T.[OriginalText] = D.[Word]
        ) AS T", commandTimeout: 300);
                conn.Close();
            }
        }
    }
}

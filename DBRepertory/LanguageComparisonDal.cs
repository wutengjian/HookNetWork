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
    public class LanguageComparisonDal
    {
        string ConnStr = "Data Source=DESKTOP-WUTENGJ;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian123";
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
                List = conn.Query<string>("SELECT DISTINCT [OriginalText]  FROM [dbo].[LanguageComparison] WITH(NOLOCK) ").ToList();
                conn.Close();
            }
            return List;
        }
    }
}

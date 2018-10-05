using Dapper;
using Models;
using PublicUnit;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DataAnalysis
{
    /// <summary>
    /// 语词切割
    /// </summary>
    public class WordDivision
    {
        List<ArticleInfo> HashList;
        string ConnStr = string.Empty;
        Dictionary<string, ArticleWordDivisionInfo> dic;
        public WordDivision()
        {
            HashList = new List<ArticleInfo>();
            dic = new Dictionary<string, ArticleWordDivisionInfo>();
            ConnStr = "Data Source=DESKTOP-WUTENGJ;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian123";
        }
        public void Run()
        {
            WordCalculation();
        }
        public void WordCalculation()
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                HashList = conn.Query<ArticleInfo>(@"select HashCode,DataTitle,DataContent 
from [dbo].[Article] as A with(nolock) 
where not EXISTS(
	select top 1 1 from [dbo].[ArticleWordDivision] as AWD with(nolock) where A.HashCode=AWD.HashCode
	)
").ToList<ArticleInfo>();
                conn.Close();
            }
            HashList.ForEach(x =>
            {
                SplitSentence(x.HashCode, x.DataTitle, "Title");
                SplitSentence(x.HashCode, x.DataContent, "Content");
            });
            SaveToSQL(dic.Values.ToList());
        }
        public void SplitSentence(string HashCode, string Content, string DataType)
        {

            var arr = Content.Split(' ');
            foreach (var key in arr)
            {
                if (string.IsNullOrEmpty(key) || key.Length < 2)
                {
                    continue;
                }
                string word = key;
                word = word.ToLower().Replace("’", "'").Trim();
                //过滤".()[],“”
                word = Regex.Replace(word, "(\"|\\.|\\(|\\)|\\[|\\]|,|“|”)", "", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                if (Regex.IsMatch(word, "^[^a-z]+.*", RegexOptions.Singleline | RegexOptions.IgnoreCase) || Regex.IsMatch(word, "\\d+", RegexOptions.Singleline | RegexOptions.IgnoreCase))
                {
                    continue;//不以字母开头
                }
                int DataState = 1;
                if (Regex.IsMatch(word, "[^a-z|A-Z]+", RegexOptions.Singleline | RegexOptions.IgnoreCase))
                {
                    DataState = -1;//出现字母以外的东西
                }
                if (dic.ContainsKey(word + "|" + HashCode))
                {
                    dic[word + "|" + HashCode].AppearNum++;
                }
                else
                {
                    dic.Add(word + "|" + HashCode, new ArticleWordDivisionInfo()
                    {
                        Word = word,
                        HashCode = HashCode,
                        AppearNum = 1,
                        DataType = DataType,
                        CreateTime = DateTime.Now,
                        DataState = DataState
                    });
                }
            }
        }
        public void SaveToSQL(List<ArticleWordDivisionInfo> DataList)
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
    }
}

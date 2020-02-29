using Dapper;
using DBRepertory;
using DBModels;

using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Threading;

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
            ConnStr = "Data Source=JiannyWu;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian123";
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
	)").ToList<ArticleInfo>();
                conn.Close();
            }
            HashList.ForEach(x =>
            {
                SplitSentence(x.HashCode, x.DataTitle, "Title");
                SplitSentence(x.HashCode, x.DataContent, "Content");
            });
            ArticleWordDivisionDal dal = new ArticleWordDivisionDal();
            dal.SaveList(dic.Values.ToList());
            LanguageComparisonDal LCdal = new LanguageComparisonDal();
            LCdal.UpdateWordNum();//更新单词出现次数
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
                    if (Regex.Replace(word, "[^a-z|A-Z]+", "", RegexOptions.IgnoreCase | RegexOptions.Singleline).Length * 100 / word.Length < 80)
                    {
                        continue;//英文字母量 少于80%， 不符合单词规则
                    }
                }
                if (string.IsNullOrEmpty(word))
                {
                    continue;
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
            Thread.Sleep(1000);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using System.Security.Cryptography;
using System.Xml.XPath;
using System.Data.SqlClient;
using Dapper;
using Models;
using PublicUnit;

namespace Downloader
{
    /// <summary>
    /// 环球时报
    /// </summary>
    public class GlobalTimes
    {
        private string RootUrl = string.Empty;
        private string RootAddress = string.Empty;
        HttpRequestFactory httpFactory = null;
        string ConnStr = string.Empty;
        List<string> FileList = null;
        public GlobalTimes()
        {
            RootUrl = "http://www.globaltimes.cn/";
            RootAddress = "F:\\HookNetWork\\" + DateTime.Now.ToString("yyyyMMdd") + "\\GlobalTimes\\";
            ConnStr = "Data Source=DESKTOP-WUTENGJ;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian123";
            httpFactory = new HttpRequestFactory();
            FileList = new List<string>();
            DirectoryInfo folder = new DirectoryInfo(RootAddress);
            foreach (FileInfo file in folder.GetFiles("*.html"))
            {
                FileList.Add(file.FullName);
            }
        }
        public void Run()
        {
            //Download();
            ExtractDetails();
        }
        public void Download()
        {
            Dictionary<string, string> TypeDic = new Dictionary<string, string>();
            TypeDic.Add("indexchina", RootUrl + "includes/indexchina.html");
            TypeDic.Add("indexbusiness", RootUrl + "includes/indexbusiness.html");
            TypeDic.Add("indexworld", RootUrl + "includes/indexworld.html");
            TypeDic.Add("indexarts", RootUrl + "includes/indexarts.html");
            foreach (string key in TypeDic.Keys)
            {
                //Task.Run(() =>
                //{
                string httpContent = httpFactory.http(TypeDic[key], "GET", null, null, Encoding.UTF8, null);
                foreach (Match infoMatch in Regex.Matches(httpContent, "<li class=\"(nav-home|dropdown)\">\\s*<a href=\"(?<url>[^<>\"]*?)\">(?<info>[^<>]*?)</a>\\s*</li>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    DownloadList(infoMatch.Groups["url"].Value, infoMatch.Groups["info"].Value);
                }
                // });
            }
        }
        public void DownloadList(string listurl, string KeyWordSort)
        {
            string url = listurl;
            string httpContent = httpFactory.http(url, "GET", null, null, Encoding.UTF8, null).Replace("&gt;", " ").Replace(">>", " ");
            string DetailsUrl = string.Empty;
            do
            {
                foreach (Match infoMatch in Regex.Matches(httpContent, "<div class=\"row-content\">(?<info>((?!</p|row-content).)*?</p>)", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    DetailsUrl = Regex.Match(infoMatch.Groups["info"].Value, "<a[^<>]*href=\"(?<info>[^<>\"]*)\">(?<title>[^<>]*)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                    if (FileList.Contains(RootAddress + GetHttpFileName(DetailsUrl, ".html")))
                        continue;
                    var dic = new Dictionary<string, string>();
                    dic.Add("title", Regex.Match(infoMatch.Groups["info"].Value, "<a[^<>]*href=\"(?<info>[^<>\"]*)\">(?<title>[^<>]*)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["title"].Value);
                    dic.Add("KeyWordSort", KeyWordSort);
                    DownloadDetails(DetailsUrl, dic);
                    Thread.Sleep(1000);
                }
                var page = Regex.Match(httpContent, "<a[^<>]*href=\"(?<info>[^<>\"]*)\">Next\\s*</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                if (string.IsNullOrEmpty(page))
                {
                    break;
                }
                url = listurl + page;
                httpContent = httpFactory.http(url, "GET", null, null, Encoding.UTF8, null).Replace("&gt;", " ");
            } while (true);
        }
        public void DownloadDetails(string url, Dictionary<string, string> dic)
        {
            string httpContent = httpFactory.http(url, "GET", null, null, Encoding.UTF8, null);
            if (string.IsNullOrEmpty(httpContent))
                return;
            httpContent = httpContent + "<Mytitle>" + dic["title"] + "</Mytitle>" + "<KeyWordSort>" + dic["KeyWordSort"] + "</KeyWordSort>";
            string FileName = GetHttpFileName(url, ".html");
            SavaFile(FileName, httpContent);
        }
        public void ExtractDetails()
        {
            List<ArticleInfo> ArticleList = new List<ArticleInfo>();
            DirectoryInfo folder = new DirectoryInfo(RootAddress);
            foreach (FileInfo file in folder.GetFiles("*.html"))
            {
                string FileContent = File.ReadAllText(file.FullName);
                string DataTitle = Regex.Match(FileContent, "<Mytitle>(?<info>[^<>]*)</Mytitle>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                string KeyWordSort = Regex.Match(FileContent, "<KeyWordSort>(?<info>[^<>]*)</KeyWordSort>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                string DataSourceLink = Regex.Match(FileContent, "<meta name=\"twitter:url\" content=\"(?<info>[^<>\"]*)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                string DataSource = Regex.Match(FileContent, "Source:(?<info>((?!Published|Last Updated).)*)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                DataSource = string.IsNullOrEmpty(DataSource) == false ? "GlobalTimes" : "GlobalTimes:" + DataSource;
                string ArticleTimeStr = Regex.Match(FileContent, "Last Updated:(?<info>((?!Published|Last Updated|</).)*)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                if (string.IsNullOrEmpty(ArticleTimeStr))
                {
                    ArticleTimeStr = Regex.Match(FileContent, "Published:(?<info>((?!Published|Last Updated|</).)*)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                }
                string DataType = "新闻";
                FileContent = Regex.Match(FileContent, "<div class=\"[^<>]*row-content\">(?<info>((?!<div|</div).)*)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                FileContent = Regex.Replace(FileContent, "(<[^<>]*>|&nbsp;)", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                FileContent = Regex.Replace(FileContent, "\\r", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                FileContent = Regex.Replace(FileContent, "\\s{2,}", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                FileContent = FileContent.Replace('\'', '’');
                string HashCode = MD5Encrypt32(DataSourceLink + DataTitle);
                DateTime ArticleTime = DateTime.Now;
                if (string.IsNullOrEmpty(ArticleTimeStr) == false)
                {
                    ArticleTime = Convert.ToDateTime(ArticleTimeStr);
                }
                ArticleList.Add(new ArticleInfo()
                {
                    HashCode = HashCode,
                    CreateTime = DateTime.Now,
                    DataTitle = DataTitle,
                    DataContent = FileContent,
                    DataType = DataType,
                    KeyWordSort = KeyWordSort,
                    DataSource = DataSource,
                    DataSourceLink = DataSourceLink,
                    ArticleTime = ArticleTime
                });
            }
            SaveToSQL(ArticleList);
        }
        public void SaveToSQL(List<ArticleInfo> ArticleList)
        {
            List<string> HashList = new List<string>();
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                HashList = conn.Query<string>("select HashCode from [dbo].[Article] with(nolock)").ToList<string>();
                conn.Close();
            }
            ArticleList = ArticleList.Where(x => HashList.Contains(x.HashCode) == false).ToList();
            var data = SqlServerBulkCopy.ToDataTable<ArticleInfo>(ArticleList);
            Dictionary<string, string> SqlMapping = new Dictionary<string, string>();
            SqlMapping.Add("HashCode", "HashCode");
            SqlMapping.Add("CreateTime", "CreateTime");
            SqlMapping.Add("DataTitle", "DataTitle");
            SqlMapping.Add("DataContent", "DataContent");
            SqlMapping.Add("DataType", "DataType");
            SqlMapping.Add("KeyWordSort", "KeyWordSort");
            SqlMapping.Add("DataSource", "DataSource");
            SqlMapping.Add("DataSourceLink", "DataSourceLink");
            SqlMapping.Add("ArticleTime", "ArticleTime");
            SqlServerBulkCopy.SqlBulkMapping(SqlMapping);
            SqlServerBulkCopy.ConnStr = ConnStr;
            SqlServerBulkCopy.SqlBulkCopyToServer(data, "Article");
        }
        public void SavaFile(string FileName, string Content)
        {
            //判断路径是否存在,不存在就创建
            if (!Directory.Exists(RootAddress))
                Directory.CreateDirectory(RootAddress);
            String FilePath = RootAddress + FileName;
            //文件覆盖方式添加内容
            StreamWriter file = new StreamWriter(FilePath, false);
            file.Write(Content);
            file.Close();
            file.Dispose();
        }
        public string GetHttpFileName(string url, string FileType)
        {
            return Regex.Replace(url, "(\\:|\\/|\\.|\\?|\\&)+", "_", RegexOptions.IgnoreCase | RegexOptions.Singleline) + FileType;
        }
        public string MD5Encrypt32(string Content)
        {
            string pwd = "";
            MD5 md5 = MD5.Create(); //实例化一个md5对像  // 加密后是一个字节类型的数组，这里要注意编码UTF8/Unicode等的选择　
            byte[] s = md5.ComputeHash(Encoding.UTF8.GetBytes(Content));
            // 通过使用循环，将字节类型的数组转换为字符串，此字符串是常规字符格式化所得
            for (int i = 0; i < s.Length; i++)
            {
                // 将得到的字符串使用十六进制类型格式。格式后的字符是小写的字母，如果使用大写（X）则格式后的字符是大写字符 
                pwd = pwd + s[i].ToString("X");
            }
            return pwd;
        }
    }
}

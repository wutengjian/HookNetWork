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
using DBModels;
using BasicExpansion;
using DBRepertory;

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
        List<string> FileList = null;
        public GlobalTimes()
        {
            RootUrl = "http://www.globaltimes.cn/";
            RootAddress = "F:\\HookNetWork\\GlobalTimes\\";
            httpFactory = new HttpRequestFactory();
            FileList = new List<string>();
            DirectoryInfo folder = new DirectoryInfo(RootAddress);
            if (folder.Exists)
            {
                foreach (FileInfo file in folder.GetFiles("*.html"))
                {
                    FileList.Add(file.FullName);
                }
            }
        }
        public void Run()
        {
            ExtractDetails();
            Download();
            ExtractDetails();
        }
        public void Download()
        {
            Console.WriteLine("Downloader =》GlobalTimes>开始下载 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            Dictionary<string, string> TypeDic = new Dictionary<string, string>();
            TypeDic.Add("indexchina", RootUrl + "includes/indexchina.html");
            TypeDic.Add("indexbusiness", RootUrl + "includes/indexbusiness.html");
            TypeDic.Add("indexworld", RootUrl + "includes/indexworld.html");
            TypeDic.Add("indexarts", RootUrl + "includes/indexarts.html");
            foreach (string key in TypeDic.Keys)
            {
                string httpContent = httpFactory.http(TypeDic[key], "GET", null, null, Encoding.UTF8, null);
                foreach (Match infoMatch in Regex.Matches(httpContent, "<li class=\"(nav-home|dropdown)\">\\s*<a href=\"(?<url>[^<>\"]*?)\">(?<info>[^<>]*?)</a>\\s*</li>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    DownloadList(infoMatch.Groups["url"].Value, infoMatch.Groups["info"].Value);
                }
            }
            Console.WriteLine("Downloader =》GlobalTimes>下载完成 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
        public void DownloadList(string listurl, string KeyWordSort)
        {
            string url = listurl;
            string httpContent = httpFactory.http(url, "GET", null, null, Encoding.UTF8, null).Replace("&gt;", " ").Replace(">>", " ");
            string DetailsUrl = string.Empty;
            int maxPage = 0;
            do
            {
                foreach (Match infoMatch in Regex.Matches(httpContent, "<div class=\"row-content\">(?<info>((?!</p|row-content).)*?</p>)", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    DetailsUrl = Regex.Match(infoMatch.Groups["info"].Value, "<a[^<>]*href=\"(?<url>[^<>\"]*)\">(?<title>[^<>]*)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["url"].Value;
                    if (FileList.Contains(RootAddress + FileHelper.GetHttpFileName(DetailsUrl, ".html")))
                        continue;
                    var dic = new Dictionary<string, string>();
                    dic.Add("title", Regex.Match(infoMatch.Groups["info"].Value, "<a[^<>]*href=\"(?<info>[^<>\"]*)\">(?<title>[^<>]*)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["title"].Value);
                    dic.Add("KeyWordSort", KeyWordSort);
                    DownloadDetails(DetailsUrl, dic);
                    Thread.Sleep(1000);
                }
                var page = Regex.Match(httpContent, "<a[^<>]*href=\"(?<info>[^<>\"]*)\">Next\\s*</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                maxPage++;
                if (string.IsNullOrEmpty(page) || maxPage > 100)
                {
                    break;
                }
                Console.WriteLine("GlobalTimes =》DownloadList：" + maxPage + " @" + DateTime.Now.ToString("HH:mm:ss:fff"));
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
            string FileName = FileHelper.GetHttpFileName(url, ".html");
            FileHelper.SavaFile(RootAddress, FileName, httpContent);
        }
        public void ExtractDetails()
        {
            Console.WriteLine("Downloader =》GlobalTimes>解析 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            List<ArticleInfo> ArticleList = new List<ArticleInfo>();
            ArticleDal dal = new ArticleDal();
            List<ArticleFileInfo> ListSQLite = new List<ArticleFileInfo>();
            ArticleFileSQLite SQLitedal = new ArticleFileSQLite();
            var _AFMList = SQLitedal.GetList("GlobalTimes");
            SQLitedal.AFMList = _AFMList;
            DirectoryInfo folder = new DirectoryInfo(RootAddress);
            int Num = 0;
            foreach (FileInfo file in folder.GetFiles("*.html"))
            {
                string FileContent = File.ReadAllText(file.FullName);
                string fileDate = file.CreationTimeUtc.ToString("yyyy-MM-dd");
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
                FileContent = FileContent.HtmlReplace();
                string HashCode = FileHelper.MD5Encrypt32(DataSourceLink + DataTitle);
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
                ListSQLite.Add(new ArticleFileInfo()
                {
                    HashCode = HashCode,
                    HttpUrl = DataSourceLink,
                    FileName = file.FullName,
                    DataSource = DataSource,
                    DataTime = Convert.ToDateTime(fileDate)
                });
                Num++;
                if (Num > 1000)
                {
                    dal.SaveList(ArticleList);
                    SQLitedal.SaveList(ListSQLite);
                    ArticleList = new List<ArticleInfo>();
                    ListSQLite = new List<ArticleFileInfo>();
                    Num = 0;
                    Thread.Sleep(1000 * 3);
                }
            }
            dal.SaveList(ArticleList);
        }
    }
}

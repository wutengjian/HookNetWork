using DBRepertory;
using Models;
using PublicUnit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace Downloader
{
    /// <summary>
    /// 新闻周刊
    /// </summary>
    public class NewsWeek
    {
        private string RootUrl = string.Empty;
        private string RootAddress = string.Empty;
        HttpRequestFactory httpFactory = null;
        string _headers = string.Empty;
        List<string> FileList = null;
        public NewsWeek()
        {
            RootUrl = "https://www.newsweek.com";
            RootAddress = "F:\\HookNetWork\\NewsWeek\\";
            httpFactory = new HttpRequestFactory(true);
            _headers = @"Accept:text/html, application/xhtml+xml, image/jxr, */* 
                        Accept-Encoding:gzip, deflate 
                        Accept-Language:zh-CN 
                        Host:www.newsweek.com 
                        If-Modified-Since:Sat, 06 Oct 2018 15:18:06 GMT 
                        User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240";
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
            Download();
            ExtractDetails();
        }
        public void Download()
        {
            Console.WriteLine("Downloader =》NewsWeek>开始下载 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            Dictionary<string, string> TypeDic = new Dictionary<string, string>();
            TypeDic.Add("us", RootUrl + "/us");
            TypeDic.Add("world", RootUrl + "/world");
            TypeDic.Add("business", RootUrl + "/business");
            TypeDic.Add("tech-science", RootUrl + "/tech-science");
            TypeDic.Add("culture", RootUrl + "/culture");
            TypeDic.Add("sports", RootUrl + "/sports");
            TypeDic.Add("health", RootUrl + "/health");
            TypeDic.Add("opinion", RootUrl + "/opinion");
            httpFactory.http(RootUrl + "/", "GET", _headers, null, Encoding.UTF8, null);
            foreach (string key in TypeDic.Keys)
            {
                //Task.Run(() =>
                //{
                DownloadList(TypeDic[key], key);
                // });
            }
            Console.WriteLine("Downloader =》NewsWeek>下载完成 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
        public void DownloadList(string listurl, string KeyWordSort)
        {
            string url = listurl;
            string httpContent = httpFactory.http(url, "GET", _headers, null, Encoding.UTF8, null).Replace("&gt;", " ").Replace(">>", " ");
            string DetailsUrl = string.Empty;
            int maxPage = 0;
            try
            {
                do
                {
                    foreach (Match infoMatch in Regex.Matches(httpContent, "<article class=\"flex-sm\">(?<info>((?!<article|</article).)*</article>)", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                    {
                        string info = infoMatch.Groups["info"].Value;
                        DetailsUrl = Regex.Match(info, "<a href=\"(?<url>[^<>\"]*)\">(?<title>[^<>]*)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["url"].Value;
                        if (string.IsNullOrEmpty(DetailsUrl) == false && DetailsUrl.Contains("http") == false)
                        {
                            DetailsUrl = RootUrl + DetailsUrl;
                        }
                        if (FileList.Contains(RootAddress + FileHelper.GetHttpFileName(DetailsUrl, ".html")))
                            continue;
                        var dic = new Dictionary<string, string>();
                        dic.Add("title", Regex.Match(info, "<a href=\"(?<url>[^<>\"]*)\">(?<title>[^<>]*)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["title"].Value);
                        dic.Add("KeyWordSort", KeyWordSort);
                        DownloadDetails(DetailsUrl, dic);
                        Thread.Sleep(1000 * 3);
                    }
                    var page = Regex.Match(httpContent, "<a href=\"(?<info>[^<>\"]*)\">Next</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                    maxPage++;
                    if (string.IsNullOrEmpty(page) || maxPage > 100)
                    {
                        break;
                    }
                    Console.WriteLine("NewsWeek  =》DownloadList：" + maxPage+" @" + DateTime.Now.ToString("HH:mm:ss:fff"));
                    Thread.Sleep(1000 * 30);
                    url = RootUrl + page;
                    httpContent = httpFactory.http(url, "GET", _headers, null, Encoding.UTF8, null).Replace("&gt;", " ");
                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Downloader =》NewsWeek>下载列表异常 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            }
        }
        public void DownloadDetails(string url, Dictionary<string, string> dic)
        {
            try
            {
                string httpContent = httpFactory.http(url, "GET", _headers, null, Encoding.UTF8, null);
                if (string.IsNullOrEmpty(httpContent))
                    return;
                httpContent = httpContent + "<Mytitle>" + dic["title"] + "</Mytitle>" + "<KeyWordSort>" + dic["KeyWordSort"] + "</KeyWordSort>" + "<MyUrl>" + url + "</MyUrl>";
                string FileName = FileHelper.GetHttpFileName(url, ".html");
                FileHelper.SavaFile(RootAddress, FileName, httpContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Downloader =》NewsWeek>下载详情异常 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            }

        }
        public void ExtractDetails()
        {
            Console.WriteLine("Downloader =》NewsWeek>解析 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            List<ArticleInfo> ArticleList = new List<ArticleInfo>();
            DirectoryInfo folder = new DirectoryInfo(RootAddress);
            foreach (FileInfo file in folder.GetFiles("*.html"))
            {
                string FileContent = File.ReadAllText(file.FullName);
                string DataTitle = Regex.Match(FileContent, "<Mytitle>(?<info>[^<>]*)</Mytitle>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                string KeyWordSort = Regex.Match(FileContent, "<KeyWordSort>(?<info>[^<>]*)</KeyWordSort>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                string DataSourceLink = Regex.Match(FileContent, "<MyUrl>(?<info>[^<>]*)</MyUrl>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;

                string title = Regex.Match(FileContent, "<h[^<>]*class=\"title\">(?<info>[^<>]*)</h", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                DataTitle = string.IsNullOrEmpty(title) ? DataTitle : title;
                string DataSource = "NewsWeek";
                string ArticleTimeStr = Regex.Match(FileContent, "<time itemprop=\"dateModified\" datetime=\"(?<info>[^<>\"]*)\">", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                if (string.IsNullOrEmpty(ArticleTimeStr))
                {
                    ArticleTimeStr = Regex.Match(FileContent, "<time itemprop=\"datePublished\" datetime=\"(?<info>[^<>\"]*)\">", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                }
                string DataType = "新闻";
                string content = Regex.Match(FileContent, "<div[^<>]*itemprop=\"articleBody\">(?<info>((?!<div|</div).)*)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                if (string.IsNullOrEmpty(content))
                {
                    content = Regex.Match(FileContent, "<div[^<>]*class=\"article-body\"[^<>]*>(?<info>((?!<div|</div).)*)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                }
                content = content.HtmlReplace();
                string HashCode = FileHelper.MD5Encrypt32(DataSourceLink + DataTitle);
                DateTime ArticleTime = DateTime.Now;
                if (string.IsNullOrEmpty(ArticleTimeStr) == false)
                {
                    ArticleTime = Convert.ToDateTime(ArticleTimeStr.Replace("T", " "));
                }
                ArticleList.Add(new ArticleInfo()
                {
                    HashCode = HashCode,
                    CreateTime = DateTime.Now,
                    DataTitle = DataTitle,
                    DataContent = content,
                    DataType = DataType,
                    KeyWordSort = KeyWordSort,
                    DataSource = DataSource,
                    DataSourceLink = DataSourceLink,
                    ArticleTime = ArticleTime
                });
            }
            ArticleDal dal = new ArticleDal();
            dal.SaveList(ArticleList);
        }
    }
}

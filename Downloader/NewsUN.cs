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
    /// 联合国新闻部
    /// </summary>
    public class NewsUN
    {
        private string RootUrl = string.Empty;
        private string RootAddress = string.Empty;
        HttpRequestFactory httpFactory = null;
        string _headers = string.Empty;
        List<string> FileList = null;
        public NewsUN()
        {
            RootUrl = "https://news.un.org";
            RootAddress = "F:\\HookNetWork\\NewsUN\\";
            httpFactory = new HttpRequestFactory(true);
            _headers = @"Accept: text/html, application/xhtml+xml, image/jxr, */*
                         Accept-Encoding: gzip, deflate
                         Accept-Language: zh-CN
                         Connection: Keep-Alive
                         Host: news.un.org
                         If-Modified-Since: Sun, 07 Oct 2018 03:58:13 GMT
                         User-Agent: Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240";
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
            Console.WriteLine("Downloader =》NewsUN>开始下载 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            Dictionary<string, string> TypeDic = new Dictionary<string, string>();
            TypeDic.Add("Africa", RootUrl + "/en/news/region/africa");
            TypeDic.Add("Americas", RootUrl + "/en/news/region/americas");
            TypeDic.Add("Asia Pacific", RootUrl + "/en/news/region/asia-pacific");
            TypeDic.Add("Climate Change ", RootUrl + "/en/news/topic/climate-change");
            TypeDic.Add("Culture and Education", RootUrl + "/en/news/topic/culture-and-education");
            TypeDic.Add("Economic Development", RootUrl + "/en/news/topic/economic-development");
            TypeDic.Add("Europe", RootUrl + "/en/news/region/europe");
            TypeDic.Add("Features", RootUrl + "/en/features");
            TypeDic.Add("Health", RootUrl + "/en/news/topic/health");
            TypeDic.Add("Human Rights", RootUrl + "/en/news/topic/human-rights");
            TypeDic.Add("Humanitarian Aid", RootUrl + "/en/news/topic/humanitarian-aid");
            TypeDic.Add("ICYMI", RootUrl + "/en/ICYMI/");
            TypeDic.Add("Interviews", RootUrl + "/en/interviews");
            TypeDic.Add("Law and Crime Prevention", RootUrl + "/en/news/topic/law-and-crime-prevention");
            TypeDic.Add("Middle East", RootUrl + "/en/news/region/middle-east");
            TypeDic.Add("Migrants and Refugees", RootUrl + "/en/news/topic/migrants-and-refugees");
            TypeDic.Add("News in Brief", RootUrl + "/en/audio-product/news-brief");
            TypeDic.Add("Peace and Security", RootUrl + "/en/news/topic/peace-and-security");
            TypeDic.Add("Photo Stories", RootUrl + "/en/gallery");
            TypeDic.Add("Podcast Classics", RootUrl + "/en/audio-product/podcast-classics");
            TypeDic.Add("SDGs", RootUrl + "/en/news/topic/sdgs");
            TypeDic.Add("The Lid is On", RootUrl + "/en/audio-product/lid");
            TypeDic.Add("UN Affairs", RootUrl + "/en/news/topic/un-affairs");
            TypeDic.Add("UN and Africa", RootUrl + "/en/audio-product/un-and-africa");
            TypeDic.Add("UN Gender Focus", RootUrl + "/en/audio-product/focus-gender");
            TypeDic.Add("Women", RootUrl + "/en/news/topic/women");

            httpFactory.http(RootUrl + "/en/", "GET", _headers, null, Encoding.UTF8, null);
            foreach (string key in TypeDic.Keys)
            {
                //Task.Run(() =>
                //{
                DownloadList(TypeDic[key], key);
                // });
            }
            Console.WriteLine("Downloader =》NewsUN>下载完成 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
        public void DownloadList(string listurl, string KeyWordSort)
        {
            string url = listurl;
            string httpContent = httpFactory.http(url, "GET", _headers, null, Encoding.UTF8, null).Replace("&gt;", " ").Replace(">>", " ");
            string DetailsUrl = string.Empty;
            int maxPage = 0;
            do
            {
                string Content = Regex.Match(httpContent, "<div class=\"view-content\">(?<info>((?!text-center|pagination).)*)<div class=\"text-center", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                foreach (Match infoMatch in Regex.Matches(Content, "<h\\d+ class=\"story-title\"><a href=\"(?<url>[^<>\"]*)\">(?<title>[^<>]*)</a>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    DetailsUrl = infoMatch.Groups["url"].Value;
                    if (string.IsNullOrEmpty(DetailsUrl) == false && DetailsUrl.Contains("http") == false)
                    {
                        DetailsUrl = RootUrl + DetailsUrl;
                    }
                    if (FileList.Contains(RootAddress + FileHelper.GetHttpFileName(DetailsUrl, ".html")))
                        continue;
                    var dic = new Dictionary<string, string>();
                    dic.Add("title", infoMatch.Groups["title"].Value);
                    dic.Add("KeyWordSort", KeyWordSort);
                    DownloadDetails(DetailsUrl, dic);
                    Thread.Sleep(1000 * 5);
                }
                var page = Regex.Match(httpContent, "<a[^<>]*href=\"(?<info>[^<>\"]*)\">next", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                maxPage++;
                if (string.IsNullOrEmpty(page) || maxPage > 100)
                {
                    break;
                }
                Console.WriteLine("NewsUN  =》DownloadList：" + maxPage + " @" + DateTime.Now.ToString("HH:mm:ss:fff"));
                Thread.Sleep(1000 * 30);
                url = RootUrl + page;
                httpContent = httpFactory.http(url, "GET", _headers, null, Encoding.UTF8, null).Replace("&gt;", " ");
            } while (true);
        }
        public void DownloadDetails(string url, Dictionary<string, string> dic)
        {
            string httpContent = httpFactory.http(url, "GET", _headers, null, Encoding.UTF8, null);
            if (string.IsNullOrEmpty(httpContent))
                return;
            httpContent = httpContent + "<Mytitle>" + dic["title"] + "</Mytitle>" + "<KeyWordSort>" + dic["KeyWordSort"] + "</KeyWordSort>" + "<MyUrl>" + url + "</MyUrl>";
            string FileName = FileHelper.GetHttpFileName(url, ".html");
            FileHelper.SavaFile(RootAddress, FileName, httpContent);
        }
        public void ExtractDetails()
        {
            Console.WriteLine("Downloader =》NewsUN>解析 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
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
                string DataSource = "NewsUN";
                string ArticleTimeStr = Regex.Match(FileContent, "<span class=\"date-display-single\"[^<>]*content=\"(?<info>[^<>\"]*)", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                string DataType = "新闻";
                string content = string.Empty;
                foreach (Match InfoMatch in Regex.Matches(FileContent, "<div class=\"field-items\"><div class=\"field-item even\"><p>(?<info>((?!<div|</div).)*)", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                {
                    string info = InfoMatch.Groups["info"].Value;
                    content += info.HtmlReplace() + " ";
                }
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

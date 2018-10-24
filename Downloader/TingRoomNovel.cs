using DBRepertory;
using DBModels;
using BasicExpansion;
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
    public class TingRoomNovel : DownloadBase
    {

        public TingRoomNovel()
        {
            RootUrl = "http://novel.tingroom.com";
            RootAddress = "F:\\HookNetWork\\TingRoomNovel\\";
            httpFactory = new HttpRequestFactory(true);
            _headers = @"Accept:text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8 
                        Accept-Encoding:gzip, deflate 
                        Accept-Language:zh-CN 
                        Host:novel.tingroom.com 
                        If-Modified-Since:Sat, 06 Oct 2018 15:18:06 GMT 
                        User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240";
            ArticleFileSQLite SQLitedal = new ArticleFileSQLite();
            FileList = SQLitedal.GetFileNames("TingRoomNovel");
        }
        public override void Run()
        {
            ExtractDetails();
            Download();
            ExtractDetails();
        }
        public void Download()
        {
            Console.WriteLine("Downloader =》TingRoomNovel>开始下载 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            Dictionary<string, string> TypeDic = new Dictionary<string, string>();
            TypeDic.Add("经典小说", RootUrl + "/jingdian/");
            TypeDic.Add("名人传记", RootUrl + "/mingren/");
            TypeDic.Add("励志小说", RootUrl + "/lizhi/");
            TypeDic.Add("短篇小说", RootUrl + "/duanpian/");
            TypeDic.Add("儿童小说", RootUrl + "/ertong/");
            TypeDic.Add("科幻小说", RootUrl + "/kehuan/");
            TypeDic.Add("宗教小说", RootUrl + "/zongjiao/");
            //TypeDic.Add("双语小说", RootUrl + "/shuangyu/");

            httpFactory.http(RootUrl + "/", "GET", _headers, null, Encoding.UTF8, null);
            foreach (string key in TypeDic.Keys)
            {
                DownloadList(TypeDic[key], key);
            }
            Console.WriteLine("Downloader =》TingRoomNovel>下载完成 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
        public void DownloadList(string listurl, string KeyWordSort)
        {
            string url = listurl;
            string httpContent = httpFactory.http(url, "GET", _headers, null, Encoding.UTF8, null).Replace("&gt;", " ").Replace(">>", " ").Replace("&nbsp;", " ");
            string DetailsUrl = string.Empty;
            int maxPage = 0;
            try
            {
                do
                {
                    foreach (Match infoMatch in Regex.Matches(httpContent, "<div class=\"text\">(?<info>((?!<div|</div).)*)</div>", RegexOptions.IgnoreCase | RegexOptions.Singleline))
                    {
                        string info = infoMatch.Groups["info"].Value;
                        DetailsUrl = info.RegexMatch("<h\\d+ class=\"yuyu\">\\s*<a href=\"(?<url>[^<>\"]*)\" >(?<BookName>[^<>]*)</a>", "url");
                        string BookName = info.RegexMatch("<h\\d+ class=\"yuyu\">\\s*<a href=\"(?<url>[^<>\"]*)\" >(?<BookName>[^<>]*)</a>", "BookName");
                        if (string.IsNullOrEmpty(DetailsUrl) == false && DetailsUrl.Contains("http") == false)
                        {
                            DetailsUrl = RootUrl + DetailsUrl;
                        }
                        if (FileList!=null&&FileList.Contains(RootAddress + FileHelper.GetHttpFileName(DetailsUrl, ".txt")))
                            continue;
                        var dic = new Dictionary<string, string>();
                        dic.Add("BookName", BookName);
                        dic.Add("title", info.RegexMatch("<p class=\"intro11\"><span>简介：</span>(?<info>[^<>]*)</p>", "info"));
                        dic.Add("KeyWordSort", KeyWordSort);
                        DownloadDetails(DetailsUrl, dic);
                        Thread.Sleep(1000 * 3);
                    }
                    var page = Regex.Match(httpContent, "<a href=\"(?<info>[^<>\"]*)\">\\s*下一页", RegexOptions.IgnoreCase | RegexOptions.Singleline).Groups["info"].Value;
                    maxPage++;
                    if (string.IsNullOrEmpty(page) || maxPage > 20)
                    {
                        break;
                    }
                    Console.WriteLine("TingRoomNovel  =》DownloadList：" + maxPage + " @" + DateTime.Now.ToString("HH:mm:ss:fff"));
                    Thread.Sleep(1000 * 30);
                    url = RootUrl + page;
                    httpContent = httpFactory.http(url, "GET", _headers, null, Encoding.UTF8, null).Replace("&gt;", " ");
                } while (true);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Downloader =》TingRoomNovel>下载列表异常 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            }
        }
        public void DownloadDetails(string url, Dictionary<string, string> dic)
        {
            try
            {
                string aid = url.RegexMatch("/(?<info>\\d+)[/]*", "info");
                string urlDetails = "http://novel.tingroom.com/novel_down.php?aid=" + aid + "&dopost=txt";
                string httpContent = httpFactory.http(urlDetails, "GET", _headers, null, Encoding.UTF8, null);
                if (string.IsNullOrEmpty(httpContent))
                    return;
                httpContent = "<MyBody>" + httpContent + "</MyBody>"
                    + "<Mytitle>" + dic["title"] + "</Mytitle>"
                    + "<KeyWordSort>" + dic["KeyWordSort"] + "</KeyWordSort>"
                    + "<MyBookName>" + dic["BookName"] + "</MyBookName>"
                    + "<MyUrl>" + url + "</MyUrl>";
                string FileName = FileHelper.GetHttpFileName(url, ".txt");
                FileHelper.SavaFile(RootAddress, FileName, httpContent);
            }
            catch (Exception ex)
            {
                Console.WriteLine("Downloader =》TingRoomNovel>下载详情异常 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            }

        }
        public void ExtractDetails()
        {
            Console.WriteLine("Downloader =》TingRoomNovel>解析 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            List<ArticleInfo> ArticleList = new List<ArticleInfo>();
            ArticleDal dal = new ArticleDal();
            List<ArticleFileInfo> ListSQLite = new List<ArticleFileInfo>();
            ArticleFileSQLite SQLitedal = new ArticleFileSQLite();
            var _AFMList = SQLitedal.GetList("TingRoomNovel");
            SQLitedal.AFMList = _AFMList;
            DirectoryInfo folder = new DirectoryInfo(RootAddress);
            int num = 0;
            foreach (FileInfo file in folder.GetFiles("*.txt"))
            {
                string FileContent = File.ReadAllText(file.FullName);
                string fileDate = file.CreationTimeUtc.ToString("yyyy-MM-dd");
                string DataTitle = FileContent.RegexMatch("<Mytitle>(?<info>[^<>]*)</Mytitle>", "info");
                string KeyWordSort = FileContent.RegexMatch("<KeyWordSort>(?<info>[^<>]*)</KeyWordSort>", "info");
                string DataSourceLink = FileContent.RegexMatch("<MyUrl>(?<info>[^<>]*)</MyUrl>", "info");
                string MyBookName = FileContent.RegexMatch("<MyBookName>(?<info>[^<>]*)</MyBookName>", "info");
                var list = File.ReadAllLines(file.FullName).ToList();
                StringBuilder _stringBuilder = new StringBuilder();
                foreach (string info in list)
                {
                    if (info.Trim() == "" || info.RegexIsMatch("(<MyBody>|<Mytitle>|<KeyWordSort>|<MyUrl>|<MyBookName>)"))
                    {
                        continue;
                    }
                    if (info.RegexIsMatch("\\[Pg\\s*\\d+\\]"))
                    {
                        continue;
                    }

                    string infoData = Regex.Replace(info, "(</Mytitle>|\")", "", RegexOptions.IgnoreCase | RegexOptions.Singleline).Trim();
                    if (infoData.Length < 50)
                    {
                        //有可能是章节名称,验证大写字母站的比例
                        int len = Regex.Replace(infoData, "[a-z]+", "", RegexOptions.Singleline).Length;
                        if (len > 0 && len * 100 / infoData.Length > 80)
                        {
                            continue;
                        }
                    }
                    infoData = infoData.Replace('\'', '‘').Replace("&nbsp;", " ");
                    _stringBuilder.Append(infoData + "\r\n");
                }
                string HashCode = FileHelper.MD5Encrypt32(DataSourceLink + DataTitle);
                string DataSource = "TingRoomNovel";
                string DataType = "小说";
                DataTitle = Regex.Replace(DataTitle, "[\\u4e00-\\u9fa5]+", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline).Replace("&nbsp;", " ");
                string DataContent = Regex.Replace(_stringBuilder.ToString(), "[\\u4e00-\\u9fa5]+", " ", RegexOptions.IgnoreCase | RegexOptions.Singleline);
                ArticleList.Add(new ArticleInfo()
                {
                    HashCode = HashCode,
                    CreateTime = DateTime.Now,
                    DataTitle = DataTitle,
                    DataContent = DataContent,
                    DataType = DataType,
                    KeyWordSort = KeyWordSort,
                    DataSource = DataSource,
                    DataSourceLink = DataSourceLink,
                    ArticleTime = DateTime.Now
                });
                ListSQLite.Add(new ArticleFileInfo()
                {
                    HashCode = HashCode,
                    HttpUrl = DataSourceLink,
                    FileName = file.FullName,
                    DataSource = DataSource,
                    DataTime = Convert.ToDateTime(fileDate)
                });
                num++;
                if (ArticleList.Count % 100 == 0)
                {
                    Thread.Sleep(1000 * 3);
                    dal.SaveList(ArticleList);
                    SQLitedal.SaveList(ListSQLite);
                    ArticleList = new List<ArticleInfo>();
                    ListSQLite = new List<ArticleFileInfo>();
                }
            }
            dal.InsertBulk(ArticleList);
        }
    }
}

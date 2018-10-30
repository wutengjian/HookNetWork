using DBRepertory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downloader
{
    public class IYuba : DownloadBase
    {
        public IYuba()
        {
            RootUrl = "http://news.iyuba.com/";
            RootAddress = "F:\\HookNetWork\\NewsWeek\\";
            httpFactory = new HttpRequestFactory(true);
            _headers = @"Accept:text/html, application/xhtml+xml, image/jxr, */* 
                        Accept-Encoding:gzip, deflate 
                        Accept-Language:zh-CN 
                        Host:www.newsweek.com 
                        If-Modified-Since:Sat, 06 Oct 2018 15:18:06 GMT 
                        User-Agent:Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/42.0.2311.135 Safari/537.36 Edge/12.10240";
            ArticleFileSQLite SQLitedal = new ArticleFileSQLite();
            FileList = SQLitedal.GetFileNames("NewsWeek");
        }
        public override void Run()
        {
            throw new NotImplementedException();
        }
    }
}

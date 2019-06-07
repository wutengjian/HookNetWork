using DBRepertory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Downloader.SharesDownload
{
    public class EastMoney : DownloadBase
    {
        public override void Init()
        {
            RootUrl = "http://qt.gtimg.cn";
            RootAddress = "F:\\HookNetWork\\TencentQT\\";
            httpFactory = new HttpRequestFactory(true);
            _headers = @"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8
Accept-Encoding:  deflate
Accept-Language: zh-CN,zh;q=0.9
Cache-Control: max-age=0
Connection: keep-alive
Host: qt.gtimg.cn
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
            ArticleFileSQLite SQLitedal = new ArticleFileSQLite();
            FileList = SQLitedal.GetFileNames("TencentQT");
        }

        public override void Run()
        {
            base.DeleteFile(RootAddress);
            Download();
        }
        public void Download()
        {
            Console.WriteLine("Downloader =》NewsUN>开始下载 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            //http://nufm.dfcfw.com/EM_Finance2014NumericApplication/JS.aspx?cb=jQuery112408855351147337134_1559569049862&type=CT&token=4f1862fc3b5e77c150a2b985b12db0fd&sty=FCOIATC&js=(%7Bdata%3A%5B(x)%5D%2CrecordsFiltered%3A(tot)%7D)&cmd=C._A&st=(Code)&sr=1&p=1&ps=20&_=1559569049945
            Console.WriteLine("Downloader =》NewsUN>下载完成 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
        public void ExtractDetails() { }
    }
}

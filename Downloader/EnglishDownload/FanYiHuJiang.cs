
using BasicToolKit;
using DBModels;
using DBRepertory;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;

namespace Downloader.EnglishDownload
{
    public class FanYiHuJiang : DownloadBase
    {
        public override void Init()
        {
            RootUrl = "https://dict.hjenglish.com/v10/dict/translation/en/cn";
            RootAddress = "F:\\HookNetWork\\FanYiHuJiang\\";
            httpFactory = new HttpRequestFactory(true);
            _headers = @"Accept: */* 
                            Accept-Encoding: gzip, deflate, br 
                            Accept-Language: zh-CN,zh;q=0.9,en;q=0.8 
                            Connection: keep-alive 
                            Content-Length: 14 
                            Content-Type: application/x-www-form-urlencoded; charset=UTF-8 
                            Host: dict.hjenglish.com 
                            Origin: https://dict.hjenglish.com 
                            Referer: https://dict.hjenglish.com/app/trans 
                            User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36 
                            X-Requested-With: XMLHttpRequest
                            Cookie:HJ_UID=e9e31f62-281b-93fb-5e8b-bf8df4e19122;HJ_CMATCH=1;TRACKSITEMAP=3%2C;_REF=https%253A%252F%252Fwww.baidu.com;_UZT_USER_SET_114_0_DEFAULT=2|b8e023386af2eef0c36c93cd075f1b00;_SREF_3=https%253A%252F%252Fwww.baidu.com;HJ_UID=38449cc3-748f-4c9a-b9c9-ee93d6faa491";
            FileList = new List<string>();
            DirectoryInfo folder = new DirectoryInfo(RootAddress);
            if (folder.Exists)
            {
                foreach (FileInfo file in folder.GetFiles("*.txt"))
                {
                    FileList.Add(file.FullName);
                }
            }
        }

        public override void Run()
        {
            ExtractDetails();
            Download();
            ExtractDetails();
        }
        public void Download()
        {
            Console.WriteLine("Downloader =》FanYiHuJiang>开始下载 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            Dictionary<string, string> TypeDic = new Dictionary<string, string>();
            ArticleWordDivisionDal dal = new ArticleWordDivisionDal();
            List<string> list = dal.GetListWord();
            foreach (string word in list)
            {
                string q = word.Trim();
                if (string.IsNullOrEmpty(q))
                    continue;
                if (FileList.Contains(RootAddress + q + ".txt"))
                {
                    continue;
                }
                string Postcontent = "content=" + q;
                string httpContent = httpFactory.http(RootUrl, "POST", _headers, Postcontent, Encoding.UTF8, null);
                Thread.Sleep(500);
                FileHelper.SavaFile(RootAddress, q + ".txt", httpContent);
                Console.WriteLine("FanYiHuJiang》Download：" + word);
            }
            Console.WriteLine("Downloader =》FanYiHuJiang>下载完成 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }

        public void ExtractDetails()
        {
            Console.WriteLine("Downloader =》FanYiHuJiang>解析 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            DirectoryInfo folder = new DirectoryInfo(RootAddress);
            List<LanguageComparisonInfo> list = new List<LanguageComparisonInfo>();

            int Num = 0;
            foreach (FileInfo file in folder.GetFiles("*.txt"))
            {
                string FileContent = File.ReadAllText(file.FullName);
                string Translation = FileContent.RegexMatch("content\":\"(?<info>[^<>:\",]*)", "info");
                string Original = FileContent.RegexMatch("original_text\":\"(?<info>[^<>:\",]*)", "info");
                if (string.IsNullOrEmpty(Translation))
                {
                    continue;
                }
                list.Add(new LanguageComparisonInfo()
                {
                    CreateTime = DateTime.Now,
                    DataState = 1,
                    DataType = "word",
                    OriginalLang = "en",
                    OriginalText = Original,
                    Translation = Translation,
                    TranslationLang = "zh",
                    WordNum = 0
                });
                Num++;
                if (Num > 1000)
                {
                    Num = 0;
                    Thread.Sleep(1000 * 3);
                }
            }
            LanguageComparisonDal dal = new LanguageComparisonDal();
            dal.SaveList(list);
            dal.UpdateWordNum();
        }
    }
}

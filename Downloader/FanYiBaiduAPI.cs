using DBRepertory;
using DBModels;
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
    public class FanYiBaiduAPI
    {
        private string RootUrl = string.Empty;
        private string RootAddress = string.Empty;
        HttpRequestFactory httpFactory = null;
        List<string> FileList = null;
        public FanYiBaiduAPI()
        {
            RootUrl = "http://api.fanyi.baidu.com/api/trans/vip/translate";
            RootAddress = "F:\\HookNetWork\\FanYiBaiduAPI\\";
            httpFactory = new HttpRequestFactory(true);

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
        public void Run()
        {
            Download();
            ExtractDetails();
        }
        public void Download()
        {
            Console.WriteLine("Downloader =》FanYiBaiduAPI>开始下载 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            Dictionary<string, string> TypeDic = new Dictionary<string, string>();
            ArticleWordDivisionDal dal = new ArticleWordDivisionDal();
            List<string> list = dal.GetListWord();
            List<string> listOld = GetWordExists();
            list = list.Where(x => listOld.Contains(x) == false).Take(10000).ToList();

            string appid = "20181007000215943";
            string key = "FUECNM4FlsQI53YBvz6o";
            Random random = new Random();
            foreach (string word in list)
            {
                string q = word.Trim();
                if (string.IsNullOrEmpty(q))
                    continue;
                string salt = random.Next(10000).ToString();
                StringBuilder parm = new StringBuilder();
                parm.AppendFormat("?q={0}", q);
                parm.AppendFormat("&from={0}", "en");
                parm.AppendFormat("&to={0}", "zh");
                parm.AppendFormat("&appid={0}", appid);
                parm.AppendFormat("&salt={0}", salt);
                parm.AppendFormat("&sign={0}", FileHelper.MD5Encrypt32(appid + q + salt + key).ToLower());
                string httpContent = httpFactory.http(RootUrl + parm.ToString(), "GET", null, null, Encoding.UTF8, null);
                Thread.Sleep(100);
                FileHelper.SavaFile(RootAddress, word + ".txt", httpContent);
            }
            Console.WriteLine("Downloader =》FanYiBaiduAPI>下载完成 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
        public void ExtractDetails()
        {
            Console.WriteLine("Downloader =》FanYiBaiduAPI>解析 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            DirectoryInfo folder = new DirectoryInfo(RootAddress);
            List<LanguageComparisonInfo> list = new List<LanguageComparisonInfo>();
            foreach (FileInfo file in folder.GetFiles("*.txt"))
            {
                string FileContent = File.ReadAllText(file.FullName);
                string error_code = FileContent.RegexMatch("\"error_code\":[\"]*(?<info>[^<>\":,]*)[\"]*", "info");
                string error_msg = FileContent.RegexMatch("\"error_msg\":[\"]*(?<info>[^<>\":,]*)[\"]*", "info");
                string from = FileContent.RegexMatch("\"from\":[\"]*(?<info>[^<>\":,]*)[\"]*", "info");
                string to = FileContent.RegexMatch("\"to\":[\"]*(?<info>[^<>\":,]*)[\"]*", "info");
                string src = FileContent.RegexMatch("\"src\":[\"]*(?<info>[^<>\":,]*)[\"]*", "info");
                string dst = FileContent.RegexMatch("\"dst\":[\"]*(?<info>[^<>\":,]*)[\"]*", "info");
                if (string.IsNullOrEmpty(error_code) == false)
                    continue;
                dst = dst.AsciiToString();
                list.Add(new LanguageComparisonInfo()
                {
                    CreateTime = DateTime.Now,
                    DataState = 1,
                    DataType = "word",
                    OriginalLang = from,
                    OriginalText = src,
                    Translation = dst,
                    TranslationLang = to,
                    WordNum = 0
                });
            }
            LanguageComparisonDal dal = new LanguageComparisonDal();
            dal.SaveList(list);
            dal.UpdateWordNum();
        }
        public List<string> GetWordExists()
        {
            List<string> list = new List<string>();
            string filename = string.Empty;
            foreach (FileInfo file in new DirectoryInfo(RootAddress).GetFiles("*.txt"))
            {
                string FileContent = File.ReadAllText(file.FullName);
                string error_code = FileContent.RegexMatch("\"error_code\":[\"]*(?<info>[^<>\":,]*)[\"]*", "info");
                if (string.IsNullOrEmpty(error_code) == false)
                {
                    filename = file.Name.Replace(".txt", "").Replace("@", "");
                    list.Add(filename);
                    continue;
                }
            }
            return list;
        }
    }
}

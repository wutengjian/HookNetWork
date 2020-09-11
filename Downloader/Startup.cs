using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;

namespace Downloader
{
    public class Startup
    {
        ContainerBuilder builder;
        IContainer container;
        public Startup()
        {
        }
        /// <summary>
        /// 配置依赖注入
        /// </summary>
        public void ConfigureServices()
        {
        }
        /// <summary>
        /// 中间件配置
        /// </summary>
        public void Configure()
        {
            Run();
        }
        private void Run()
        { 
            Console.WriteLine("Downloader =》Run @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            var DownloaderJob = ConfigurationManager.AppSettings["DownloaderJob"];
            Dictionary<string, IDownload> dic = new Dictionary<string, IDownload>();
            foreach (var job in DownloaderJob.Split(','))
            {
                if (job == "GlobalTimes") dic.Add("GlobalTimes", new EnglishDownload.GlobalTimes());
                if (job == "NewsWeek") dic.Add("NewsWeek", new EnglishDownload.NewsWeek());
                if (job == "NewsUN") dic.Add("NewsUN", new EnglishDownload.NewsUN());
                if (job == "TingRoomNovel") dic.Add("TingRoomNovel", new EnglishDownload.TingRoomNovel());
                if (job == "FanYiHuJiang") dic.Add("FanYiHuJiang", new EnglishDownload.FanYiHuJiang());
                if (job == "FanYiBaiduAPI") dic.Add("FanYiBaiduAPI", new EnglishDownload.FanYiBaiduAPI());
                if (job == "TencentQT") dic.Add("TencentQT", new SharesDownload.TencentQT());
            }
            foreach (string key in dic.Keys)
            {
                try
                {
                    dic[key].Init();
                    dic[key].Run();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(key + " Downloader 异常" + ex.Message);
                }
                finally
                {

                }
            }
        }
    }
}

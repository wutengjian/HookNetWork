using Autofac;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

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
            Dictionary<string, IDownload> dic = new Dictionary<string, IDownload>();
            dic.Add("GlobalTimes", new EnglishDownload.GlobalTimes());
            dic.Add("NewsWeek", new EnglishDownload.NewsWeek());
            dic.Add("NewsUN", new EnglishDownload.NewsUN());
            dic.Add("TingRoomNovel", new EnglishDownload.TingRoomNovel());
            dic.Add("FanYiHuJiang", new EnglishDownload.FanYiHuJiang());
            dic.Add("FanYiBaiduAPI", new EnglishDownload.FanYiBaiduAPI());
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

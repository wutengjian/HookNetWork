using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Downloader;

namespace Scheduler.Monitors
{
    /// <summary>
    /// 请求监听器
    /// </summary>
    public class MonitorRequest : IMonitor
    {
        public void Init()
        {
            Console.WriteLine("MonitorRequest Init");
            Downloader.StartUp DownloaderStartup = new Downloader.StartUp();
            DownloaderStartup.ConfigureServices();
            DownloaderStartup.Configure();
            Extractor.StartUp ExtractorStartup = new Extractor.StartUp();
            ExtractorStartup.ConfigureServices();
            ExtractorStartup.Configure();
        }
        public void Run()
        {
            Console.WriteLine("MonitorRequest Run");
            try
            {
                SchedulerInfo.MonitorStatus = 1;
                StartMonitor();//开始监听
            }
            catch
            {
                EndMonitor();//结束监听
            }
        }
        private void StartMonitor()
        {
            Console.WriteLine("IMonitor 开始监听");
            Task.Run(() =>
            {
                while (SchedulerInfo.MonitorStatus > 0)
                {
                    //执行任务
                    Thread.Sleep(100);//1毫秒
                }
            });
        }
        private void EndMonitor()
        {
            SchedulerInfo.MonitorStatus = 0;
            Console.WriteLine("IMonitor 结束监听");
        }
    }
}

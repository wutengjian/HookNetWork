using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Scheduler
{
    /// <summary>
    /// 任务执行器
    /// </summary>
    public class ActuatorTask : IActuator
    {
        public void Init()
        {
            Console.WriteLine("ActuatorTask Init");
            Downloader.Startup DownloaderStartup = new Downloader.Startup();
            DownloaderStartup.ConfigureServices();
            DownloaderStartup.Configure();
        }
        public void Run()
        {
            Console.WriteLine("ActuatorTask Run");
            Console.WriteLine("MonitorRequest Run");
            try
            {
                SchedulerInfo.MonitorStatus = 1;
                //ActuatorDownloader();
                //ActuatorExtractor();
            }
            catch
            {
                SchedulerInfo.MonitorStatus = 0;
                Console.WriteLine("IMonitor 结束监听");
            }
        }
        private void ActuatorDownloader()
        {
            Console.WriteLine("IActuator 开始执行 下载器");
            Task.Run(() =>
            {
                while (true)
                {
                    var data = TaskPoolData.TryDequeue("sys");
                    if (data != null)
                    {
                        Console.WriteLine("下载器：" + data.HashKey);
                    }
                    //执行任务
                    Thread.Sleep(140);//1毫秒
                }
            });

        }
    }
}

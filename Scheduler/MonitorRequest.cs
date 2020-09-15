using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks; 

namespace Scheduler
{
    /// <summary>
    /// 请求监听器
    /// </summary>
    public class MonitorRequest : IMonitor
    {
        public void Init()
        {
            Console.WriteLine("MonitorRequest Init");
        }
        public void Run()
        {
            Console.WriteLine("MonitorRequest Run");
            try
            {
                SchedulerInfo.MonitorStatus = 1;
                Task.Run(() =>
                {
                    int num = -10;
                    string key = "sys";
                    while (SchedulerInfo.MonitorStatus == 1)
                    {
                        TaskPoolData.Enqueue(key, new TaskPoolInfo() { HashKey = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff") });
                        Thread.Sleep(100);
                    }
                });
            }
            catch
            {
                SchedulerInfo.MonitorStatus = 0;
                Console.WriteLine("IMonitor 结束监听");
            }
        }
    }
}

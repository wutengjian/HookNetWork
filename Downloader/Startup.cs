using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

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
            //new GlobalTimes().Run();
            //new NewsWeek().Run();
            new NewsUN().Run();
            //new FanYiBaiduAPI().Run();
        }
    }
}

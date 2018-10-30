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
            try
            {
                new FanYiBaiduAPI().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FanYiBaiduAPI @" + ex.Message);
            }

            try
            {
                new GlobalTimes().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("GlobalTimes @" + ex.Message);
            }

            try
            {
                new NewsWeek().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("NewsWeek @" + ex.Message);
            }

            try
            {
                new NewsUN().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("NewsUN @" + ex.Message);
            }

            try
            {
                new TingRoomNovel().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("TingRoomNovel @" + ex.Message);
            }
            try
            {
                new FanYiHuJiang().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("FanYiHuJiang @" + ex.Message);
            }
        }
    }
}

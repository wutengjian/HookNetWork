using Autofac;
using DataAnalysis.SharesAnalysis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace DataAnalysis
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
           // Thread.Sleep(1000 * 60 * 10);//休眠10分钟
            Console.WriteLine("DataAnalysis>Run @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            try
            {
                //new WordDivision().Run();
                new PriceRangeShares().Run();
                //new SharesAnalysisMonitor().Run();
            }
            catch (Exception ex)
            {
                Console.WriteLine("WordDivision @" + ex.Message);
            }
        }
    }
}

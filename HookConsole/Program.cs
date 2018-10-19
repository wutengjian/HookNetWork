using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            RunDownloader();
            RunDataAnalysis();
            while (true)
            {
                var key = Console.ReadLine();
                if (key == "exit")
                {
                    break;
                }
            }
        }
        void InitParm()
        {
            string conn = ConfigurationManager.ConnectionStrings["connHookNetWork"].ConnectionString;
            string str = ConfigurationManager.AppSettings["userName"];
        }
        private void AccessAppSettings()
        {
            //获取Configuration对象
            Configuration config = System.Configuration.ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
            //根据Key读取<add>元素的Value
            string name = config.AppSettings.Settings["name"].Value;
            //写入<add>元素的Value
            config.AppSettings.Settings["name"].Value = "fx163";
            //增加<add>元素
            config.AppSettings.Settings.Add("url", "http://www.fx163.net");
            //删除<add>元素
            config.AppSettings.Settings.Remove("name");
            //一定要记得保存，写不带参数的config.Save()也可以
            config.Save(ConfigurationSaveMode.Modified);
            //刷新，否则程序读取的还是之前的值（可能已装入内存）
            System.Configuration.ConfigurationManager.RefreshSection("appSettings");
        }
        public void FidderConfig()
        {
            //访问配置节sampleSection1  

            IDictionary IDTest1 = (IDictionary)ConfigurationSettings.GetConfig("sampleSection1");
            string str = (string)IDTest1["setting1"] + " " + (string)IDTest1["setting2"];
            //访问配置节sampleSection1的另一个方法
            string[] values1 = new string[IDTest1.Count];
            IDTest1.Values.CopyTo(values1, 0);
            //访问配置节sampleSection2  
            IDictionary IDTest2 = (IDictionary)ConfigurationSettings.GetConfig("sampleSection2");
            string[] keys = new string[IDTest2.Keys.Count];
            string[] values = new string[IDTest2.Values.Count];
            IDTest2.Keys.CopyTo(keys, 0);
            IDTest2.Values.CopyTo(values, 0);
            //访问配置节quartz  
            NameValueCollection nc = (NameValueCollection)ConfigurationSettings.GetConfig("quartz");
            NameValueCollection dc = (NameValueCollection)ConfigurationSettings.GetConfig("TestGroup/Test");
        }
        public static void RunScheduler()
        {
            Task.Run(() =>
                  {
                      Scheduler.StartUp Startup = new Scheduler.StartUp();
                      Startup.ConfigureServices();
                      Startup.Configure();
                  });
        }
        public static void RunDownloader()
        {
            Task.Run(() =>
                  {
                      Scheduler.StartUp Startup = new Scheduler.StartUp();
                      Startup.ConfigureServices();
                      Startup.Configure();
                  });
        }
        public static void RunExtractor()
        {
            Task.Run(() =>
                  {
                      Scheduler.StartUp Startup = new Scheduler.StartUp();
                      Startup.ConfigureServices();
                      Startup.Configure();
                  });
        }
        public static void RunDataAnalysis()
        {
            Task.Run(() =>
                  {
                      DataAnalysis.Startup Startup = new DataAnalysis.Startup();
                      Startup.ConfigureServices();
                      Startup.Configure();
                  });
        }
    }
}

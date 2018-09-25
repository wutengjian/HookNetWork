using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HookConsole
{
    class Program
    {
        static void Main(string[] args)
        {
            Task.Run(() =>
                   {
                       Scheduler.StartUp Startup = new Scheduler.StartUp();
                       Startup.ConfigureServices();
                       Startup.Configure();
                   });
            while (true)
            {
                var key = Console.ReadLine();
                if (key == "exit")
                {
                    break;
                }
            }
        }
    }
}

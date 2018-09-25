using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Distributors
{
    /// <summary>
    /// 任务分发器
    /// </summary>
    public class DistributorTask : IDistributor
    {
        public void Init()
        {
           Console.WriteLine("DistributorTask Init");
        }

        public void Run()
        {
          Console.WriteLine("DistributorTask Run");
        }
    }
}

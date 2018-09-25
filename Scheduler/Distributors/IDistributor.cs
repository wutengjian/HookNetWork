using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Distributors
{
    /// <summary>
    /// 分发器
    /// </summary>
    public interface IDistributor
    {
        void Init();
        void Run();
    }
}

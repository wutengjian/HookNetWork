using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Monitors
{
    /// <summary>
    /// 监听器
    /// </summary>
    public interface IMonitor
    {
        void Init();
        void Run();
    }
}

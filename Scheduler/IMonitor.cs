using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    /// <summary>
    /// 监听器(实时监听、生成任务)
    /// </summary>
    public interface IMonitor
    {
        void Init();
        void Run();
    }
}

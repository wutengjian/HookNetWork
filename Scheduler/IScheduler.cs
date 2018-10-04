using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    /// <summary>
    /// 调度器{任务启动的时间控制、系统控制}
    /// </summary>
    public interface IScheduler
    {
        void InitScheduler();
        void Monitor(IMonitor monitor);
        void Actuator(IActuator actuator);
    }
}

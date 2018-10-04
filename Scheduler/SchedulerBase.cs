using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public abstract class SchedulerBase : IScheduler
    {
        public virtual void Monitor(IMonitor monitor)
        {
            monitor.Init();
            monitor.Run();
        }
        public virtual void Actuator(IActuator actuator)
        {
            actuator.Init();
            actuator.Run();
        }

        public virtual void InitScheduler()
        {
            new TaskPoolData();
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public abstract class SchedulerBase : IScheduler
    {
        public virtual void Monitor(Monitors.IMonitor monitor)
        {
            monitor.Init();
            monitor.Run();
        }
        public virtual void Actuator(Actuators.IActuator actuator)
        {
            actuator.Init();
            actuator.Run();
        }

        public virtual void Distributor(Distributors.IDistributor distributor)
        {
            distributor.Init();
            distributor.Run();
        }
    }
}

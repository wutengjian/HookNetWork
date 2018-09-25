using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    /// <summary>
    /// 调度器
    /// </summary>
    public interface IScheduler
    {
        void Monitor(Monitors.IMonitor monitor);
        void Distributor(Distributors.IDistributor distributor);
        void Actuator(Actuators.IActuator actuator);
    }
}

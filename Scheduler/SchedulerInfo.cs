using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public class SchedulerInfo
    {
        public static ContainerBuilder builder { get; set; }
        public static int MonitorStatus = 0;
        public static int DistributorStatus = 0;
        public static int ActuatorStatus = 0;
    }
}

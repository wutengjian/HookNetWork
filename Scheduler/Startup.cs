using Autofac;
using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    public class StartUp
    {
        private IContainer container;
        private ContainerBuilder builder;
        public StartUp()
        {
            SchedulerInfo.builder = new ContainerBuilder();
            builder = SchedulerInfo.builder;
        }
        /// <summary>
        /// 配置依赖注入
        /// </summary>
        public void ConfigureServices()
        {
            builder.RegisterType<SchedulerDefault>().As<IScheduler>().SingleInstance();//注册默认的调度器
            builder.RegisterType<Monitors.MonitorRequest>().As<Monitors.IMonitor>();
            builder.RegisterType<Distributors.DistributorTask>().As<Distributors.IDistributor>();
            builder.RegisterType<Actuators.ActuatorTask>().As<Actuators.IActuator>();


        }
        /// <summary>
        /// 中间件配置
        /// </summary>
        public void Configure()
        {
            container = builder.Build();
            IScheduler scheduler = container.Resolve<IScheduler>();
            scheduler.Monitor(container.Resolve<Monitors.IMonitor>());
            scheduler.Distributor(container.Resolve<Distributors.IDistributor>());
            scheduler.Actuator(container.Resolve<Actuators.IActuator>());
        }
    }
}

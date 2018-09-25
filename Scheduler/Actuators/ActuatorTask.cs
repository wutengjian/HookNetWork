using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Actuators
{
    /// <summary>
    /// 任务执行器
    /// </summary>
    public class ActuatorTask : IActuator
    {
        public void Init()
        {
            Console.WriteLine("ActuatorTask Init");
        }

        public void Run()
        {
            Console.WriteLine("ActuatorTask Run");
        }
    }
}

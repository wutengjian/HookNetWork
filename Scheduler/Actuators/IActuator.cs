using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler.Actuators
{
    /// <summary>
    /// 执行器
    /// </summary>
    public interface IActuator
    {
        void Init();
        void Run();
    }
}

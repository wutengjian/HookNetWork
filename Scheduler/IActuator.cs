using System;
using System.Collections.Generic;
using System.Text;

namespace Scheduler
{
    /// <summary>
    /// 执行器(执行分发器中的任务)
    /// </summary>
    public interface IActuator
    {
        void Init();
        void Run();
    }
}

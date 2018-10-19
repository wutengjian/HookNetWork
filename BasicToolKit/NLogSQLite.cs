using NLog;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BasicToolKit
{
    public class NLogSQLite
    {
        public static void AddLog()
        {
            Logger log = LogManager.GetCurrentClassLogger();
            LogManager.ThrowExceptions = true;
            log.Trace("Test Begin");
            log.Debug("LogInfo");
            log.Trace("Test End");
        }

    }
}

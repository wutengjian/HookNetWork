using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using DBRepertory;
using DataAnalysis.CalculationAnalysis;
using System.Threading;

namespace DataAnalysis
{
    /// <summary>
    /// 分析管理器
    /// </summary>
    public class SharesAnalysisMonitor
    {
        public void Run()
        {
            List<ICalculationShares> list = new List<ICalculationShares>();
            //Assembly assembly = Assembly.Load("DataAnalysis");
            //Type[] types = assembly.GetTypes();
            //foreach (var t in types)
            //{
            //    if (t.GetInterface("ICalculationShares") != null && t.IsAbstract == false)
            //    {
            //        list.Add((ICalculationShares)Activator.CreateInstance(t));
            //        //list.Add((ICalculationAnalysis)Activator.CreateInstance(t,  t.FullName));
            //    }
            //}
            //list.Add(new StatisticsShares());
            list.Add(new ContinuedShares());
            //list.Add(new VertexShares());
            //list.Add(new VolatilityShares());
            //list.Add(new WaveShares());
            SharesDal dal = new SharesDal();
            var shares = dal.Getlist();
            foreach (var item in shares)
            {
                try
                {
                    Thread.Sleep(1000);
                    var data = dal.GetSharesRealDateList(item.ShareType, item.ShareCode);
                    if (data == null || data.Count < 3)
                        continue;
                    foreach (var method in list)
                    {
                        method.Run(data);
                    } 
                }
                catch (Exception ex) {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}

using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using System.Reflection;
using DBRepertory;
using DataAnalysis.SharesAnalysis;
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
            //list.Add(new ContinuedShares());
<<<<<<< HEAD
            //list.Add(new VertexShares());
=======
            list.Add(new VertexShares());
>>>>>>> d21cf772bb42714a33342a756e55a4f28aa29128
            //list.Add(new VolatilityShares());
            //list.Add(new WaveShares());
            list.Add(new ResultDimensionShares());
            SharesDal dal = new SharesDal();
            dal.UpdateHashCode();
            var shares = dal.Getlist();
            foreach (var item in shares)
            {
                try
                {
<<<<<<< HEAD
                    //item.ShareType = "sh";
                    //item.ShareCode = "601258";
=======
                    item.ShareType = "sh";
                    item.ShareCode = "601258";
>>>>>>> d21cf772bb42714a33342a756e55a4f28aa29128
                    var data = dal.GetSharesRealDateList(item.ShareType, item.ShareCode);
                    if (data == null || data.Count < 5)
                        continue;
                    foreach (var method in list)
                    {
                        method.Run(data);
                    }
<<<<<<< HEAD
                    Thread.Sleep(100);
=======
                    Thread.Sleep(1000);
>>>>>>> d21cf772bb42714a33342a756e55a4f28aa29128
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }
        }
    }
}

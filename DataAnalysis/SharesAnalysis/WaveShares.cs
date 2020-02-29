using DataAnalysis.DBModels;
using DBModels.DBSharesModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnalysis.CalculationAnalysis
{
    /// <summary>
    /// 波动剧烈(涨、跌、平三种情况的波动)
    /// </summary>
    public class WaveShares : CalculationSharesBase
    {
        public override void Run(List<SharesRealDateInfo> list)
        {
            base.Method = "WaveAnalysis"; 
            ShockWave(list);
        }
        /// <summary>
        /// 震荡波动
        /// </summary>
        /// <param name="list"></param>
        public void ShockWave(List<SharesRealDateInfo> list)
        {
            //稳定--波动--稳定。稳定：涨/跌/平
            //开始点-结束点 字典
            Dictionary<int, int> map = new Dictionary<int, int>();

            int num = 0;
            int DIndex = -1;
            double rate = 0;
            for (var i = 1; i < list.Count; i++)
            {
                var d = list[i - 1].StockRate;
                var next = list[i].StockRate;
                var a = next - d;
                if ((rate > 0 && a > 0) || (rate <= 0 && a <= 0))
                {
                    num++;
                }
                else
                {
                    num = 0;
                }
                rate = a;
                if (num > 2)
                {
                    if (i - DIndex > 2)
                    {
                        map.Add(DIndex, i);
                    }
                    num = 0;
                    DIndex = -1;
                    continue;
                }
                if (DIndex == -1)
                {
                    DIndex = i;
                }
            }
        }
        public override void SaveData()
        {
            throw new NotImplementedException();
        }
    }
}

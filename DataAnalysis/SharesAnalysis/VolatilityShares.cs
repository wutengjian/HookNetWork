using DataAnalysis.DBModels;
using DBModels.DBSharesModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnalysis.SharesAnalysis
{
    /// <summary>
    /// 波动性上涨/下跌
    /// </summary>
    public class VolatilityShares : CalculationSharesBase
    {
        public override void Run(List<SharesRealDateInfo> list)
        {
            throw new NotImplementedException();
        }

        public void aa(List<SharesRealDateInfo> list)
        {
            //持续两次以上 2:1的比例
            //平衡波动后略有上阳趋势
            var num = 0;
            var count = 0;
            for (var i = 1; i < list.Count; i++)
            {
                var rate = list[i].StockRate - list[i - 1].StockRate;

            }
        }
        public override void SaveData()
        {
            throw new NotImplementedException();
        }
    }
}

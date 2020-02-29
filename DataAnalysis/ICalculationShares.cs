using DBModels.DBSharesModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnalysis
{
    public interface ICalculationShares
    {
        void Run(List<SharesRealDateInfo> list);
    }
}

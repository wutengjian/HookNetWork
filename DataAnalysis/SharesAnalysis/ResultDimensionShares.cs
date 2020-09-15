using DBModels.DBSharesModels;
using DBRepertory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis.SharesAnalysis
{
    public class ResultDimensionShares : CalculationSharesBase
    {
        public List<StatisticalResults> records = null;
        public override void Run(List<SharesRealDateInfo> list)
        {

            换手率区间(list);
        }
        public void 换手率区间(List<SharesRealDateInfo> list)
        {
            if (list.Any(x => x.ChangeHands != "0.00%" && x.ChangeHands != "-") == false)
                return;
            records = new List<StatisticalResults>();
            Dictionary<int, List<SharesRealDateInfo>> dicData = new Dictionary<int, List<SharesRealDateInfo>>();
            dicData.Add(-3, new List<SharesRealDateInfo>());
            dicData.Add(-2, new List<SharesRealDateInfo>());
            dicData.Add(-1, new List<SharesRealDateInfo>());
            dicData.Add(0, new List<SharesRealDateInfo>());
            dicData.Add(1, new List<SharesRealDateInfo>());
            dicData.Add(2, new List<SharesRealDateInfo>());
            dicData.Add(3, new List<SharesRealDateInfo>());
            dicData.Add(-99, new List<SharesRealDateInfo>());
            foreach (var data in list)
            {
                data.ChangeHands = data.ChangeHands.Replace("%", "");
                if (data.ChangeHands == "-")
                    continue;
                data.ChangeHande = Convert.ToDouble(data.ChangeHands);
                int level = Getlevel(data.StockRate);
                if (data.ChangeHande > 0)
                    dicData[level].Add(data);
            }
            //各涨跌区间的数据
            foreach (var key in dicData.Keys)
            {
                if (dicData[key] == null || dicData[key].Count < 1)
                    continue;
                GetStatisticalResults(dicData[key], key);
            }
            SaveData();
        }
        /// <summary>
        /// 各指标统计
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        private void GetStatisticalResults(List<SharesRealDateInfo> list, int level)
        {
            records.Add(new StatisticalResults()
            {
                MethodType = "换手率",
                MaxValue = list.Max(x => x.ChangeHande),
                MinValue = list.Min(x => x.ChangeHande),
                AvgValue = list.Average(x => x.ChangeHande),
                EndDate = list.Max(x => x.ShareDate),
                StartDate = list.Min(x => x.ShareDate),
                HashCode = list[0].HashCode,
                LevelMap = level,
                StockNo = list[0].ShareType += list[0].ShareCode
            });
            //振幅、盘比 
            //众数需要考虑与总数的比值 
        }
        private int Getlevel(double data)
        {
            if (data >= 7)
            {
                return 3; //极高
            }
            else if (data >= 4 && data < 7)
            {
                return 2; //高
            }
            else if (data > 1 && data < 4)
            {
                return 1; //偏高
            }
            else if (data >= -1 && data <= 1)
            {
                return 0; //持平
            }
            else if (data >= -4 && data < -1)
            {
                return -1; //偏低
            }
            else if (data >= -7 && data < -4)
            {
                return -2; //低
            }
            else if (data < -7)
            {
                return -3; //极低
            }
            return -99;
        }

        public override void SaveData()
        {
            if (records == null && records.Count < 1)
                return;
            SharesRecordDal dal = new SharesRecordDal();
            dal.InsertBulkRecord_Dimension(records);
        }
    }

}

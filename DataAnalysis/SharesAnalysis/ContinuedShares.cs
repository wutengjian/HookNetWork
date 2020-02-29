using DataAnalysis.DBModels;
using DBModels.DBSharesModels;
using System;
using System.Collections.Generic;
using System.Text;

namespace DataAnalysis.CalculationAnalysis
{
    /// <summary>
    /// 持续上涨、下跌
    /// </summary>
    public class ContinuedShares : CalculationSharesBase
    {
        List<ContinuedRecord> records =null;
        public override void Run(List<SharesRealDateInfo> list)
        {
            base.Method = "ContinuedAnalysis";
            records = new List<ContinuedRecord>();
            //1：找出连续上升3次以上的点 集合
            ContinuedRise(list);

            ContinuedFall(list);

            Console.WriteLine(Method);
        }
        /// <summary>
        /// 上涨(10次)
        /// </summary>
        /// <param name="list"></param>
        public void ContinuedRise(List<SharesRealDateInfo> list)
        {
            Dictionary<int, List<double>> dic = new Dictionary<int, List<double>>();
            int count1 = 0;//  x>0
            int count2 = 0;//  -2<x<=0
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].StockRate > 0)
                    count1++;
                else if (list[i].StockRate > -2)
                    count2++;
                else
                {
                    if (count1 > 6 && count2 < 4)
                    {
                        var dIndex = i - (count1 + count2);//取到上涨的初始点
                        var data = getPointList(dIndex, list, 10, 60);
                        if (data != null && data.Count > 0)
                        {
                            dic.Add(i, data);
                        }
                    }
                    count1 = 0;
                    count2 = 0;
                }
            }
        }

        /// <summary>
        /// 下跌(10次)
        /// </summary>
        /// <param name="list"></param>
        public void ContinuedFall(List<SharesRealDateInfo> list)
        {
            Dictionary<int, List<double>> dic = new Dictionary<int, List<double>>();
            int count1 = 0;//  x<0
            int count2 = 0;//  0<=x<2
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].StockRate < 0)
                    count1++;
                else if (list[i].StockRate < 2)
                    count2++;
                else
                {
                    if (count1 > 6 && count2 < 4)
                    {
                        var dIndex = i - (count1 + count2);//取到下跌的初始点
                        var data = getPointList(dIndex, list, 10, 60);
                        if (data != null && data.Count > 0)
                        {
                            dic.Add(i, data);
                        }
                    }
                    count1 = 0;
                    count2 = 0;
                }
            }
        }

        /// <summary>
        /// 提取指定点的前驱 数据集合
        /// </summary>
        /// <param name="dIndex">点位</param>
        /// <param name="list">原数据</param>
        /// <param name="minNum">最小数</param>
        /// <param name="maxNum">最大数</param>
        /// <returns></returns>
        private List<double> getPointList(int dIndex, List<SharesRealDateInfo> list, int minNum, int maxNum)
        {
            if (minNum > dIndex)
                return null;
            int cIndex = dIndex;
            List<double> data = new List<double>();
            if (dIndex > maxNum)
            {
                while (maxNum > 0)
                {
                    data.Add(list[cIndex--].ClosingQuotation);
                    maxNum--;
                }
            }
            else
            {
                while (cIndex > 0)
                {
                    data.Add(list[cIndex--].ClosingQuotation);
                }
            }
            return data;
        }

        //还需要继续分析 前驱数据的画像

        public override void SaveData()
        {
            throw new NotImplementedException();
        }
    }
    public class ContinuedRecord
    {
        public int ID { get; set; }
        public int StockId { get; set; }
        public string StockNo { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Daily { get; set; } 
    }
}

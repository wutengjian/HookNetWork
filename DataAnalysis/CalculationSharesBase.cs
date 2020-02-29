using DataAnalysis.DBModels;
using DBModels.DBSharesModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DataAnalysis
{
    public abstract class CalculationSharesBase : ICalculationShares
    {
        /// <summary>
        /// 方式
        /// </summary>
        public string Method { get; set; }
        /// <summary>
        /// 
        /// </summary>
        public abstract void Run(List<SharesRealDateInfo> list); 
        /// <summary>
        /// 
        /// </summary>
        public abstract void SaveData();
        /// <summary>
        /// 
        /// </summary>
        public virtual void GetData()
        {

        }

        /// <summary>
        /// 去除最大最小值
        /// </summary>
        /// <param name="dic"></param>
        /// <returns></returns>
        public Dictionary<int, SharesRealDateInfo> RemoveMaxMin(Dictionary<int, SharesRealDateInfo> dic)
        { 
            double max = 0;
            double min = 0;
            int maxCount = 0;
            int minCount = 0;
            int maxId = 0;
            int minId = 0;
            foreach (var key in dic.Keys)
            {
                var price = dic[key].ClosingQuotation;
                if (price > max)
                {
                    max = price;
                    maxId = key;
                    maxCount = 1;
                    continue;
                }
                else if (min > price)
                {
                    min = price;
                    minId = key;
                    minCount = 1;
                    continue;
                }

                if (max == price)
                    maxCount++;
                else if (min == price)
                    minCount++;

                if (minCount > 1 || maxCount > 1)
                    break;
            }
            if (minCount > 1 || maxCount > 1)
                return dic;
            dic.Remove(maxId);
            dic.Remove(minId);
            return dic;
        }

        /// <summary>
        /// 分箱平滑，1.5倍四分位距
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public List<SharesRealDateInfo> SubBox(List<SharesRealDateInfo> list)
        {
            var lcount = list.Count;
            var center = list[lcount / 2].ClosingQuotation;//中值
            var upper = list[(int)(lcount * 0.75)].ClosingQuotation;//上
            var lower = list[(int)(lcount * 0.25)].ClosingQuotation;//下
            var maxUpper = upper+ Math.Abs(upper - center) * 1.5;
            var minLower = lower- Math.Abs(lower - center) * 1.5;
            list = list.Where(x => x.ClosingQuotation >= minLower && x.ClosingQuotation <= maxUpper).ToList();
            return list;
        }

        /// <summary>
        /// 数据分片,计算出落在每一片的数量
        /// </summary>
        /// <param name="list"></param>
        /// <returns></returns>
        public Dictionary<double, int> Blocks(List<SharesRealDateInfo> list)
        {
            var max = list.Max(x => x.ClosingQuotation);
            var min = list.Min(x => x.ClosingQuotation);
            //最小分块单位
            var block = (max - min) / (0.1 * 2);//按10%计算，结果再减半
            double current = min;
            Dictionary<double, int> dic = new Dictionary<double, int>();
            do
            {
                dic.Add(current, 0);
                current = current + block;
            }
            while (current <= max);
            var blockArr = dic.Keys.ToArray();
            for (int i = 1; i < blockArr.Length; i++)
            {
                dic[blockArr[i - 1]] = list.Where(x => x.ClosingQuotation >= blockArr[i - 1] && x.ClosingQuotation < blockArr[i]).Count();
            }
            return dic;
        }
    }
}

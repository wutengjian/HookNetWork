using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataAnalysis.DBModels;
using DBModels.DBSharesModels;
using DBRepertory;

namespace DataAnalysis.SharesAnalysis
{
    /// <summary>
    /// 统计分析
    /// </summary>
    public class StatisticsShares : CalculationSharesBase
    {
        List<ShareStatisticsRecord> records = null;
        public override void Run(List<SharesRealDateInfo> list)
        {
            records = new List<ShareStatisticsRecord>();
            Dictionary<int, SharesRealDateInfo> dic = new Dictionary<int, SharesRealDateInfo>();
            for (var i = 0; i < list.Count; i++)
            {
                dic.Add(i, list[i]);
            }
            if (list.Count > 30)
            {
                dic = RemoveMaxMin(dic);
            }
            list = dic.Values.OrderBy(x => x.ClosingQuotation).ToList<SharesRealDateInfo>();
            if (list.Count > 30)
            {
                list = SubBox(list);
            }
            list = list.OrderByDescending(x => x.ShareDate).ToList();
            for (int i = list.Count; i > 20; i--)
            {
                var item = list.Skip(list.Count - i).Take(20).ToList();
                Average(item);
                Median(item);
                ModeNumber(item);
            }

            this.SaveData();
        }
        /// <summary>
        /// 平均值
        /// </summary>
        /// <param name="dic"></param>
        public void Average(List<SharesRealDateInfo> list)
        {
            var statistics = list.Average(x => x.ClosingQuotation);
            records.Add(new ShareStatisticsRecord()
            {
                Daily = list.Max(x => x.ShareDate),
                MethodType = "Statistics>>Average",
                HashCode = list[0].HashCode,
                StockNo = list[0].ShareType + list[0].ShareCode,
                StartDate = list.Min(x => x.ShareDate),
                EndDate = list.Max(x => x.ShareDate),
                Statistics = statistics
            });

            var data = list.Where(x => x.ClosingQuotation > 1.2 * statistics && x.ClosingQuotation < 1.8 * statistics && x.StockRate < 8).ToList();//不要选出大于8的数据
            //1：记录平均数  待完成，2：记录筛选结果  待完成
        }
        /// <summary>
        /// 众数
        /// </summary>
        /// <param name="list"></param>
        public void ModeNumber(List<SharesRealDateInfo> list)
        {
            var block = Blocks(list);
            var max = block.Max(x => x.Value);
            var statistics = block.Where(x => x.Value == max).FirstOrDefault().Key;
            records.Add(new ShareStatisticsRecord() { Daily = list.Max(x => x.ShareDate), MethodType = "Statistics>>ModeNumber", HashCode = list[0].HashCode, StockNo = list[0].ShareType + list[0].ShareCode, StartDate = list.Min(x => x.ShareDate), EndDate = list.Max(x => x.ShareDate), Statistics = statistics });
            //1：记录众数  待完成
        }
        /// <summary>
        /// 中位数
        /// </summary>
        /// <param name="list"></param>
        public void Median(List<SharesRealDateInfo> list)
        {
            var count = list.Count / 2;
            var statistics = list[count].ClosingQuotation;
            records.Add(new ShareStatisticsRecord() { Daily = list.Max(x => x.ShareDate), MethodType = "Statistics>>Median", HashCode = list[0].HashCode, StockNo = list[0].ShareType + list[0].ShareCode, StartDate = list.Min(x => x.ShareDate), EndDate = list.Max(x => x.ShareDate), Statistics = statistics });
            //1：记录中位数  待完成
        }

        //需要继续分析 分片数据画像 

        public override void SaveData()
        {
            if (records == null && records.Count < 1)
                return;
            SharesRecordDal dal = new SharesRecordDal();
            dal.InsertBulkRecord(records);
        }
    }
}

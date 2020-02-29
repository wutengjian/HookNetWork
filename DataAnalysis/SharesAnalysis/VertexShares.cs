using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using DataAnalysis.DBModels;
using DBModels.DBSharesModels;

namespace DataAnalysis.CalculationAnalysis
{
    /// <summary>
    /// 顶点
    /// </summary>
    public class VertexShares : CalculationSharesBase
    {
        List<VertexRecord> records = null;
        public override void Run(List<SharesRealDateInfo> list)
        {
            base.Method = "VertexAnalysis";
            records = new List<VertexRecord>(); 
            VertexSpacing(list);
        }

        /// <summary>
        /// 顶点间隔
        /// </summary>
        public void VertexSpacing(List<SharesRealDateInfo> list)
        {
            UpperLowerVertices(list);
            //1：记录上下顶点  待完成 

            //后续可以增加 上、下定点间距计算，
        }

        /// <summary>
        /// 上下顶点
        /// </summary>
        /// <returns></returns>
        public void UpperLowerVertices(List<SharesRealDateInfo> list)
        {
            for (int i = 1; i < list.Count - 1; i++)
            {
                var LRate = (list[i].ClosingQuotation - list[i - 1].ClosingQuotation) * 100 / list[i].ClosingQuotation;//与上次比的涨跌率
                var RRate = (list[i + 1].ClosingQuotation - list[i].ClosingQuotation) * 100 / list[i + 1].ClosingQuotation;//与下次比的涨跌率
                //上阳顶点
                if (LRate > 2 && RRate <= -3)
                {
                    if (isLeftChecked(list, i, 1) && isRigthChecked(list, i, -1))
                    {
                        records.Add(new VertexRecord() { ID = i, IsUp = true, Rate = LRate, Daily = list[i].ShareDate, HashCode = list[i].HashCode, StockNo = list[i].ShareCode });
                    }
                }
                //下阴顶点
                else if (RRate > 1 && LRate <= -2)
                {
                    if (isLeftChecked(list, i, -1) && isRigthChecked(list, i, 1))
                    {
                        records.Add(new VertexRecord() { ID = i, IsUp = false, Rate = RRate, Daily = list[i].ShareDate, HashCode = list[i].HashCode, StockNo = list[i].ShareCode });
                    }
                }
            }
        }

        /// <summary>
        /// 是否连续且处于顶点
        /// </summary>
        /// <param name="i">某点</param>
        /// <param name="v">涨跌率，>0：涨，<0：跌</param>
        /// <returns></returns>
        private bool isLeftChecked(List<SharesRealDateInfo> list, int i, int v)
        {
            int num = 2;
            bool result = true;
            if (i < 2)
                return true;
            List<double> rates = new List<double>();
            while (num >= 0)
            {
                var rate = (list[i - num].ClosingQuotation - list[i - 1 - num].ClosingQuotation) * 100 / list[i - num].ClosingQuotation;
                rates.Add(rate);
                num--;
            }

            if (rates.Count < 1)
                return true;
            var avg = rates.Average();
            if (v >= 0 && avg <= v)
                result = false;
            else if (v < 0 && avg > v)
                result = false;
            return result;
        }

        /// <summary>
        /// 是否连续且处于顶点
        /// </summary>
        /// <param name="i">某点</param>
        /// <param name="v">涨跌率，>0：跌，<0：涨</param>
        /// <returns></returns>
        private bool isRigthChecked(List<SharesRealDateInfo> list, int i, int v)
        {
            int num = 2;
            bool result = true;
            if (list.Count - i < 2)
                return true;
            List<double> rates = new List<double>();
            while (num >= 0)
            {
                var rate = (list[i + 1 + num].ClosingQuotation - list[i + num].ClosingQuotation) * 100 / list[i + 1 + num].ClosingQuotation;
                rates.Add(rate);
                num--;
            }
            if (rates.Count < 1)
                return true;
            var avg = rates.Average();
            if (v >= 0 && avg <= v)
                result = false;
            else if (v < 0 && avg > v)
                result = false;
            return result;
        }

        public override void SaveData()
        {
            throw new NotImplementedException();
        }
    }
    public class VertexRecord
    {
        public int ID { get; set; }
        public string HashCode { get; set; }
        public string StockNo { get; set; }
        /// <summary>
        /// 日期
        /// </summary>
        public DateTime Daily { get; set; }
        /// <summary>
        /// 顶点的涨跌率
        /// </summary>
        public double Rate { get; set; }
        /// <summary>
        /// 是否上顶点
        /// </summary>
        public bool IsUp { get; set; }
    }
}

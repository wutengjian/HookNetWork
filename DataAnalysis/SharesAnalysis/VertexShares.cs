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
    /// 顶点
    /// </summary>
    public class VertexShares : CalculationSharesBase
    {
        List<ShareStatisticsRecord> records = null;
        public override void Run(List<SharesRealDateInfo> list)
        {
            records = new List<ShareStatisticsRecord>();
            UpperLowerVertices(list);
            //1：记录上下顶点  待完成 
            SaveData();
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
                var LRate = list[i - 1].StockRate;//与上次比的涨跌率
                var RRate = list[i].StockRate;//与下次比的涨跌率 
                if (LRate > 2 && RRate <= -3 && isLeftChecked(list, i, 1) && isRigthChecked(list, i, -1))
                {
                    //上阳顶点
                    records.Add(new ShareStatisticsRecord() { ID = i, Statistics = 1, Daily = list[i].ShareDate, MethodType = "VertexShares", HashCode = list[i].HashCode, StockNo = list[i].ShareType + list[i].ShareCode, StartDate = DateTime.Now, EndDate = DateTime.Now });
                }
                else if (RRate > 1 && LRate <= -2 && isLeftChecked(list, i, -1) && isRigthChecked(list, i, 1))
                {
                    //下阴顶点
                    records.Add(new ShareStatisticsRecord() { ID = i, Statistics = -1, Daily = list[i].ShareDate, MethodType = "VertexShares", HashCode = list[i].HashCode, StockNo = list[i].ShareType + list[i].ShareCode, StartDate = DateTime.Now, EndDate = DateTime.Now });
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
                rates.Add(list[i - num].StockRate);
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
                rates.Add(list[i + num].StockRate);
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
            if (records == null && records.Count < 1)
                return;
            SharesRecordDal dal = new SharesRecordDal();
            dal.InsertBulkRecord(records);
        }
    }
    //public class VertexRecord
    //{
    //    public int ID { get; set; }
    //    public string HashCode { get; set; }
    //    public string StockNo { get; set; }
    //    /// <summary>
    //    /// 日期
    //    /// </summary>
    //    public DateTime Daily { get; set; }
    //    /// <summary>
    //    /// 顶点的涨跌率
    //    /// </summary>
    //    public double Rate { get; set; }
    //    /// <summary>
    //    /// 是否上顶点
    //    /// </summary>
    //    public bool IsUp { get; set; }
    //}
}

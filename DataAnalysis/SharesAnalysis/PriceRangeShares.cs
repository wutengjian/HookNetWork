using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DBModels.DBSharesModels;
using DBRepertory;

namespace DataAnalysis.SharesAnalysis
{
    public class PriceRangeShares
    {
        SharesDal dal = null;
        Dictionary<string, int> dicData = null;
        public void Run()
        {
            dal = new SharesDal();
            if (DateTime.Now.DayOfWeek == DayOfWeek.Friday)
            {
                Newest();
                History();
            }
        }
        public void Newest()
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = DateTime.Now.AddMonths(-1);
            List<SharesRealDateInfo> list = dal.GetSharesRealDateList(startDate, endDate);
            Dictionary<int, int> priceNum = new Dictionary<int, int>();
            var result = Analysis(list, out priceNum);
            List<PriceRangeRecord> data = new List<PriceRangeRecord>();
            foreach (var key in result.Keys)
            {
                data.Add(new PriceRangeRecord() { DataNum = result[key], ExecutionTime = DateTime.Now, RatePrice = key, StartDate = startDate, EndDate = endDate, ShareNum = priceNum[Convert.ToInt32(key.Split(':')[1])], TotalNum = list.Count });
            }
        }
        public void History()
        {
            DateTime endDate = DateTime.Now;
            DateTime startDate = endDate.AddMonths(-1);
            while (true)
            {
                if (startDate < Convert.ToDateTime("2010-01-01"))
                    break;
                List<SharesRealDateInfo> list = dal.GetSharesRealDateList(startDate, endDate);
                Dictionary<int, int> priceNum = new Dictionary<int, int>();
                var result = Analysis(list, out priceNum);
                List<PriceRangeRecord> data = new List<PriceRangeRecord>();
                foreach (var key in result.Keys)
                {
                    data.Add(new PriceRangeRecord() { DataNum = result[key], ExecutionTime = DateTime.Now, RatePrice = key, StartDate = startDate, EndDate = endDate, ShareNum = priceNum[Convert.ToInt32(key.Split(':')[1])], TotalNum = list.Count });
                }
                SaveData(data);
                endDate = startDate;
                startDate = endDate.AddMonths(-1);
            }
        }
        public Dictionary<string, int> Analysis(List<SharesRealDateInfo> list, out Dictionary<int, int> priceNum)
        {
            dicData = new Dictionary<string, int>();
            priceNum = new Dictionary<int, int>();
            foreach (var data in list)
            {
                int level = GetlevelRate(data.StockRate);
                int price = GetLevelPrice(data.ClosingQuotation);
                string key = level + ":" + price;
                if (dicData.ContainsKey(key) == false)
                    dicData.Add(key, 0);
                dicData[key] = dicData[key] + 1;
                if (priceNum.ContainsKey(price) == false)
                    priceNum.Add(price, 0);
                priceNum[price] = priceNum[price] + 1;
            }
            return dicData;
        }

        public void SaveData(List<PriceRangeRecord> data)
        {
            new SharesRecordDal().InsertBulkRecord_PriceRange(data);
        }
        private int GetlevelRate(double data)
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
        private int GetLevelPrice(double data)
        {
            if (data < 3)
                return 1;
            else if (data >= 3 && data < 10)
                return 2;
            else if (data >= 10 && data < 20)
                return 3;
            else if (data >= 20 && data < 30)
                return 4;
            else if (data >= 30 && data < 60)
                return 5;
            else if (data >= 60 && data < 100)
                return 6;
            else if (data >= 100 && data < 150)
                return 7;
            else if (data >= 150 && data < 300)
                return 8;
            else if (data >= 300 && data < 800)
                return 9;
            else if (data >= 800)
                return 10;
            return 999;
        }
    }
}

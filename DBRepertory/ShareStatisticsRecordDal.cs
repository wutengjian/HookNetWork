using Dapper;
using DBModels.DBSharesModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepertory
{
    public class SharesRecordDal
    {
        string ConnStr = "Data Source=JiannyWu;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian123";

        public void InsertBulkRecord(List<ShareStatisticsRecord> list)
        {
            string sql = string.Format(@" insert into ShareStatisticsRecord(HashCode,StockNo,Daily,MethodType,StartDate,EndDate,[Statistics])
 values(@HashCode,@StockNo,@Daily,@MethodType,@StartDate,@EndDate,@Statistics)");
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open(); conn.Execute(sql, list, commandTimeout: 60);
                conn.Close();
            }
        }
        public void InsertBulkRecord_Dimension(List<StatisticalResults> list)
        {
            string sql = string.Format(@" insert into SharesResultDimension(HashCode,StockNo,MethodType,StartDate,EndDate,LevelMap,MaxValue,MinValue,AvgValue,MostValue)
 values(@HashCode,@StockNo,@MethodType,@StartDate,@EndDate,@LevelMap,@MaxValue,@MinValue,@AvgValue,@MostValue)");
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open(); conn.Execute(sql, list, commandTimeout: 60);
                conn.Close();
            }
        }
        public void InsertBulkRecord_PriceRange(List<PriceRangeRecord> list) {
              string sql = string.Format(@"INSERT INTO Record_PriceRange(StartDate,EndDate,RatePrice,DataNum,ShareNum,TotalNum,ExecutionTime) VALUES(@StartDate,@EndDate,@RatePrice,@DataNum,@ShareNum,@TotalNum,@ExecutionTime)");
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open(); conn.Execute(sql, list, commandTimeout: 60);
                conn.Close();
            }
        }
    }
}

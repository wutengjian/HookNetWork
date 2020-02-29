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
    public class ShareStatisticsRecordDal
    {
        string ConnStr = "Data Source=192.168.0.102;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian128";
       
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
    }
}

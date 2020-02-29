using Dapper;
using DBModels;
using DBModels.DBSharesModels;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBRepertory
{
    public class SharesDal
    {
        string ConnStr = "Data Source=JiannyWu;Initial Catalog=HookNetWork;Persist Security Info=True;User ID=sa;Password=wutengjian123";
        public void SaveList(List<SharesBasicInfo> ArticleList)
        {
            var data = SqlServerBulkCopy.ToDataTable<SharesBasicInfo>(ArticleList);
            Dictionary<string, string> SqlMapping = new Dictionary<string, string>();
            SqlMapping.Add("ShareType", "ShareType");
            SqlMapping.Add("ShareCode", "ShareCode");
            SqlMapping.Add("ShareName", "ShareName");
            SqlMapping.Add("YesterdayHarvest", "YesterdayHarvest");
            SqlMapping.Add("TodayOpens", "TodayOpens");
            SqlMapping.Add("VolumeHand", "VolumeHand");
            SqlMapping.Add("OuterDisc", "OuterDisc");
            SqlMapping.Add("Invol", "Invol");
            SqlMapping.Add("BuyingPrice", "BuyingPrice");
            SqlMapping.Add("BuyingVolume", "BuyingVolume");
            SqlMapping.Add("SellingPrice", "SellingPrice");
            SqlMapping.Add("SellQuantity", "SellQuantity");
            SqlMapping.Add("ShareTime", "ShareTime");
            SqlMapping.Add("RiseFall", "RiseFall");
            SqlMapping.Add("RiseFallRate", "RiseFallRate");
            SqlMapping.Add("Highest", "Highest");
            SqlMapping.Add("Minimum", "Minimum");
            SqlMapping.Add("Turnover", "Turnover");
            SqlMapping.Add("PERatio", "PERatio");
            SqlMapping.Add("Amplitude", "Amplitude");
            SqlMapping.Add("MarketValueCirculation", "MarketValueCirculation");
            SqlMapping.Add("TotalMarketValue", "TotalMarketValue");
            SqlMapping.Add("MarketRate", "MarketRate");
            SqlMapping.Add("HighLimit", "HighLimit");
            SqlMapping.Add("LimitPrice", "LimitPrice");
            SqlMapping.Add("DownloadDate", "DownloadDate");
            SqlMapping.Add("CreateTime", "CreateTime");
            SqlServerBulkCopy.SqlBulkMapping(SqlMapping);
            SqlServerBulkCopy.ConnStr = ConnStr;
            SqlServerBulkCopy.SqlBulkCopyToServer(data, "SharesBasic");
        }
        public void InsertBulk(List<SharesBasicInfo> ArticleList)
        {
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                string sql = string.Format(@"INSERT INTO dbo.SharesBasic(ShareType,ShareCode,ShareName,YesterdayHarvest,TodayOpens,VolumeHand,OuterDisc,Invol,BuyingPrice,BuyingVolume,SellingPrice,SellQuantity,ShareTime,RiseFall,RiseFallRate,Highest,Minimum,Turnover,PERatio,Amplitude,MarketValueCirculation,TotalMarketValue,MarketRate,HighLimit,LimitPrice,DownloadDate,CreateTime) 
                  VALUES(@ShareType,@ShareCode,@ShareName,@YesterdayHarvest,@TodayOpens,@VolumeHand,@OuterDisc,@Invol,@BuyingPrice,@BuyingVolume,@SellingPrice,@SellQuantity,@ShareTime,@RiseFall,@RiseFallRate,@Highest,@Minimum,@Turnover,@PERatio,@Amplitude,@MarketValueCirculation,@TotalMarketValue,@MarketRate,@HighLimit,@LimitPrice,@DownloadDate,@CreateTime)");
                conn.Execute(sql, ArticleList, commandTimeout: 60);
                conn.Close();
            }
        }
        public void InsertBulkHistory(List<SharesRealDateInfo> list)
        {
            string sql = string.Format(@"INSERT INTO [dbo].[SharesRealDate]
           ([ShareType]
           ,[ShareCode]
           ,[ShareDate]
           ,[OpeningQuotation]
           ,[ClosingQuotation]
           ,[UpsDowns]
           ,[Gain]
           ,[Minimum]
           ,[Highest]
           ,[Volume]
           ,[Turnover]
           ,[ChangeHands])
     VALUES
           (@ShareType,
            @ShareCode,
            @ShareDate,
            @OpeningQuotation, 
            @ClosingQuotation, 
            @UpsDowns,
            @Gain, 
            @Minimum,
            @Highest,
            @Volume, 
            @Turnover,
            @ChangeHands) ");
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open(); conn.Execute(sql, list, commandTimeout: 60);
                conn.Close();
            }
        }
        public List<SharesBasicInfo> Getlist()
        {
            List<SharesBasicInfo> List = new List<SharesBasicInfo>();
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                List = conn.Query<SharesBasicInfo>("select * from [dbo].[SharesBasic] with(nolock) where DataStatus=1 ", commandTimeout: 3000).ToList();
                conn.Close();
            }
            return List;
        }
        public List<SharesRealDateInfo> GetSharesRealDateList(string ShareType, string ShareCode)
        {
            string query = @"select ShareType,ShareCode,ShareDate,OpeningQuotation, ClosingQuotation, UpsDowns,Gain,Minimum, Highest,Volume, Turnover,ChangeHands,convert(varchar(50), HashCode) as HashCode,replace(gain,'%','') as StockRate 
  from SharesRealDate where ShareDate>'2018-01-01' ";
            if (string.IsNullOrEmpty(ShareType) == false)
            {
                query += " and ShareType='" + ShareType + "'";
            }
            if (string.IsNullOrEmpty(ShareCode) == false)
            {
                query += " and ShareCode='" + ShareCode + "'";
            }
            query += " order by ShareDate desc";
            List<SharesRealDateInfo> List = new List<SharesRealDateInfo>();
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                List = conn.Query<SharesRealDateInfo>(query, commandTimeout: 3000).ToList();
                conn.Close();
            }
            return List;
        }
        public DateTime GetMaxDate()
        {
            DateTime dt;
            using (var conn = new SqlConnection(ConnStr))
            {
                conn.Open();
                dt = conn.QueryFirst<DateTime>("select max(ShareDate) from [dbo].[SharesRealDate] with(nolock)", commandTimeout: 3000);
                conn.Close();
            }
            return dt;
        }
    }
}

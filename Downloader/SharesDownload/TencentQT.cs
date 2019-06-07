using DBModels;
using DBRepertory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Text.RegularExpressions;

namespace Downloader.SharesDownload
{
    public class TencentQT : DownloadBase
    {
        private Dictionary<int, map> DataMap;
        public List<SharesBasicInfo> DataList;

        string CodeType = "";
        public override void Init()
        {
            RootUrl = "http://qt.gtimg.cn";
            RootAddress = "F:\\HookNetWork\\TencentQT\\";
            _headers = @"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8
Accept-Encoding:  deflate
Accept-Language: zh-CN,zh;q=0.9
Cache-Control: max-age=0
Connection: keep-alive
Host: qt.gtimg.cn
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/69.0.3497.100 Safari/537.36";
            httpFactory = new HttpRequestFactory(true);
            ArticleFileSQLite SQLitedal = new ArticleFileSQLite();
            FileList = SQLitedal.GetFileNames("TencentQT");
            DataList = new List<SharesBasicInfo>(100);
            DataMap = new Dictionary<int, map>();
            DataMap.Add(1, new map() { mapIndex = 1, mapName = "ShareName", mapDescribe = "股票名字" });
            DataMap.Add(2, new map() { mapIndex = 2, mapName = "ShareCode", mapDescribe = "股票代码" });
            DataMap.Add(3, new map() { mapIndex = 3, mapName = "CurrentPrice", mapDescribe = "当前价格" });
            DataMap.Add(4, new map() { mapIndex = 4, mapName = "YesterdayHarvest", mapDescribe = "昨收" });
            DataMap.Add(5, new map() { mapIndex = 5, mapName = "TodayOpens", mapDescribe = "今开" });
            DataMap.Add(6, new map() { mapIndex = 6, mapName = "VolumeHand", mapDescribe = "成交量（手）" });
            DataMap.Add(7, new map() { mapIndex = 7, mapName = "OuterDisc", mapDescribe = "外盘" });
            DataMap.Add(8, new map() { mapIndex = 8, mapName = "Invol", mapDescribe = "内盘" });
            DataMap.Add(9, new map() { mapIndex = 9, mapName = "BuyingPrice", mapDescribe = "买入价" });
            DataMap.Add(10, new map() { mapIndex = 10, mapName = "BuyingVolume", mapDescribe = "买入量（手）" });
            DataMap.Add(11, new map() { mapIndex = 11, mapName = "BuyingPrice2", mapDescribe = "买入价二" });
            DataMap.Add(12, new map() { mapIndex = 12, mapName = "BuyingVolume2", mapDescribe = "买入量二" });
            DataMap.Add(13, new map() { mapIndex = 13, mapName = "BuyingPrice3", mapDescribe = "买入价三" });
            DataMap.Add(14, new map() { mapIndex = 14, mapName = "BuyingVolume3", mapDescribe = "买入量三" });
            DataMap.Add(15, new map() { mapIndex = 15, mapName = "BuyingPrice4", mapDescribe = "买入价四" });
            DataMap.Add(16, new map() { mapIndex = 16, mapName = "BuyingVolume4", mapDescribe = "买入量四" });
            DataMap.Add(17, new map() { mapIndex = 17, mapName = "BuyingPrice5", mapDescribe = "买入价五" });
            DataMap.Add(18, new map() { mapIndex = 18, mapName = "BuyingVolume5", mapDescribe = "买入量五" });
            DataMap.Add(19, new map() { mapIndex = 19, mapName = "SellingPrice", mapDescribe = "卖出价" });
            DataMap.Add(20, new map() { mapIndex = 20, mapName = "SellQuantity", mapDescribe = "卖出量" });
            DataMap.Add(21, new map() { mapIndex = 21, mapName = "SellingPrice2", mapDescribe = "卖出价二" });
            DataMap.Add(22, new map() { mapIndex = 22, mapName = "SellQuantity2", mapDescribe = "卖出量二" });
            DataMap.Add(23, new map() { mapIndex = 23, mapName = "SellingPrice3", mapDescribe = "卖出价三" });
            DataMap.Add(24, new map() { mapIndex = 24, mapName = "SellQuantity3", mapDescribe = "卖出量三" });
            DataMap.Add(25, new map() { mapIndex = 25, mapName = "SellingPrice4", mapDescribe = "卖出价四" });
            DataMap.Add(26, new map() { mapIndex = 26, mapName = "SellQuantity4", mapDescribe = "卖出量四" });
            DataMap.Add(27, new map() { mapIndex = 27, mapName = "SellingPrice5", mapDescribe = "卖出价五" });
            DataMap.Add(28, new map() { mapIndex = 28, mapName = "SellQuantity5", mapDescribe = "卖出量五" });
            DataMap.Add(29, new map() { mapIndex = 29, mapName = "RecentDeal", mapDescribe = "最近逐笔成交" });
            DataMap.Add(30, new map() { mapIndex = 30, mapName = "ShareTime", mapDescribe = "时间" });
            DataMap.Add(31, new map() { mapIndex = 31, mapName = "RiseFall", mapDescribe = "涨跌" });
            DataMap.Add(32, new map() { mapIndex = 32, mapName = "RiseFallRate", mapDescribe = "涨跌比例" });
            DataMap.Add(33, new map() { mapIndex = 33, mapName = "Highest", mapDescribe = "最高" });
            DataMap.Add(34, new map() { mapIndex = 34, mapName = "Minimum", mapDescribe = "最低" });
            DataMap.Add(35, new map() { mapIndex = 35, mapName = "Price/Volume (Hand)/Volume", mapDescribe = "价格/成交量（手）/成交额" });
            DataMap.Add(36, new map() { mapIndex = 36, mapName = "Volume", mapDescribe = "成交量（手）" });
            DataMap.Add(37, new map() { mapIndex = 37, mapName = "Turnover", mapDescribe = "成交额（万）" });
            DataMap.Add(39, new map() { mapIndex = 39, mapName = "PERatio", mapDescribe = "市盈率" });
            DataMap.Add(43, new map() { mapIndex = 43, mapName = "Amplitude", mapDescribe = "振幅" });
            DataMap.Add(44, new map() { mapIndex = 44, mapName = "MarketValueCirculation", mapDescribe = "流通市值" });
            DataMap.Add(45, new map() { mapIndex = 45, mapName = "TotalMarketValue", mapDescribe = "总市值" });
            DataMap.Add(46, new map() { mapIndex = 46, mapName = "MarketRate", mapDescribe = "市净率" });
            DataMap.Add(47, new map() { mapIndex = 47, mapName = "HighLimit", mapDescribe = "涨停价" });
            DataMap.Add(48, new map() { mapIndex = 48, mapName = "LimitPrice", mapDescribe = "跌停价" });

        }

        public override void Run()
        {
            //base.DeleteFile(RootAddress);
            //Download();
            StockHistoryPrice();
        }

        public void Download()
        {
            Console.WriteLine("Downloader =》TencentQT>开始下载 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            var SharesList = new List<string>();
            CodeType = "sh";
            for (int i = 1; i < 1000000; i++)
            {
                var ShareCode = GetCode(CodeType, i);
                LatestQuotation(ShareCode);
            }
            CodeType = "sz";
            for (int i = 1; i < 1000000; i++)
            {
                var ShareCode = GetCode(CodeType, i);
                LatestQuotation(ShareCode);
            }
            Console.WriteLine("Downloader =》TencentQT>下载完成 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
        }
        #region 最新报价
        /// <summary>
        /// 最新报价
        /// </summary>
        /// <param name="ShareCode"></param>
        public void LatestQuotation(string ShareCode)
        {
            //http://qt.gtimg.cn/q=sz000858
            string httpContent = httpFactory.http(RootUrl + "/q=" + ShareCode, "GET", _headers, null, Encoding.GetEncoding("gbk"), null).Replace("&gt;", " ").Replace(">>", " ").Replace('"', '\'');
            // httpContent = "v_sz000858='51~五 粮 液~000858~101.25~103.13~103.40~288484~137485~150799~101.23~5~101.22~15~101.21~164~101.20~127~101.19~9~101.25~53~101.26~76~101.27~65~101.28~124~101.29~287~15:00:04/101.25/4611/S/46684249/15960|14:57:00/101.37/87/B/881851/15827|14:56:57/101.36/19/S/192595/15823|14:56:54/101.37/91/S/922392/15820|14:56:51/101.37/93/B/942719/15817|14:56:48/101.37/263/B/2665611/15813~20190531153004~-1.88~-1.82~103.40~101.03~101.25/288484/2938475709~288484~293848~0.76~26.40~~103.40~101.03~2.30~3843.22~3930.13~5.62~113.44~92.82~0.81~-285~101.86~15.17~29.36~~~1.40~293847.57';";
            if (httpContent.Contains("v_pv_none_match"))
                return;
            Thread.Sleep(5);
            var arr = httpContent.Split('~');
            if (arr.Length >= DataMap.Keys.Count)
            {
                foreach (var key in DataMap.Keys)
                {
                    DataMap[key].DataValue = arr[key];
                    Console.WriteLine(DataMap[key].mapDescribe + "：" + arr[key]);
                }
                ExtractLatestQuotation();
            }
            if (DataList.Count >= 100)
            {
                SharesDal dal = new SharesDal();
                dal.SaveList(DataList);
                DataList = new List<SharesBasicInfo>();
            }
        }
        public void ExtractLatestQuotation()
        {
            Console.WriteLine("Downloader =》TencentQT>解析 @" + DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss:fff"));
            SharesBasicInfo model = new SharesBasicInfo()
            {
                ShareType = CodeType,
                ShareCode = DataMap[2].DataValue,
                ShareName = DataMap[1].DataValue,
                YesterdayHarvest = DataMap[4].DataValue,
                TodayOpens = DataMap[5].DataValue,
                VolumeHand = DataMap[6].DataValue,
                OuterDisc = DataMap[7].DataValue,
                Invol = DataMap[8].DataValue,
                BuyingPrice = DataMap[9].DataValue,
                BuyingVolume = DataMap[10].DataValue,
                SellingPrice = DataMap[19].DataValue,
                SellQuantity = DataMap[20].DataValue,
                ShareTime = GetDateTime(DataMap[30].DataValue),
                RiseFall = DataMap[31].DataValue,
                RiseFallRate = DataMap[32].DataValue,
                Highest = DataMap[33].DataValue,
                Minimum = DataMap[34].DataValue,
                Turnover = DataMap[37].DataValue,
                PERatio = DataMap[39].DataValue,
                Amplitude = DataMap[43].DataValue,
                MarketValueCirculation = DataMap[44].DataValue,
                TotalMarketValue = DataMap[45].DataValue,
                MarketRate = DataMap[46].DataValue,
                HighLimit = DataMap[47].DataValue,
                LimitPrice = DataMap[48].DataValue,
                DownloadDate = DateTime.Now,
                CreateTime = DateTime.Now,
            };
            DataList.Add(model);
        }
        #endregion

        #region 历史股价
        public void StockHistoryPrice()
        {
             _headers = @"Accept: text/html,application/xhtml+xml,application/xml;q=0.9,image/webp,image/apng,*/*;q=0.8
Accept-Encoding: gzip, deflate
Accept-Language: zh-CN,zh;q=0.9,en;q=0.8
Cache-Control: max-age=0
Connection: keep-alive
Host: q.stock.sohu.com
Upgrade-Insecure-Requests: 1
User-Agent: Mozilla/5.0 (Windows NT 10.0; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/65.0.3325.181 Safari/537.36";
            httpFactory = new HttpRequestFactory(true);
            //http://q.stock.sohu.com/hisHq?code=cn_600009&start=20180716&end=20180720&stat=1&order=D&period=d&callback=historySearchHandler&rt=jsonp
            //各参数的含义为：code：股票代码，以cn_开头，start:起始时间，end：截止时间，stat:统计信息，为0的时候就不返回stat对应的值了，order：排序方法（D表示降序排，A表示升序排），period：数据周期（d表示日线，m表示月线，w表示周线）。
            //返回的数据以这条为例"2018-07-20","61.22","61.83","0.61","1.00%","61.22","62.69","57637","35856.55","0.53%"分别表示日期，开盘，收盘，涨跌，涨幅，最低，最高，成交量，成交额，换手
            SharesDal dal = new SharesDal();
            var list = dal.Getlist();
            Regex rContext = new Regex("hq'\\:\\[(.*?\\])\\]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            Regex rList = new Regex("\\[(.*?)\\]", RegexOptions.Singleline | RegexOptions.IgnoreCase);
            foreach (var model in list)
            {
                string url = string.Format("http://q.stock.sohu.com/hisHq?code=cn_{0}&start=20000101&end=20101231&stat=1&order=D&period=d&callback=historySearchHandler&rt=jsonp", model.ShareCode);
                string httpContent = httpFactory.http(url, "GET", _headers, null, Encoding.UTF8, null).Replace('"', '\'');
                if (string.IsNullOrEmpty(httpContent) || httpContent.Contains("non-existent"))
                {
                    continue;
                }
                httpContent = rContext.Match(httpContent).Value;
                List<SharesRealDateInfo> listHistory = new List<SharesRealDateInfo>(1000);
                foreach (Match m in rList.Matches(httpContent))
                {
                    var arr = m.Value.Split(',');
                    if (arr.Length < 10)
                    {
                        break;
                    }
                    var 日期 = arr[0];
                    var 开盘 = arr[1];
                    var 收盘 = arr[2];
                    var 涨跌 = arr[3];
                    var 涨幅 = arr[4];
                    var 最低 = arr[5];
                    var 最高 = arr[6];
                    var 成交量 = arr[7];
                    var 成交额 = arr[8];
                    var 换手 = arr[9];

                    listHistory.Add(new SharesRealDateInfo()
                    {
                        ShareDate = 日期,
                        OpeningQuotation = 开盘,
                        ClosingQuotation = 收盘,
                        UpsDowns = 涨跌,
                        Gain = 涨幅,
                        Minimum = 最低,
                        Highest = 最高,
                        Volume = 成交量,
                        Turnover = 成交额,
                        ChangeHands = 换手,
                        ShareType = model.ShareType,
                        ShareCode = model.ShareCode
                    });
                }
                dal.InsertBulkHistory(listHistory);
            }
        }
        #endregion
        private string GetCode(string CodeType, int Num)
        {
            if (Num < 10)
                return CodeType + "00000" + Num;
            else if (Num < 100)
                return CodeType + "0000" + Num;
            else if (Num < 1000)
                return CodeType + "000" + Num;
            else if (Num < 10000)
                return CodeType + "00" + Num;
            else if (Num < 100000)
                return CodeType + "0" + Num;
            return CodeType + Num;
        }
        private DateTime GetDateTime(string data)
        {
            string t = Regex.Replace(data, @"(\d{4})(\d{2})(\d{2})(\d{2})(\d{2})(\d{2})", "$1-$2-$3 $4:$5:$6", RegexOptions.IgnoreCase | RegexOptions.Singleline);
            return Convert.ToDateTime(t);
        }
    }
    public class map
    {
        public int mapIndex { get; set; }
        public string mapName { get; set; }
        public string mapDescribe { get; set; }
        public string DataValue { get; set; }
    }
}

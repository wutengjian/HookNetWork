using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBModels.DBSharesModels
{
    public class PriceRangeRecord
    {
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string RatePrice { get; set; }
        public int DataNum { get; set; }
        public int ShareNum { get; set; }
        public int TotalNum { get; set; }
        public DateTime ExecutionTime { get; set; }
    }
}

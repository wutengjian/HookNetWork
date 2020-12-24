using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAnalysis
{
    public class BlockDivision
    {
        private List<decimal> interval { get; set; }
        private decimal coefficient = 0;
        public int MinBlock { get; set; }
        public int MaxBlock { get; set; }
        public decimal MinInterval { get; set; }
        public decimal MaxInterval { get; set; }
        public List<int> AutomaticBlock(decimal[] data, out decimal coeff)
        {
            coeff = 0;
            interval = new List<decimal>();
            data = data.OrderBy(x => x).ToArray();
            for (int i = 1; i < data.Length; i++)
            {
                interval.Add(data[i] - data[i - 1]);
            }
            Console.WriteLine("最大系数" + interval.Max());
            Console.WriteLine("最小系数" + interval.Min());
            coefficient = interval.Average();
            coefficient = (interval.Max() - interval.Min()) / 2;
            List<int> blockIndex = AutomaticBlockMonitor();
            coeff = coefficient;
            return blockIndex;
        }
        public List<int> AutomaticBlockMonitor()
        {
            if (MinInterval > 0 && coefficient < MinInterval)
                return null;
            if (MaxInterval > 0 && coefficient > MaxInterval)
                return null;
            List<int> blockIndex = AutomaticBlock();
            if (MinBlock > 0 && blockIndex.Count < MinBlock)
            {
                coefficient = coefficient - interval.Min();
                blockIndex = AutomaticBlockMonitor();
            }
            if (MaxBlock > 0 && blockIndex.Count > MaxBlock)
            {
                coefficient = coefficient + interval.Min();
                blockIndex = AutomaticBlockMonitor();
            }
            return blockIndex;
        }
        public List<int> AutomaticBlock()
        {
            List<int> blockIndex = new List<int>();
            for (int i = 1; i < interval.Count; i++)
            {
                if (interval[i] > coefficient)
                    blockIndex.Add(i);
            }
            return blockIndex;
        }

        public void AutomaticBlockTest()
        {
            List<decimal> data = new List<decimal>();
            Random random = new Random();
            for (int i = 0; i < 1000; i++)
            {
                data.Add(Convert.ToDecimal(random.NextDouble() * 1000));
            }
            this.MinBlock = 10;
            this.MaxBlock = 20;
            this.MinInterval = 3;
            this.MaxInterval = 5;
            decimal coeff = 0;
            List<int> blockIndex = AutomaticBlock(data.ToArray(), out coeff);
            Console.WriteLine("数据被切割成{0}份，切割系数{1}", blockIndex.Count, coeff);
        }
    }
}

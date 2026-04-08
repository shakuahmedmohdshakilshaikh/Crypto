using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class AllocationChartDTO
    {
        public int CryptoId { get; set; }
        public string CryptoName { get; set; }
        public string Symbol { get; set; }
        public decimal Value { get; set; }
        public decimal Percentage { get; set; }
    }
}

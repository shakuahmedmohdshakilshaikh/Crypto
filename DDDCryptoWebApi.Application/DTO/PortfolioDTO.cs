using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class PortfolioDTO
    {
        public int PortFolioId { get; set; }
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public string CryptoName { get; set; }
        public string Symbol { get; set; }
        public string Image { get; set; }
        public decimal Quantity { get; set; }
        public decimal AvgBuyPrice { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal TotalInvestment { get; set; }
        public decimal CurrentValue { get; set; }
        public decimal ProfitLoss { get; set; }
    }
}

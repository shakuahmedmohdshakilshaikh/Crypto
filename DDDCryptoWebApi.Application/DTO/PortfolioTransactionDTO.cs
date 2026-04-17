using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class PortfolioTransactionDTO
    {
        public int CryptoId { get; set; }
        public string CryptoName { get; set; }
        public string Symbol { get; set; }

        public string Image { get; set; }
        public string TransactionType { get; set; }
        public decimal Quantity { get; set; }
        public decimal PricePerUnit { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

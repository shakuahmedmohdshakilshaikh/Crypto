using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class DashboardTransactionDTO
    {
        public int TransactionId { get; set; }
        public string Type { get; set; }
        public string CryptoName { get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }
        public decimal PriceAtTime { get; set; }
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}

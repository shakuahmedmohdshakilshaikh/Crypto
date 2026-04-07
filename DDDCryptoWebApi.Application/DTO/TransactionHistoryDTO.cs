using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class TransactionHistoryDTO
    {
        public int TransactionId { get; set; }

      
        public int UserId { get; set; }

      
        public int WalletId { get; set; }

      
        public int CryptoId { get; set; }

        public string Type { get; set; }

      public string CryptoName {  get; set; }
        public string Symbol { get; set; }
        public decimal Quantity { get; set; }

       
        public decimal PriceAtTime { get; set; }

        public decimal TotalAmount { get; set; }

       public DateTime CreatedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class WalletTransactionDTO
    {
        public decimal Amount { get; set; }

        public string TransactionType { get; set; }

        public string Status { get; set; }

        public string PaymentMethod { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}

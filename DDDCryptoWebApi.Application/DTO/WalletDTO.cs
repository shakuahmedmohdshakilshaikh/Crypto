using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class WalletDTO
    {
        public int WalletId { get; set; }

        public int UserId { get; set; }

        public decimal Balance { get; set; }
    }
}

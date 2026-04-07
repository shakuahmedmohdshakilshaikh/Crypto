using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class BuyCryptoDTO
    {
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public decimal Amount { get; set; }
    }
}

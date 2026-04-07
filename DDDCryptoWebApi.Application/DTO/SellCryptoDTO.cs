using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class SellCryptoDTO
    {
        public int UserId { get; set; }
        public int CryptoId { get; set; }
        public decimal Quantity { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class CreateOrderDTO
    {
        public int UserId { get; set; }
        public decimal Amount { get; set; }
    }
}

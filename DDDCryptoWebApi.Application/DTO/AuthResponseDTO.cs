using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class AuthResponseDTO
    {
        public string Token { get; set; }
        public string Role { get; set; }
    }
}

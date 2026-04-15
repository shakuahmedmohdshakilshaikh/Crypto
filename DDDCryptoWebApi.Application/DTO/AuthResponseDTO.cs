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
        //public string Role { get; set; }
        public string Message { get; set; }

        public int UserId { get; set; }

        public string Email { get; set; }
    }
}

using DDDCryptoWebApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.Interface
{
    public interface IAuthService
    {
        Task<string> Register(RegisterDTO dto);
        Task<AuthResponseDTO> Login(LoginDTO dto);
    }
}

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

        Task<string> ForgotPassword(string email);

        Task<string> VerifyResetOtp(string email, string otp);

        Task<string> ResetPassword(ResetPasswordDTO dto);

        Task<Setup2FADTO> Setup2FA(string email);

        Task<AuthResponseDTO> Verify2FA(string email, string code);
    }
}

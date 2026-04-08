using Asp.Versioning;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiversion}/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            var data = await _service.Register(dto);
            return Ok(ApiResponse<string>.SuccessResponse(data,"Registered successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            var result = await _service.Login(dto);

            if (result == null)
                return Unauthorized();

            return Ok(ApiResponse<AuthResponseDTO>.SuccessResponse(result,"Login successfull"));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword(string email)
        {
            return Ok(await _service.ForgotPassword(email));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword(
            ResetPasswordDTO dto)
        {
          var data =  await _service.ResetPassword(dto);
            return Ok(ApiResponse<string>.SuccessResponse(data,"Password is reset"));
        }

        [HttpPost("setup-2fa")]
        public async Task<IActionResult> SetUp2Fa(string email) {

            var data = await _service.Setup2FA(email);
            return Ok(ApiResponse<Setup2FADTO>.SuccessResponse(data, "2fa enabled"));
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA(
            TwoFactorDTO dto)
        {
            var result = await _service.Verify2FA(
                dto.Email,
                dto.Code);

            return Ok(ApiResponse<AuthResponseDTO>.SuccessResponse(result, "otp is verify"));
        }

    }
}

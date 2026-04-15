using Asp.Versioning;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiVersion}/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            var data = await _service.Register(dto);
            return Ok(ApiResponse<string>.SuccessResponse(data, "Registered successfully"));
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var res = await _service.Login(dto);

            if (res == null)
                return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid email or password"));

            // New user -> show QR page
            if (res.Message == "2FA_REQUIRED")
            {
                var setup = await _service.Setup2FA(res.Email);

                return Ok(new
                {
                    message = "2FA_REQUIRED",
                    email = res.Email,
                    qrCode = setup.QrCodeImageBase64
                });
            }

            // Old user -> show OTP verify page
            if (res.Message == "VERIFY_2FA_REQUIRED")
            {
                return Ok(new
                {
                    message = "VERIFY_2FA_REQUIRED",
                    email = res.Email
                });
            }

            return BadRequest(ApiResponse<string>.ErrorResponse("Unexpected login flow"));
        }

        [HttpPost("forgot-password")]
        public async Task<IActionResult> ForgotPassword([FromQuery] string email)
        {
            var result = await _service.ForgotPassword(email);
            return Ok(ApiResponse<string>.SuccessResponse(result, "OTP sent"));
        }

        [HttpPost("reset-password")]
        public async Task<IActionResult> ResetPassword([FromBody] ResetPasswordDTO dto)
        {
            var data = await _service.ResetPassword(dto);
            return Ok(ApiResponse<string>.SuccessResponse(data, "Password reset"));
        }

        [HttpPost("setup-2fa")]
        public async Task<IActionResult> SetUp2Fa([FromQuery] string email)
        {
            var data = await _service.Setup2FA(email);
            return Ok(ApiResponse<Setup2FADTO>.SuccessResponse(data, "2FA setup QR generated"));
        }

        [HttpPost("verify-2fa")]
        public async Task<IActionResult> Verify2FA([FromBody] TwoFactorDTO dto)
        {
            var result = await _service.Verify2FA(dto.Email, dto.Code);

            if (result == null)
                return Unauthorized(ApiResponse<string>.ErrorResponse("Invalid OTP"));

            return Ok(ApiResponse<AuthResponseDTO>.SuccessResponse(result, "OTP verified successfully"));
        }
    }
}
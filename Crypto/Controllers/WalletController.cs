using Asp.Versioning;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers.v1
{
    [ApiVersion("1.0")]
    [Route("api/v{version:Apiversion}/wallet")]
    [ApiController]
    public class WalletController : ControllerBase
    {
        private readonly IWalletService _service;

        public WalletController(IWalletService service)
        {
            _service = service;
        }

        [HttpPost("create-order")]
        public async Task<IActionResult> CreateOrder(CreateOrderDTO dto)
        {
            var data = await _service.CreateRazorpayOrderAsync(dto);

            return Ok(ApiResponse<object>
                .SuccessResponse(data, "Order created"));
        }

        [HttpPost("verify-payment")]
        public async Task<IActionResult> VerifyPayment(PaymentVerifyDTO dto)
        {
            await _service.VerifyPaymentAndAddMoneyAsync(dto);

            return Ok(ApiResponse<string>
                .SuccessResponse("Done", "Money added successfully"));
        }

        [HttpPost("deduct-balance")]
        public async Task<IActionResult> DeductBalance(DeductBalanceDTO dto)
        {
            await _service.DeductBalanceAsync(dto);

            return Ok(ApiResponse<string>
                .SuccessResponse("Done", "Balance deducted"));
        }

        [HttpGet("balance/{userId}")]
        public async Task<IActionResult> GetBalance(int userId)
        {
            var balance = await _service.GetBalanceAsync(userId);

            return Ok(ApiResponse<decimal>
                .SuccessResponse(balance, "Balance fetched"));
        }
    }
}

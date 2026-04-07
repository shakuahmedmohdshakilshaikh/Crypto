using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PortfolioController : ControllerBase
    {
        private readonly IPortfolioService service;

        public PortfolioController(IPortfolioService service)
        {
            this.service = service;
        }

        [HttpPost("buy")]
        public async Task<IActionResult> Buy(BuyCryptoDTO dto)
        {
            await service.BuyCryptoAsync(dto);

            return Ok(ApiResponse<string>.SuccessResponse("Done", "Crypto bought successfully"));
        }

        [HttpPost("sell")]
        public async Task<IActionResult> Sell(SellCryptoDTO dto)
        {
            await service.SellCryptoAsync(dto);

            return Ok(ApiResponse<string>.SuccessResponse("Done", "Crypto sold successfully"));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetPortfolio(int userId)
        {
            var data = await service.GetPortfolioByUserIdAsync(userId);

            return Ok(ApiResponse<List<PortfolioDTO>>.SuccessResponse(data, "Portfolio fetched successfully"));
        }

        [HttpGet("transactions/{userId}")]
        public async Task<IActionResult> GetTransactions(int userId)
        {
            var data = await service.GetPortfolioTransactionsAsync(userId);

            return Ok(ApiResponse<List<PortfolioTransactionDTO>>.SuccessResponse(data, "Portfolio transactions fetched successfully"));
        }
    }
}

using Asp.Versioning;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiversion}/portfolio")]
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
        [ResponseCache(Duration = 60)] // caching 60 sec
        public async Task<IActionResult> GetPortfolio(int userId)
        {
            var data = await service.GetPortfolioByUserIdAsync(userId);

            return Ok(ApiResponse<List<PortfolioDTO>>.SuccessResponse(data, "Portfolio fetched successfully"));
        }

        [HttpGet("transactions/{userId}")]
        [ResponseCache(Duration = 60)] // caching 60 sec
        public async Task<IActionResult> GetTransactions(int userId)
        {
            var data = await service.GetPortfolioTransactionsAsync(userId);

            return Ok(ApiResponse<List<PortfolioTransactionDTO>>.SuccessResponse(data, "Portfolio transactions fetched successfully"));
        }
    }
}

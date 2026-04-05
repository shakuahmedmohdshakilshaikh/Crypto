using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CryptoController : ControllerBase
    {
        private readonly ICoinGeckoService _service;

        public CryptoController(ICoinGeckoService service)
        {
            _service = service;
        }

        [HttpGet("market")]
        public async Task<IActionResult> GetMarketData(
            int page = 1,
            int pageSize = 10)
        {
            var data = await _service
                .GetCryptoMarketDataAsync("inr", page, pageSize);

            return Ok(data);
        }
    }
}

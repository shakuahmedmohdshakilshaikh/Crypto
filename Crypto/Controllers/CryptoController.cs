using DDDCryptoWebApi.Application.DTO;
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

        [HttpPost("sync")]
        public async Task<IActionResult> Sync()
        {
            await _service.SyncCoinsToDatabaseAsync();

            return Ok(
                ApiResponse<string>
                    .SuccessResponse("Done", "Crypto data synced successfully")
            );
        }

        [HttpGet("fetch")]
        public async Task<IActionResult> Fetch()
        {
            var data = await _service.FetchCoinsAsync();

            return Ok(
                ApiResponse<List<CoinGeckoCoinDTO>>
                    .SuccessResponse(data, "Data fetched successfully")
            );
        }
    }
    }

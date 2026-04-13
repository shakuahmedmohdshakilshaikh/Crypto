using Asp.Versioning;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiversion}/crypto")]
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
        [ResponseCache(Duration = 60)]
        public async Task<IActionResult> Fetch()
        {
            var data = await _service.FetchCoinsAsync();

            return Ok(
                ApiResponse<List<CoinGeckoCoinDTO>>
                    .SuccessResponse(data, "Data fetched successfully")
            );
        }

        [HttpGet("get-coins")]
        [ResponseCache(Duration = 60)]
        public  async Task<IActionResult> GetCoins([FromQuery] CryptoPageRequestDTO request)
        {
           var data = await _service.GetCoinAsync(request);
            return Ok(ApiResponse<PagedResponse<CryptoListDTO>>.SuccessResponse(data, "Coins is fetched succesfully"));
        }
    }
    }

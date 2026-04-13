using Asp.Versioning;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiversion}/dashboard")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        IDashboardService service;

        public DashboardController(IDashboardService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ResponseCache(Duration = 60)]
        public  async Task<IActionResult> GetDashboard(int userId)
        {   
           var data = await service.GetDashboardAsync(userId);
            return Ok(ApiResponse<DashboardDTO>.SuccessResponse(data, "Dashboard Data Fetched"));
        }
    }
}

using Asp.Versioning;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:ApiVersion}/userfavourite")]
    [ApiController]
    public class UserFavouriteController : ControllerBase
    {
        private readonly IUserFavouriteService service;

        public UserFavouriteController(IUserFavouriteService service)
        {
            this.service = service;
        }

        [HttpPost]
        public async Task<IActionResult> Add(AddFavouriteDTO dto)
        {
            await service.AddAsync(dto);

            return Ok(ApiResponse<string>
                .SuccessResponse("Done", "Favourite added successfully"));
        }

        [HttpGet("{userId}")]
        public async Task<IActionResult> GetByUserId(int userId)
        {
            var data = await service.GetByUserIdAsync(userId);

            return Ok(ApiResponse<List<UserFavouriteDTO>>
                .SuccessResponse(data, "Favourites fetched successfully"));
        }

        [HttpDelete("{fid}")]
        public async Task<IActionResult> Delete(int fid)
        {
            await service.DeleteAsync(fid);

            return Ok(ApiResponse<string>
                .SuccessResponse("Done", "Favourite deleted successfully"));
        }
    }
}

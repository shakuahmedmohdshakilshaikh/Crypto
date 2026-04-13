using Asp.Versioning;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiversion}/user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        IUserService service;
        public UserController(IUserService service)
        {
            this.service = service;
        }

        [HttpGet]
        [ResponseCache(Duration = 60)] //cacahing
        public async Task<IActionResult> GetAll()
        {
            var data = await service.GetAllAsync();

            return Ok(ApiResponse<List<UserDTO>>
                .SuccessResponse(data, "Users fetched successfully"));
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 60)] //cacahing
        public async Task<IActionResult> GetById(int id)
        {
            var data = await service.GetByIdAsync(id);

            return Ok(ApiResponse<UserDTO>
                .SuccessResponse(data, "User fetched successfully"));
        }

        [HttpPut("{id}")]

        public async Task<IActionResult> Update(int id, UpdateUserDTO dto)
        {
            await service.UpdateAsync(id, dto);

            return Ok(ApiResponse<string>
                .SuccessResponse("Done", "User updated successfully"));
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await service.DeleteAsync(id);

            return Ok(ApiResponse<string>
                .SuccessResponse("Done", "User deleted successfully"));
        }
    }
}

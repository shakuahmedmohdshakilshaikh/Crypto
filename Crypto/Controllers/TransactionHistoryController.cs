using Asp.Versioning;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers.v1
{
    [ApiVersion("1")]
    [Route("api/v{version:apiversion}/transaction")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private ITransactionHistoryService service;

        public TransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            service = transactionHistoryService;
        }

        [HttpGet]
        [ResponseCache(Duration = 60)] // caching 60 sec
        public async Task<IActionResult> Getall()
        {
          var data =  await service.GetAllAsync();

            return Ok(ApiResponse<List<TransactionHistoryDTO>>.SuccessResponse(data, "TransactionHistoryController"));
        }

        [HttpGet("{id}")]
        [ResponseCache(Duration = 60)] // caching 60 sec
        public async Task<IActionResult> GetById(int id)
        {
            var data = await service.GetByIdAsync(id);

            return Ok(ApiResponse<TransactionHistoryDTO>
                .SuccessResponse(data, "Transaction history fetched successfully"));
        }

        [HttpGet("wallet/{userid}")]
        [ResponseCache(Duration = 60)] // caching 60 sec
        public async Task<IActionResult> GetByWalletId(int userid)
        {
            var data = await service.GetByWalletIdAsync(userid);

            return Ok(ApiResponse<List<TransactionHistoryDTO>>
                .SuccessResponse(data, "Wallet transaction history fetched successfully"));
        }
    }
}

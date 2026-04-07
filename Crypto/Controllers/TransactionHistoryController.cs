using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Crypto.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class TransactionHistoryController : ControllerBase
    {
        private ITransactionHistoryService service;

        public TransactionHistoryController(ITransactionHistoryService transactionHistoryService)
        {
            service = transactionHistoryService;
        }

        [HttpGet]
        public async Task<IActionResult> Getall()
        {
          var data =  await service.GetAllAsync();

            return Ok(ApiResponse<List<TransactionHistoryDTO>>.SuccessResponse(data, "TransactionHistoryController"));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var data = await service.GetByIdAsync(id);

            return Ok(ApiResponse<TransactionHistoryDTO>
                .SuccessResponse(data, "Transaction history fetched successfully"));
        }

        [HttpGet("wallet/{walletId}")]
        public async Task<IActionResult> GetByWalletId(int walletId)
        {
            var data = await service.GetByWalletIdAsync(walletId);

            return Ok(ApiResponse<List<TransactionHistoryDTO>>
                .SuccessResponse(data, "Wallet transaction history fetched successfully"));
        }
    }
}

using DDDCryptoWebApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.Interface
{
    public interface ITransactionHistoryService
    {
        Task<List<TransactionHistoryDTO>> GetAllAsync();

 
        Task<TransactionHistoryDTO> GetByIdAsync(int transactionId);

        Task<List<TransactionHistoryDTO>> GetByWalletIdAsync(int userid);

    }
}

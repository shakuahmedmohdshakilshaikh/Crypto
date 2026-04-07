using DDDCryptoWebApi.Application.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.Interface
{
    public interface IPortfolioService
    {
        Task BuyCryptoAsync(BuyCryptoDTO dto);
        Task SellCryptoAsync(SellCryptoDTO dto);
        Task<List<PortfolioDTO>> GetPortfolioByUserIdAsync(int userId);
        Task<List<PortfolioTransactionDTO>> GetPortfolioTransactionsAsync(int userId);
    }
}

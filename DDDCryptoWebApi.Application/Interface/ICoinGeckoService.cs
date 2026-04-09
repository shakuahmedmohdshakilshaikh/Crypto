using Azure;
using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.Interface
{
    public interface ICoinGeckoService
    {
        Task<List<CoinGeckoCoinDTO>> FetchCoinsAsync(
            string currency = "usd",
            int page = 1,
            int pageSize = 10);

        Task SyncCoinsToDatabaseAsync();

        Task<PagedResponse<CryptoListDTO>> GetCoinAsync(CryptoPageRequestDTO request);
    }
}

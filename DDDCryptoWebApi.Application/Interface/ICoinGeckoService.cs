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
        Task<List<CryptoMaster>> GetCryptoMarketDataAsync(
            string currency = "inr",
            int page = 1,
            int pageSize = 10
        );
    }
}

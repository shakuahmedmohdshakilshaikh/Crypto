using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Domain.Model;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Infrastructure.Services
{
    public class CoinGeckoService : ICoinGeckoService
    {
        private readonly HttpClient _httpClient;
        private readonly CoinGeckoSettings _settings;

        public CoinGeckoService(
            HttpClient httpClient,
            IOptions<CoinGeckoSettings> settings)
        {
            _httpClient = httpClient;
            _settings = settings.Value;
        }

        public async Task<List<CryptoMaster>> GetCryptoMarketDataAsync(
            string currency = "inr",
            int page = 1,
            int pageSize = 10)
        {
            var url =
                $"coins/markets?vs_currency=inr" +
                $"&page={page}&per_page={pageSize}";

            //var url =  $"/coins/markets?vs_currency=inr"

            var response = await _httpClient
                .GetFromJsonAsync<List<CoinGeckoCoinDTO>>(url);

            return response.Select(x => new CryptoMaster
            {
                CryptoName = x.Name,
                Symbol = x.Symbol.ToUpper(),
                Image = x.Image,
                CurrentPrice = x.CurrentPrice,
                MarketCap = x.MarketCap,
                CoinGeckoId = x.Id,
                IsActive = true,
                LastSyncedAt = DateTime.Now
            }).ToList();
        }
    }
}

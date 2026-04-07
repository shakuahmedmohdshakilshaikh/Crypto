using DDDCryptoWebApi.Application.DTO;
using DDDCryptoWebApi.Application.Interface;
using DDDCryptoWebApi.Domain.Model;
using DDDCryptoWebApi.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using System.Net.Http.Json;

namespace DDDCryptoWebApi.Infrastructure.Services
{
    public class CoinGeckoService : ICoinGeckoService
    {
        private readonly HttpClient _httpClient;
        private readonly ApplicationDbContext _context;

        public CoinGeckoService(
            HttpClient httpClient,
            ApplicationDbContext context)
        {
            _httpClient = httpClient;
            _context = context;
        }

        public async Task<List<CoinGeckoCoinDTO>> FetchCoinsAsync(
            string currency = "inr",
            int page = 1,
            int pageSize = 50)
        {
            var url =
                $"https://api.coingecko.com/api/v3/coins/markets" +
                $"?vs_currency={currency}" +
                $"&page={page}" +
                $"&per_page={pageSize}";

            var result = await _httpClient
                .GetFromJsonAsync<List<CoinGeckoCoinDTO>>(url);

            return result ?? new List<CoinGeckoCoinDTO>();
        }

        public async Task SyncCoinsToDatabaseAsync()
        {
            var coins = await FetchCoinsAsync();

            var inrCurrency = await _context.Currencies
                .FirstOrDefaultAsync(x => x.Symbol == "INR");

            if (inrCurrency == null)
            {
                throw new Exception("INR currency not found in Currencies table");
            }

            foreach (var coin in coins)
            {
                var existing = await _context.Cryptos
                    .FirstOrDefaultAsync(x => x.CoinGeckoId == coin.Id);

                if (existing == null)
                {
                    var newCoin = new CryptoMaster
                    {
                        CryptoName = coin.Name,
                        Symbol = coin.Symbol.ToUpper(),
                        CoinGeckoId = coin.Id,
                        CurrentPrice = coin.CurrentPrice,
                        MarketCap = coin.MarketCap,
                        Image = coin.Image,
                        IsActive = true,
                        LastSyncedAt = DateTime.Now,
                        CurrencyId = inrCurrency.CurrencyId
                    };

                    _context.Cryptos.Add(newCoin);
                }
                else
                {
                    existing.CurrentPrice = coin.CurrentPrice;
                    existing.MarketCap = coin.MarketCap;
                    existing.Image = coin.Image;
                    existing.LastSyncedAt = DateTime.Now;
                    existing.ModifiedAt = DateTime.Now;
                }
            }

            await _context.SaveChangesAsync();
        }
    }
}
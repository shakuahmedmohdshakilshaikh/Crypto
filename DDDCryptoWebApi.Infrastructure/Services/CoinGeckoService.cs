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
        private readonly ApplicationDbContext db;

        public CoinGeckoService( HttpClient httpClient, ApplicationDbContext context)
        {
            _httpClient = httpClient;
            db = context;
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

            var inrCurrency = await db.Currencies
                .FirstOrDefaultAsync(x => x.Symbol == "INR");

            if (inrCurrency == null)
            {
                throw new Exception("INR currency not found in Currencies table");
            }

            foreach (var coin in coins)
            {
                var existing = await db.Cryptos
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

                    db.Cryptos.Add(newCoin);
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

            await db.SaveChangesAsync();
        }


        public async Task<PagedResponse<CryptoListDTO>> GetCoinAsync(CryptoPageRequestDTO request)
        {
            var query = db.Cryptos.AsQueryable();

            // Search
            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                var search = request.SearchText.Trim().ToLower();

                query = query.Where(x =>
                    (x.CryptoName != null && x.CryptoName.ToLower().Contains(search)) ||
                    (x.Symbol != null && x.Symbol.ToLower().Contains(search)) ||
                    (x.CoinGeckoId != null && x.CoinGeckoId.ToLower().Contains(search))
                );
            }

            // Sorting
            var sortBy = request.SortBy?.Trim().ToLower();
            var sortOrder = request.SortOrder?.Trim().ToLower();

            switch (sortBy)
            {
                //case "cryptoname":
                case "name":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(x => x.CryptoName)
                        : query.OrderBy(x => x.CryptoName);
                    break;

                case "symbol":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(x => x.Symbol)
                        : query.OrderBy(x => x.Symbol);
                    break;

                //case "currentprice":
                case "price":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(x => x.CurrentPrice)
                        : query.OrderBy(x => x.CurrentPrice);
                    break;

                case "marketcap":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(x => x.MarketCap)
                        : query.OrderBy(x => x.MarketCap);
                    break;

                case "lastsyncedat":
                    query = sortOrder == "desc"
                        ? query.OrderByDescending(x => x.LastSyncedAt)
                        : query.OrderBy(x => x.LastSyncedAt);
                    break;

                default:
                    query = query.OrderBy(x => x.CryptoName);
                    break;
            }

            var totalRecord = await query.CountAsync();

            var totalPages = (int)Math.Ceiling((double)totalRecord / request.PageSize);

            var data = await query
                .Skip((request.PageNumber - 1) * request.PageSize)
                .Take(request.PageSize)
                .Select(x => new CryptoListDTO
                {
                    CryptoId = x.CryptoId,
                    CryptoName = x.CryptoName,
                    Symbol = x.Symbol,
                    CoinGeckoId = x.CoinGeckoId,
                    CurrentPrice = x.CurrentPrice,
                    MarketCap = x.MarketCap,
                    Image = x.Image,
                    IsActive = x.IsActive,
                    LastSyncedAt = x.LastSyncedAt,
                    CurrencyName = x.Currency.Currencyname
                })
                .ToListAsync();

            return new PagedResponse<CryptoListDTO>
            {
                Data = data,
                TotalPages = totalPages,
                PageNumber = request.PageNumber,
                PageSize = request.PageSize,
                HasNext = request.PageNumber < totalPages,
                HasPrevious = request.PageNumber > 1,
                TotalRecords = totalRecord,
                Nextpage = request.PageNumber < totalPages ? request.PageNumber + 1 : 0,
                PreviousPage = request.PageNumber > 1 ? request.PageNumber - 1 : 0,
            };
        }
    }
}
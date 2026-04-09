using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class CryptoListDTO
    {
        public int CryptoId { get; set; }
        public string CryptoName { get; set; }
        public string Symbol { get; set; }
        public string CoinGeckoId { get; set; }
        public decimal CurrentPrice { get; set; }
        public decimal MarketCap { get; set; }
        public string Image { get; set; }
        public bool IsActive { get; set; }
        public DateTime? LastSyncedAt { get; set; }
        public string CurrencyName { get; set; }
    }
}

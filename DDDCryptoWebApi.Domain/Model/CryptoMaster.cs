using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class CryptoMaster 
    {
        [Key]
        public int CryptoId { get; set; }

        [Required, StringLength(100)]
        public string CryptoName { get; set; }

        
        public string Image { get; set; }

        [Required, StringLength(100)]
        public string Symbol { get; set; }

        public decimal CurrentPrice { get; set; }
       
        public decimal MarketCap { get; set; }



        [Required, StringLength(100)]
        public string CoinGeckoId { get; set; }

        public bool IsActive { get; set; }

        public DateTime LastSyncedAt { get; set; }

        [Required]
        public int CurrencyId { get; set; }

        [ForeignKey("CurrencyId")]
        public Currency Currency { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }



    }
}

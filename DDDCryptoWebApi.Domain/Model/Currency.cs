using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class Currency : BaseEntity
    {
        [Key]
        public int CurrencyId { get; set; }

        [Required, StringLength(20)]
        public string Currencyname { get; set; }

        [Required, StringLength(5)]
        public string Symbol { get; set; }

        public ICollection<CryptoMaster>? Cryptos { get; set; }
    }
}

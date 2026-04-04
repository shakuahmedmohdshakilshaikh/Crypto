using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class Currency 
    {
        [Key]
        public int CurrencyId { get; set; }

        [Required, StringLength(20)]
        public string Currencyname { get; set; }

        [Required, StringLength(5)]
        public string Symbol { get; set; }


        //[ForeignKey("Createdby")]
        public int CreatedBy { get; set; }
        //public UserMaster Createdby { get; set; }


        public DateTime CreatedAt { get; set; }

        //[ForeignKey("ModifiedBy")]
        public int ModifiedBy { get; set; }
        //public UserMaster ModifyiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public ICollection<CryptoMaster>? Cryptos { get; set; }
    }
}

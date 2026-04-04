using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class CryptoMaster 
    {
        [Key]
        public int CryptoId { get; set; }

        public string CryptoName { get; set; }

        public string Symbol { get; set; }

        public int CoinGeckoId { get; set; }

        public string IsActive { get; set; }

        //public string LastSyncedAt { get; set; }

        //[ForeignKey("Createdby")]
        public int CreatedBy { get; set; }
        //public UserMaster Createdby { get; set; }


        public DateTime CreatedAt { get; set; }

        //[ForeignKey("ModifiedBy")]
        public int ModifiedBy { get; set; }
        //public UserMaster ModifyiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }


        //[ForeignKey("Currency")]
        public int CurrencyId { get; set; }
        //public Currency Currency { get; set; }



    }
}

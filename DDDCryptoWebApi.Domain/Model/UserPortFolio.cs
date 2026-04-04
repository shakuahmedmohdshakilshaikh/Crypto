using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class UserPortFolio : BaseEntity
    {

        [Key]
        public int PortFolioId { get; set; }

      
        [Column(TypeName = "decimal(20,2)")]
        public decimal AvgBuyPrice { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal TotalInvestment { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal Quantity { get; set; }

        [ForeignKey("CryptoMaster")]
        public int CryptoId { get; set; }
        public CryptoMaster CryptoMaster { get; set; }

        [ForeignKey("UserMaster")]
        public int UserId { get; set; }
        public UserMaster UserMaster { get; set; }


    }
}

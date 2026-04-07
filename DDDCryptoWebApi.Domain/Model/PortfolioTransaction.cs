using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class PortfolioTransaction
    {
        [Key]
        public int PortfolioTransactionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CryptoId { get; set; }

        [Required, StringLength(10)]
        public string TransactionType { get; set; } // Buy / Sell

        [Column(TypeName = "decimal(20,8)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal PricePerUnit { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal TotalAmount { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [ForeignKey("UserId")]
        public UserMaster User { get; set; }

        [ForeignKey("CryptoId")]
        public CryptoMaster Crypto { get; set; }
    }
}

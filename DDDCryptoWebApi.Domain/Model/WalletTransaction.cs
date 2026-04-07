using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public  class WalletTransaction
    {
        [Key]
        public int WalletTransactionId { get; set; }

        public string TransactionType { get; set; }

        [Required]
        public int WalletId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("WalletId")]
        public WalletMaster Wallet { get; set; }

        [ForeignKey("UserId")]
        public UserMaster User { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal Amount { get; set; }

        [Required, StringLength(20)]
        public string Status { get; set; }

        [Required, StringLength(50)]
        public string PaymentMethod { get; set; }

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

    }
}

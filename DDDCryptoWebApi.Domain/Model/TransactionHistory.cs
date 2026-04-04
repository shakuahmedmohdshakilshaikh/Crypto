using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class TransactionHistory 
    {
        [Key]
        public int TransactionId { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int WalletId { get; set; }

        [Required]
        public int CryptoId { get; set; }

        [ForeignKey("UserId")]
        public UserMaster User { get; set; }

        [ForeignKey("WalletId")]
        public WalletMaster Wallet { get; set; }

        [ForeignKey("CryptoId")]
        public CryptoMaster Crypto { get; set; }

        [Required, StringLength(30)]
        public string Type { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal Quantity { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal PriceAtTime { get; set; }

        [Column(TypeName = "decimal(20,2)")]
        public decimal TotalAmount { get; set; }



        //[ForeignKey("Createdby")]
        public int CreatedBy { get; set; }
        //public UserMaster Createdby { get; set; }


        public DateTime CreatedAt { get; set; }

        //[ForeignKey("ModifiedBy")]
        public int ModifiedBy { get; set; }
        //public UserMaster ModifyiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }
    }

}

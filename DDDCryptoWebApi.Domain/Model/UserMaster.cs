using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class UserMaster
    {
        [Key]
        public int UserId { get; set; }

        [Required, StringLength(30)]
        public string UserFullName { get; set; }

        
        [Required, StringLength(50)]
        public string Email { get; set; }

        [Required, StringLength(200)]
        public string PassWord { get; set; }

        [Required, StringLength(15)]
        public string PhoneNumber { get; set; }

        public string? Role { get; set; }

        public bool IsTwoFactorEnabled { get; set; }

        public string? TwoFactorSecretKey { get; set; }

        public string? ResetOtp { get; set; }
        public bool IsActive { get; set; } = true;

        public string? CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.Now;

        public string? ModifiedBy { get; set; }

        public DateTime? ModifiedAt { get; set; }

        public string? DeletedBy { get; set; }

        public DateTime? DeletedAt { get; set; }

        public ICollection<WalletMaster>? Wallets { get; set; }


    }
}

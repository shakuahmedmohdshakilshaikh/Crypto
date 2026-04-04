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


        [Required, EmailAddress, StringLength(25)]
        public string Email { get; set; }

        [Required, StringLength(50)]
        public string PassWord{ get; set; }

        public string PhoneNumber{ get; set; }

        public bool IsActive { get; set; }



        public ICollection<WalletMaster>? Wallets { get; set; }


    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class RegisterDTO
    {
        public string UserFullName { get; set; }

        [Required, EmailAddress, StringLength(25)]
        public string Email { get; set; }

        [Required, StringLength(50)]
        public string PassWord { get; set; }

        [Required, StringLength(12)]
        public string PhoneNumber { get; set; }




    }
}

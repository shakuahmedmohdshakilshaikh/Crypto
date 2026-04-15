using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Application.DTO
{
    public class LoginDTO
    {
        [Required, StringLength(50)]
        [RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", ErrorMessage = "Invalid email format")]
        public string Email { get; set; }

        [Required, StringLength(200)]
        [RegularExpression(
         @"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&]).{8,}$",
         ErrorMessage = "Password must be at least 8 characters and include uppercase, lowercase, number and special character"
)]
        public string PassWord { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class UserFavourite : BaseEntity
    {
        [Key]
        public int Fid { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int CryptoId { get; set; }

        [ForeignKey("UserId")]
        public UserMaster User { get; set; }

        [ForeignKey("CryptoId")]
        public CryptoMaster Crypto { get; set; }
    }
}

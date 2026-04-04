using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class WalletMaster
    {

        [Key]
        public int WalletId { get; set; }


        public decimal Balance { get; set; }

        public string CreatedBy { get; set; }

        public DateTime CreatedAt { get; set; }

        public string ModifiedBy { get; set; }

        public DateTime ModifiedAt { get; set; }

        public string DelatedBy { get; set; }
        public DateTime DelatedAt { get; set; }

        [ForeignKey("UserMaster")]
        public int UserId { get; set; }
        public UserMaster UserMaster{ get; set; }


        //[ForeignKey("Createdby")]
        //public int CreatedBy { get; set; }
        //public UserMaster Createdby { get; set; }


        //public DateTime CreatedAt { get; set; }

        //[ForeignKey("ModifiedBy")]
        //public int ModifiedBy { get; set; }
        //public UserMaster ModifyiedBy { get; set; }

        //public DateTime ModifiedAt { get; set; }
    }
}

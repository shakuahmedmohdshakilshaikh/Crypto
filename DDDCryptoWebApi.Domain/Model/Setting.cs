using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public class Setting : BaseEntity
    {
        [Key]
        public int SettingId { get; set; }

        [Required]
        public int UserId { get; set; }

        [ForeignKey("UserId")]
        public UserMaster User { get; set; }

        [Required, StringLength(30)]
        public string Theme { get; set; }

        public DateTime UpdatedAt { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DDDCryptoWebApi.Domain.Model
{
    public  abstract class BaseEntity
    {
    [Required]
    public int CreatedBy { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.Now;

    public int? ModifiedBy { get; set; }
    
    
    public DateTime? ModifiedAt { get; set; }

    public int DeletedBy { get; set; }

    public DateTime DeletedAt { get; set; } = DateTime.Now;

    [ForeignKey("DeletedBy")]
    public UserMaster DeletedUser { get; set; }

    [ForeignKey("CreatedBy")]
    public UserMaster CreatedUser { get; set; }

    [ForeignKey("ModifiedBy")]
    public UserMaster? ModifiedUser { get; set; }

    }
}

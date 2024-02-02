using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Contract
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractID { get; set; }
        [Required]
        public int BookingID { get; set; }
        [Required]
        [StringLength(50)]
        public string CarInformation { get; set; }
        [Required]
        public decimal Deposit { get; set; }
        public string? Note { get; set; }
        public virtual BookingDetail BookingDetail { get; set; }
    }
}

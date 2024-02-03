using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Contract
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int ContractID { get; set; }
        [StringLength(50)]
        public string CarInformation { get; set; }
        public decimal Deposit { get; set; }
        public string? Note { get; set; }
        public virtual BookingDetail BookingDetail { get; set; }
    }
}

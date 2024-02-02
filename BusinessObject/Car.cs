using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Car
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarID { get; set; }
        [Required]
        [StringLength(50)]
        public string CarName { get; set; }
        [Required]
        public int CarBrandID { get; set; }
        [Required]
        [StringLength(10)]
        public string CarPlate { get; set; }
        [Required]
        public decimal Deposit { get; set; }
        [Required]
        public decimal PricePerDay { get; set; }
        public int Status { get; set; }

        public virtual CarBrand CarBrand { get; set; }
        public virtual BookingDetail BookingDetail { get; set; }
    }
}

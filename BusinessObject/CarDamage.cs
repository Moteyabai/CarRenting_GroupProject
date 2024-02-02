using System.ComponentModel.DataAnnotations;

namespace BusinessObject
{
    public class CarDamage
    {
        [Required]
        public int BookingDetailID { get; set; }
        public string Damage { get; set; }
        public decimal Fined { get; set; }
        public virtual BookingDetail BookingDetail { get; set; }
    }
}

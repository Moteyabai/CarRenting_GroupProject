using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class CarDamage
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarDamageID { get; set; }
        [Required]
        public int BookingDetailID { get; set; }
        public string Damage { get; set; }
        public decimal Fined { get; set; }
        public string ImageCarDamage { get; set; }
        public virtual BookingDetail BookingDetail { get; set; }

    }
}

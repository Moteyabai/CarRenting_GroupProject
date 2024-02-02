using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Booking
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int BookingID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public DateTime BookingDate { get; set; }
        [Required]
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
        public virtual ICollection<BookingDetail> BookingDetails { get; set; }
        public virtual User User { get; set; }
    }
}

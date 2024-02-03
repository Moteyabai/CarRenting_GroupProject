using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class BookingDetail
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int DetailsID { get; set; }
        [Required]
        public int BookingID { get; set; }
        public int ContractID { get; set; }
        [Required]
        public int CarID { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? CarStatus { get; set; }
        public decimal? CarPrice { get; set; }
        public decimal? Fined { get; set; }
        public virtual Car Car { get; set; }
        public virtual Booking Booking { get; set; }
        public virtual Contract Contract { get; set; }
    }
}

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Transaction
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int TransactionID { get; set; }
        [Required]
        public int UserID { get; set; }
        [Required]
        public DateTime? PaymentDate { get; set; }
        [Required]
        public decimal Price { get; set; }
        public int Status { get; set; }
        public virtual User Users { get; set; }
    }
}

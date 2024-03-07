using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class TransactionViewDTO
    {
        public int TransactionID { get; set; }
        public int UserID { get; set; }
        public DateTime PaymentDate { get; set; }
        public decimal Price { get; set; }
        public int Status { get; set; }
    }
}

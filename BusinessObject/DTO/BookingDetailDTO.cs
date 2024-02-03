using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class BookingDetailDTO
    {
        public int CarID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string? CarStatus { get; set; }
        public decimal? CarPrice { get; set; }
        public decimal? Fined { get; set; }
    }
}

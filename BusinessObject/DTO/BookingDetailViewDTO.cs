using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class BookingDetailViewDTO
    {
        public int DetailsID { get; set; }
        public int BookingID { get; set; }
        public int ContractID { get; set; }
        public int CarID { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string CarStatus { get; set; }
        public decimal CarPrice { get; set; }
        public decimal? Fined { get; set; }
    }
}

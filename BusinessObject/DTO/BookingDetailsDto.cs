using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class BookingDetailsDto
    {
        public int BookingID { get; set; }
        public int UserID { get; set; }
        public DateTime BookingDate { get; set; }
        public decimal TotalPrice { get; set; }
        public int Status { get; set; }
        public List<BookingDetail1Dto> BookingDetails { get; set; }
        public UserDto User { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class BookingDTO
    {
        public int UserID { get; set; }
        public List<BookingDetailDTO> BookingDetails { get; set; }
    }
}

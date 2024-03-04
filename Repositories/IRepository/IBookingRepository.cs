using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IBookingRepository
    {
        int Create(BookingDTO dto);
        List<Booking> Bookings();
        void Update(int bookingID, BookingUpdateDTO dto);
        void UpdatePrice(int bookingID, BookingPriceDTO dto);
    }
}

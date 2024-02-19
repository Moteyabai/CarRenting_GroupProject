using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IBookingDetailRepository
    {
        int Create(int bookingID, int contractID, BookingDetailDTO dto);
        bool CheckBooking(int carId, DateTime start, DateTime end);
    }
}

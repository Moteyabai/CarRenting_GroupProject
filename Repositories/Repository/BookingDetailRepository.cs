using BusinessObject;
using BusinessObject.DTO;
using DataAccess;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class BookingDetailRepository : IBookingDetailRepository
    {
        public BookingDetail BookingDetails(int detailID)
        => BookingDetailDAO.Instance.BookingDetails(detailID);

        public List<BookingDetail> BookingDetailss()
        => BookingDetailDAO.Instance.BookingDetailss();

        public bool CheckBooking(int carId, DateTime start, DateTime end)
        => BookingDetailDAO.Instance.CheckBooking(carId, start, end);

        public int Create(int bookingID, int contractID, BookingDetailDTO dto)
        => BookingDetailDAO.Instance.Create(bookingID,contractID,dto);
    }
}

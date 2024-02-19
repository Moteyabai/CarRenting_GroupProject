using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class BookingDetailDAO
    {
        private static BookingDetailDAO instance = null;
        private static readonly object instanceLock = new object();
        public BookingDetailDAO() { }
        public static BookingDetailDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingDetailDAO();
                    }
                    return instance;
                }
            }
        }

        public int Create(int bookingID, int contractID, BookingDetailDTO dto)
        {
            try
            {
                using var context = new CarRentingDBContext();
                var check = context.Bookings.Find(bookingID);

                if (check==null)
                {
                    throw new Exception("Booking ID is not existing. Please choose a different booking id.");
                }
                else
                {

                    var bookingDetail = new BookingDetail 
                    {
                        BookingID = bookingID,
                        ContractID = contractID,
                        CarID = dto.CarID,
                        StartDate = dto.StartDate,
                        EndDate = dto.EndDate,
                        CarStatus = dto.CarStatus,
                        CarPrice = dto.CarPrice,
                        Fined = dto.Fined
                    };

                    context.BookingDetails.Add(bookingDetail);
                    context.SaveChanges();
                    return bookingDetail.DetailsID;
                }

             
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create booking detail: {ex.Message}");
            }
        }

        public bool CheckBooking(int carId, DateTime start, DateTime end)
        {
            try
            {
                using var context = new CarRentingDBContext();

                DateTime startUtc = start.ToUniversalTime();
                DateTime endUtc = end.ToUniversalTime();
                DateTime nowUtc = DateTime.UtcNow;

                bool isCarBooked = context.BookingDetails
                    .Any(b =>
                        b.CarID == carId &&
                        ((startUtc >= b.StartDate && startUtc < b.EndDate) || (endUtc > b.StartDate && endUtc <= b.EndDate) || (startUtc <= b.StartDate && endUtc >= b.EndDate))
                    );

                return isCarBooked; // Return true if the car is booked within the specified date range
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to check BookingDetail: {ex.Message}");
            }
        }


    }
}

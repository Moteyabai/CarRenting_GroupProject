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
    public class BookingDAO
    {
        private static BookingDAO instance = null;
        private static readonly object instanceLock = new object();
        public BookingDAO() { }
        public static BookingDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new BookingDAO();
                    }
                    return instance;
                }
            }
        }

        public int Create(BookingDTO dto)
        {
            try
            {
                using var context = new CarRentingDBContext();

                if (IsUserIdExist(dto.UserID))
                {
                    throw new Exception("UserId is not already. Please choose a different userId.");
                }

                var newBooking = new Booking
                {
                    UserID = dto.UserID,
                    BookingDate = DateTime.Now,
                    TotalPrice = 0,
                    Status = 1
                };

                context.Bookings.Add(newBooking);
                context.SaveChanges();

                return newBooking.BookingID; // Assuming 'Id' is the primary key property of the Booking entity
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create booking: {ex.Message}");
            }
        }


        public bool IsUserIdExist(int userId)
        {
            using var context = new CarRentingDBContext();
            return !context.Users.Any(c => c.UserID == userId);
        }

        public List<Booking> Bookings()
        {
            List<Booking> bookings;
            try
            {
                using var context = new CarRentingDBContext();
                bookings = context.Bookings
                    .Include(b => b.BookingDetails)
                        .ThenInclude(bd => bd.Car)
                            .ThenInclude(c => c.CarBrand) // Thêm ThenInclude cho CarBrand
                    .Include(b => b.User)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookings;
        }

        public void Update(int bookingID, BookingUpdateDTO dto)
        {
            try
            {
                using var context = new CarRentingDBContext();
                var existingBooking = context.Bookings.Find(bookingID);

                if (existingBooking != null)
                {
                    existingBooking.Status = dto.Status;

                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Booking not found for update.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update booking: {ex.Message}");
            }
        }

        public void UpdatePrice(int bookingID, BookingPriceDTO dto)
        {
            try
            {
                using var context = new CarRentingDBContext();
                var existingBooking = context.Bookings.Find(bookingID);

                if (existingBooking != null)
                {
                    existingBooking.TotalPrice = dto.TotalPrice;

                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Booking not found for update.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update booking: {ex.Message}");
            }
        }
    }
}

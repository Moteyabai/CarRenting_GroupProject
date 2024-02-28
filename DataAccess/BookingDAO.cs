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
                bookings = context.Bookings.Include(c => c.BookingDetails).AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return bookings;
        }
    }
}

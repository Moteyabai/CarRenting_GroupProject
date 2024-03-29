﻿using AutoMapper;
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

                var Contract = context.Contracts.Find(contractID);

                if (Contract == null)
                {
                    throw new Exception("Contract ID is not existing. Please choose a different Contract id.");
                }

                var Booking = context.Bookings.Find(bookingID);

                if (Booking == null)
                {
                    throw new Exception("Booking ID is not existing. Please choose a different booking id.");
                }
                else
                {
                    var Car = context.Cars.Find(dto.CarID);

                    if (Car == null)
                    {
                        throw new Exception("Car ID is not existing. Please choose a different Car id.");
                    }

                    var bookingDetail = new BookingDetail 
                    {
                        Booking = Booking,
                        Car = Car,
                        Contract = Contract,
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

                DateTime nowUtc = DateTime.Now.Date;
                DateTime startUtcDate = start.Date;
                DateTime endUtcDate = end.Date;

                if (startUtcDate < nowUtc)
                {
                    return true;
                }

                if (endUtcDate < startUtcDate)
                {
                    return true;
                }

                if (endUtcDate < nowUtc)
                {
                    return true;
                }

                // Kiểm tra xem xe có được đặt trong khoảng thời gian cụ thể không
                bool isCarBooked = context.BookingDetails
                    .Any(b =>
                        b.CarID == carId &&
                        ((startUtcDate >= b.StartDate && startUtcDate < b.EndDate) || (endUtcDate > b.StartDate && endUtcDate <= b.EndDate) || (startUtcDate <= b.StartDate && endUtcDate >= b.EndDate))
                    );

                return isCarBooked; // Trả về true nếu chiếc xe đã được đặt trong khoảng thời gian cụ thể
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to check BookingDetail: {ex.Message}");
            }
        }


        public BookingDetail BookingDetails(int detailID)
        {
            try
            {
                using var context = new CarRentingDBContext();
                return context.BookingDetails.Find(detailID);
            }
            catch(Exception e)
            {
                throw new Exception($"Failed to get BookingDetail: {e.Message}");
            }
        }

        public List<BookingDetail> BookingDetailss()
        {
            try
            {
                using var context = new CarRentingDBContext();
                return context.BookingDetails.ToList();
            }
            catch (Exception e)
            {
                throw new Exception($"Failed to get BookingDetail: {e.Message}");
            }
        }



    }
}

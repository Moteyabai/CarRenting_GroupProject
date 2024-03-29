﻿using BusinessObject;
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
    public class BookingRepository : IBookingRepository
    {
        public List<Booking> Bookings()
        => BookingDAO.Instance.Bookings();

        public int Create(BookingDTO dto)
        => BookingDAO.Instance.Create(dto);

        public void Update(int bookingID, BookingUpdateDTO dto)
        => BookingDAO.Instance.Update(bookingID,dto);

        public void UpdatePrice(int bookingID, BookingPriceDTO dto)
        => BookingDAO.Instance.UpdatePrice(bookingID, dto);
    }
}

﻿using BusinessObject.DTO;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repositories.IRepository;
using Repositories.Repository;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using DataAccess;
using Microsoft.AspNetCore.OData.Formatter;
using Microsoft.AspNetCore.Authorization;

namespace CarRenting_API.Controllers
{
    public class BookingController : ODataController
    {
        private IBookingRepository bookingRepository = new BookingRepository();
        private IBookingDetailRepository bookingDetailRepository = new BookingDetailRepository();
        private ICarDamageRepository carDamageRepository = new CarDamageRepository();
        private IContractRepository contractRepository = new ContractRepository();

        [EnableQuery(MaxExpansionDepth = 3)]
        public ActionResult<IQueryable<Booking>> Get()
        {
            return Ok(bookingRepository.Bookings());
        }

        [Authorize]
        [EnableQuery]
        public ActionResult Post([FromBody] BookingDTO dto)
        {
            try
            {
                int bookingID = 0;
                foreach (var bookingDetail in dto.BookingDetails)
                {
                    bool check = bookingDetailRepository.CheckBooking(bookingDetail.CarID, bookingDetail.StartDate, bookingDetail.EndDate);
                    if (bookingDetailRepository.CheckBooking(bookingDetail.CarID, bookingDetail.StartDate, bookingDetail.EndDate)==false)
                    {
                        bookingID = bookingRepository.Create(dto);
                        foreach (var bookingDetail1 in dto.BookingDetails)
                        {
                            int contractID = contractRepository.Create();
                            int detailID = bookingDetailRepository.Create(bookingID, contractID, bookingDetail1);
                            carDamageRepository.Create(detailID);
                            var getbookingDetail = bookingDetailRepository.BookingDetails(detailID);
                            if (getbookingDetail != null)
                            {
                                int totalDate = (int)(getbookingDetail.EndDate - getbookingDetail.StartDate).Value.TotalDays + 1;
                                decimal? price = getbookingDetail.CarPrice;
                                decimal total = price * totalDate ?? 0;
                                var update = new BookingPriceDTO
                                {
                                    TotalPrice = total
                                };
                                bookingRepository.UpdatePrice(bookingID, update);
                            }

                        }

                        return Ok("Create sucessful.");
                    }
                    else
                    {
                        return BadRequest("The car is currently rented for that day, please come back later.");
                    }
                }
                return BadRequest("No booking details provided.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Exception in Post: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [EnableQuery]
        public IActionResult Put([FromRoute] int key, [FromBody] BookingUpdateDTO dto)
        {
            bookingRepository.Update(key, dto);
            return Ok();
        }
    }
}

using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.IRepository;
using Repositories.Repository;

namespace CarRenting_API.Controllers
{
    public class CartController : ControllerBase
    {
        private IBookingDetailRepository bookingDetailRepository = new BookingDetailRepository();
        private static List<BookingDetailDTO> BookingDetails = new List<BookingDetailDTO>();

        [HttpPost("cart")]
        public ActionResult<List<BookingDetailDTO>> CreateBookingDetail([FromBody] BookingDetailDTO bookingDetail)
        {
            if (bookingDetail == null)
            {
                return BadRequest("Invalid data");
            }

            if (BookingDetails.Any(bd => bd.CarID == bookingDetail.CarID))
            {
                return BadRequest("CarID already exists in the list.");
            }

            if (!IsValidBookingDetail(bookingDetail, out var validationErrorMessage))
            {
                return BadRequest(validationErrorMessage);
            }

            bool check = bookingDetailRepository.CheckBooking(bookingDetail.CarID, bookingDetail.StartDate, bookingDetail.EndDate);
            if (check)
            {
                // Car is available, you can perform further actions or return a response
                BookingDetails.Add(bookingDetail);

                return Ok(BookingDetails);
            }
            else
            {
                // Car is not available, you can return a response indicating that the car is booked
                return BadRequest("Car is not available. Please choice car other.");
            }

            
        }

        private bool IsValidBookingDetail(BookingDetailDTO bookingDetail, out string errorMessage)
        {
            errorMessage = null;

            if (bookingDetail.StartDate != null && bookingDetail.EndDate != null)
            {
                // Check if StartDate is in the past
                if (bookingDetail.StartDate < DateTime.Now.Date)
                {
                    errorMessage = "StartDate cannot be in the past.";
                    return false;
                }

                // Check if StartDate is greater than or equal to EndDate
                if (bookingDetail.StartDate > bookingDetail.EndDate)
                {
                    errorMessage = "StartDate must be less than or equal to EndDate.";
                    return false;
                }
            }

            return true;
        }


        [HttpGet("booking-details")]
        public ActionResult<List<BookingDetailDTO>> GetBookingDetails()
        {
            return Ok(BookingDetails);
        }

        [HttpDelete("{id}")]
        public ActionResult<List<BookingDetailDTO>> DeleteBookingDetail(int id)
        {
            var bookingDetailToRemove = BookingDetails.FirstOrDefault(b => b.CarID == id);

            if (bookingDetailToRemove == null)
            {
                return NotFound("Booking detail not found");
            }

            // Xóa phần tử khỏi danh sách
            BookingDetails.Remove(bookingDetailToRemove);

            return Ok(BookingDetails);
        }

        [HttpDelete("clear")]
        public ActionResult ClearAllBookingDetails()
        {
            BookingDetails.Clear();

            return Ok("All booking details deleted successfully");
        }
    }
}

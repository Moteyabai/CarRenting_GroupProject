using BusinessObject.DTO;
using BusinessObject;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.IRepository;
using Repositories.Repository;

namespace CarRenting_API.Controllers
{
    public class BookingDetailController : ODataController
    {
        private IBookingDetailRepository bookingDetailRepository = new BookingDetailRepository();

        [EnableQuery]
        public ActionResult Post([FromRoute] int key, [FromRoute] int key1, [FromBody] BookingDetailDTO dto)
        {
            try
            {
                bookingDetailRepository.Create(key,key1,dto);

                return Ok("Create sucessful.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Exception in Post: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }
    }
}

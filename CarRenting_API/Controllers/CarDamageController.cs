using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.IRepository;
using Repositories.Repository;

namespace CarRenting_API.Controllers
{
    public class CarDamageController : ODataController
    {
        private ICarDamageRepository carDamageRepository = new CarDamageRepository();

        [EnableQuery]
        public ActionResult<IQueryable<CarDamage>> Get()
        {
            return Ok(carDamageRepository.CarDamages());
        }


        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] CarDamageDTO dto)
        {
            try
            {
                carDamageRepository.Update(key, dto);

                return Ok("Update sucessful.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Exception in Post: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [EnableQuery]
        public ActionResult Post(int key)
        {
            try
            {
                carDamageRepository.Create(key);

                return Ok("Create successful.");
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

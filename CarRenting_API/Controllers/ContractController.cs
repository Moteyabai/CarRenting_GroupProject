using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Repositories.IRepository;
using Repositories.Repository;

namespace CarRenting_API.Controllers
{
    public class ContractController : ODataController
    {
        private IContractRepository contractRepository = new ContractRepository();

        [Authorize]
        [EnableQuery]
        public ActionResult<IQueryable<Contract>> Get()
        {
            return Ok(contractRepository.Contracts());
        }

        [Authorize]
        [EnableQuery]
        public ActionResult Post()
        {
            try
            {
                contractRepository.Create();

                return Ok("Create sucessful.");
            }
            catch (Exception ex)
            {
                // Log the exception or handle it appropriately
                Console.WriteLine($"Exception in Post: {ex}");
                return StatusCode(500, "Internal Server Error");
            }
        }

        [Authorize]
        [EnableQuery]
        public ActionResult Put([FromRoute] int key, [FromBody] ContractDTO dto)
        {
            try
            {
                contractRepository.Update(key, dto);

                return Ok("Update sucessful.");
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

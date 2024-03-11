using BusinessObject;
using BusinessObject.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Repositories.IRepository;
using Repositories.Repository;

namespace CarRenting_API.Controllers
{
    public class TransactionController : Controller
    {
        private ITransactionRepository transactionRepository = new TransactionRepository();

        [Authorize]
        [EnableQuery]
        public ActionResult<IQueryable<Transaction>> Get()
        {
            return Ok(transactionRepository.Transactions());
        }

        [Authorize]
        [EnableQuery]
        public ActionResult Post([FromBody] TransactionDTO dto)
        {
            try
            {
                transactionRepository.Create(dto);

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

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using Repositories.IRepository;
using Repositories.Repository;
using AutoMapper;
using BusinessObject.DTO;

namespace CarRenting_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private ICarRepository _carRepository = new CarRepository();
        private readonly IMapper _mapper;
        private string Message;

        public CarsController(IMapper mapper)
        {
            _mapper = mapper;
        }
       
        // GET: api/Cars
        [HttpGet("CarList")]
        public ActionResult<IEnumerable<Car>> GetUsers()
        {
            List<Car> list = new List<Car>();
            list = _carRepository.GetListCar();
            if (list.Count == 0) {
                Message = "List is empty!";

                return NotFound(Message);
            }
            return Ok(list);
        }

        // GET: api/Cars/5
        [HttpGet("Search/{name}")]
        public ActionResult<IEnumerable<Car>> SearchCarByName(string name)
        {
            List<Car> list = new List<Car>();
            list = _carRepository.SearchCarByName(name);
            if (list.Count == 0) {
                Message = "No Car Found";
                return NotFound(Message);
            }
            return Ok(list);
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpGet("GetCar{id}")]
        public ActionResult<Car> GetCarByID(int id)
        {
            var u = _carRepository.GetCarByID(id);
            if (u == null) {
                return NotFound("No user found!");
            }
            return Ok(u);
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddCar")]
        public ActionResult<Car> Register(UserRegisterDTO userRegisterDTO)
        {
            var car = _mapper.Map<Car>(userRegisterDTO);
            var list = _carRepository.GetListCar();
            foreach (Car us in list) {
                if (us.CarName.Equals(car.CarName)) {
                    Message = "CarName existed!";
                    return NotFound(Message);
                }
            }
            
           
            _carRepository.AddCar(car);
            Message = "New User Added!";
            return Ok(Message);
        }

        // DELETE: api/Cars/5
        [HttpDelete("Delete{id}")]
        public IActionResult DeleteUser(int id)
        {
            var u = _carRepository.GetCarByID(id);
            if (u == null) {
                Message = "No User Found!";
                return NotFound(Message);
            }
            Message = "Deleted " + u.CarName;
            _carRepository.DeleteCar(id);
            return Ok(Message);
        }

        // UPDATE: api/Cars/5
        [HttpPut("Update")]
        public IActionResult UpdateUser(CarUpdateDTO carUpdateDTO)
        {
            var car = _mapper.Map<Car>(carUpdateDTO);
            var u = _carRepository.GetCarByID(car.CarID);
            if (u == null) {
                Message = "No Car Found!";
                return NotFound(Message);
            }
            car.CarBrandID = u.CarBrandID;
            car.Status = u.Status;
            _carRepository.UpdateCar(car);
            Message = "Car Updated!";
            return Ok(Message);


        }
    }
}

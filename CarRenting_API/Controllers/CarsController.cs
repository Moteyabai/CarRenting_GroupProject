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
using BusinessObject.Models.CarModels;
using GrpcService;
using GrpcService.Services;
using Microsoft.AspNetCore.Authorization;

namespace CarRenting_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarsController : ControllerBase
    {
        private ICarRepository _carRepository = new CarRepository();
        private readonly IMapper _mapper;
        private string Message;
        CarService _carService = new CarService();

        public CarsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        [Authorize]
        [HttpGet("grpc")]
        public CarListResponse GetCarList(int carId)
        {
            CarRequest carRequest = new CarRequest();
            carRequest.ID = carId;
            return _carService.GetCar(carRequest, null).Result;
        }

        [Authorize]
        // GET: api/Cars
        [HttpGet("Carlist")]
        public ActionResult<IEnumerable<CarViewModels>> GetListCars()
        {
            try
            {
                List<CarViewModels> list = new List<CarViewModels>();
                var cars = _carRepository.GetListCar();
                if (cars == null)
                {
                    return NotFound("No Cars Found!");
                }
                else
                {
                    list = _mapper.Map<List<CarViewModels>>(cars);    
                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        // GET: api/Cars/5
        [HttpGet("Search/{name}")]
        public ActionResult<IEnumerable<Car>> SearchCarByName(string name)
        {
            List<Car> list = _carRepository.SearchCarByName(name);
            var carList = _mapper.Map<List<CarViewModels>>(list);
            if (carList.Count == 0) {
                Message = "No Car Found";
                return NotFound(Message);
            }
            return Ok(carList);
        }

        // PUT: api/Cars/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpGet("GetCar{id}")]
        public ActionResult<Car> GetCarByID(int id)
        {
            var car = _carRepository.GetCarByID(id);
            var c = _mapper.Map<CarViewModels>(car);
            if (c == null) {
                Message = "No Car Found!";

                return NotFound(Message);
            }
            return Ok(c);
        }

        // POST: api/Cars
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize]
        [HttpPost("AddCar")]
        public ActionResult<CarAddDTO> Addcar(CarAddDTO carAddDTO)
        {
            var car = _mapper.Map<Car>(carAddDTO);
            var list = _carRepository.GetListCar();
            foreach (Car c in list) {
                if (c.CarName.Equals(car.CarName)) {
                    Message = "CarName Existed!";
                    return NotFound(Message);
                }
            }
            car.Status = 1;
            
            _carRepository.AddCar(car);
            Message = "New Car Added!";
            return Ok(Message);
        }

        // DELETE: api/Cars/5
        [Authorize]
        [HttpDelete("DeleteCar{id}")]
        public IActionResult DeleteCar(int id)
        {
            var c = _carRepository.GetCarByID(id);
            if (c == null) {
                Message = "No Car Found!";
                return NotFound(Message);
            }
            Message = "Deleted " + c.CarName;
            _carRepository.DeleteCar(id);
            return Ok(Message);
        }

        // UPDATE: api/Cars/5
        [Authorize]
        [HttpPut("UpdateCar")]
        public IActionResult UpdateCar(CarUpdateDTO carUpdateDTO)
        {
            var car = _mapper.Map<Car>(carUpdateDTO);
            var u = _carRepository.GetCarByID(car.CarID);
            if (u == null) {
                Message = "No Car Found!";
                return NotFound(Message);
            }
            
            car.Status = u.Status;
            _carRepository.UpdateCar(car);
            Message = "Car Updated!";
            return Ok(Message);


        }
    }
}

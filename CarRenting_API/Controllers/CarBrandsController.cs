using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusinessObject;
using DataAccess;
using AutoMapper;
using Repositories.IRepository;
using Repositories.Repository;
using BusinessObject.Models.CarModels;
using BusinessObject.DTO;

namespace CarRenting_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarBrandsController : ControllerBase
    {
        private ICarBrandRepository _carbrandRepository = new CarBrandRepository();
        private readonly IMapper _mapper;
        private string Message;
        public CarBrandsController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/CarBrands
        [HttpGet("CarBrandlist")]
        public ActionResult<IEnumerable<BrandCarDTO>> GetListCars()
        {
            try {
                List<BrandCarDTO> list = new List<BrandCarDTO>();
                var cars = _carbrandRepository.GetListCarBrand();
                if (cars == null) {
                    return NotFound("No CarBrand Found!");
                }
                else {
                    list = _mapper.Map<List<BrandCarDTO>>(cars);
                    return Ok(list);
                }
            }
            catch (Exception ex) {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/CarBrands/5
        [HttpGet("Search/{name}")]
        public ActionResult<IEnumerable<CarBrand>> SearchCarBrandByName(string name)
        {
            List<CarBrand> list = _carbrandRepository.SearchCarBrandByName(name);
            var carBrandList = _mapper.Map<List<BrandCarDTO>>(list);
            if (carBrandList.Count == 0) {
                Message = "No CarBrand Found";
                return NotFound(Message);
            }
            return Ok(carBrandList);
        }

        // PUT: api/CarBrands/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
       
        [HttpGet("GetCar{id}")]
        public ActionResult<CarBrand> GetCarByID(int id)
        {
            var car = _carbrandRepository.GetCarBrandByID(id);
            var c = _mapper.Map<BrandCarDTO>(car);
            if (c == null) {
                Message = "No CarBrand Found!";

                return NotFound(Message);
            }
            return Ok(c);
        }

        // POST: api/CarBrands
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("AddCarBrand")]
        public ActionResult<CarBrandAddDTO> Addcar(CarBrandAddDTO carBrandAddDTO)
        {
            var car = _mapper.Map<CarBrand>(carBrandAddDTO);
            var list = _carbrandRepository.GetListCarBrand();
            foreach (CarBrand c in list) {
                if (c.Name.Equals(car.Name)) {
                    Message = "CarBrandName Existed!";
                    return NotFound(Message);
                }
            }


            _carbrandRepository.AddCarBrand(car);
            Message = "New CarBrand Added!";
            return Ok(Message);
        }

        // DELETE: api/CarBrands/5
        [HttpDelete("DeleteCarBrand{id}")]
        public IActionResult DeleteCar(int id)
        {
            var c = _carbrandRepository.GetCarBrandByID(id);
            if (c == null) {
                Message = "No CarBrand Found!";
                return NotFound(Message);
            }
            Message = "Deleted " + c.Name;
            _carbrandRepository.DeleteCarBrand(id);
            return Ok(Message);
        }

        // UPDATE: api/Cars/5
        [HttpPut("UpdateCarBrand")]
        public IActionResult UpdateCar(CarBrandUpdateDTO carBrandateDTO)
        {
            var car = _mapper.Map<CarBrand>(carBrandateDTO);
            var u = _carbrandRepository.GetCarBrandByID(car.CarBrandID);
            if (u == null) {
                Message = "No Car Found!";
                return NotFound(Message);
            }
            car.CarBrandID = u.CarBrandID;
            _carbrandRepository.UpdateCarBrand(car);
            Message = "CarBrand Updated!";
            return Ok(Message);


        }
    }
}

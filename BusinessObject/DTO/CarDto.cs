using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class CarDto
    {
        public int CarID { get; set; }
        public string CarName { get; set; }
        public string CarPlate { get; set; }
        public decimal Deposit { get; set; }
        public decimal PricePerDay { get; set; }
        public int Status { get; set; }
        public string ImageCar { get; set; }
        public int Seat { get; set; }
        public string Description { get; set; }
        public CarBrandDto CarBrand { get; set; }
    }
}

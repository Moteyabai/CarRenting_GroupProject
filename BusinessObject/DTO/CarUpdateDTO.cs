using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class CarUpdateDTO
    {
        [Required]
        public int CarID { get; set; }   
        public string CarName { get; set; }
        public int CarBrandID { get; set; }
        public string CarPlate {  get; set; }
        [Range(0, int.MaxValue)]
        public decimal Deposit { get; set; }
        [Range(0, int.MaxValue)]
        public double PricePerDay { get; set;}
        public string ImageCar { get; set; }
        [Range(0, int.MaxValue)]
        public int Seat { get; set; }    
        public string Description { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.CarModels
{
    public class CarViewModels
    {
        public int CarID { get; set; }
        public string CarName { get; set; }
        public string CarBrand { get; set; }
        public string CarPlate { get; set; }
        public decimal Deposit { get; set; }
        public decimal PricePerDay { get; set; }
        public int Status { get; set; }
    }
}

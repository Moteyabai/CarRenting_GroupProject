using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class CarUpdateDTO
    {
        public string CarID { get; set; }
        public string CarName { get; set; }
       
        public string CarPlate {  get; set; }
        public string Deposit { get; set; }
        public double PricePerDay { get; set;}
      
    }
}

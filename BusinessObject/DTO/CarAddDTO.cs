﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class CarAddDTO
    {
        
        public string CarName { get; set; }
        public int CarBrandID { get; set; }
        [StringLength(10)]
        public string CarPlate { get; set; }
        public decimal Deposit { get; set; }
        public decimal PricePerDay { get; set; }
        public string ImageCar { get; set; }
        public int Seat { get; set; }
        public string Description { get; set; }
    }
}

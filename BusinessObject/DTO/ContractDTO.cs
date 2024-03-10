using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.DTO
{
    public class ContractDTO
    {
        public string CarInformation { get; set; }
        public decimal Deposit { get; set; }
        public string? Note { get; set; }
        public int? Status { get; set; }
    }
}

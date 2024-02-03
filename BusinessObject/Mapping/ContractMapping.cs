using AutoMapper;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Mapping
{
    public class ContractMapping : Profile
    {
        public ContractMapping()
        {
            CreateMap<ContractDTO, Contract>();
        }
    }
}

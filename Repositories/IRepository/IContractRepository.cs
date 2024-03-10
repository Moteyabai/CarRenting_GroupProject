using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface IContractRepository
    {
        int Create();
        void Update(int contractId, ContractDTO dto);
        void UpdateStatus(int contractId, BookingUpdateDTO dto);  
        List<Contract> Contracts();
    }
}

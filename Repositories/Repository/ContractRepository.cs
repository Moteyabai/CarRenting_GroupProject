using BusinessObject;
using BusinessObject.DTO;
using DataAccess;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class ContractRepository : IContractRepository
    {
        public List<Contract> Contracts()
        => ContractDAO.Instance.Contracts();

        public int Create()
        => ContractDAO.Instance.Create();

        public void Update(int contractId, ContractDTO dto)
        => ContractDAO.Instance.Update(contractId,dto);

        public void UpdateStatus(int contractId, BookingUpdateDTO dto)
        => ContractDAO.Instance.UpdateStatus(contractId,dto);
    }
}

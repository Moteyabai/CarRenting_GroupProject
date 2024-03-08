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
    public class TransactionRepository : ITransactionRepository
    {
        public void Create(TransactionDTO dto)
        => TransactionDAO.Instance.Create(dto);

        public List<Transaction> Transactions()
        => TransactionDAO.Instance.Transactions();
    }
}

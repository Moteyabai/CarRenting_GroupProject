using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ITransactionRepository
    {
        void Create(TransactionDTO dto);

        List<Transaction> Transactions();
    }
}

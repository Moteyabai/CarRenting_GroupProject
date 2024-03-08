using BusinessObject.DTO;
using BusinessObject;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class TransactionDAO
    {
        private static TransactionDAO instance = null;
        private static readonly object instanceLock = new object();
        public TransactionDAO() { }
        public static TransactionDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new TransactionDAO();
                    }
                    return instance;
                }
            }
        }

        public void Create(TransactionDTO dto)
        {
            try
            {
                using var context = new CarRentingDBContext();

                var newTransaction = new Transaction
                {
                    UserID = dto.UserID,
                    PaymentDate = DateTime.Now,
                    Price = dto.Price,
                    Status = 1
                };

                context.Transactions.Add(newTransaction);
                context.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create payment: {ex.Message}");
            }
        }

        public List<Transaction> Transactions()
        {
            List<Transaction> transactions;
            try
            {
                using var context = new CarRentingDBContext();
                transactions = context.Transactions.Include(c => c.Users).AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return transactions;
        }
    }
}

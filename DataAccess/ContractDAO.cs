using AutoMapper;
using BusinessObject;
using BusinessObject.DTO;
using BusinessObject.Mapping;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class ContractDAO
    {
        private static ContractDAO instance = null;
        private static readonly object instanceLock = new object();
        public ContractDAO() { }
        public static ContractDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ContractDAO();
                    }
                    return instance;
                }
            }
        }

        public int Create()
        {
            try
            {
                using var context = new CarRentingDBContext();
                var newContract = new Contract 
                {
                    CarInformation = "",
                    Deposit = 0,
                    Status = 1,
                    Note = "",
                };

                context.Contracts.Add(newContract);
                context.SaveChanges();
                return newContract.ContractID;

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create customer: {ex.Message}");
            }
        }
        public void Update(int contractId, ContractDTO dto)
        {
            try
            {
                using var context = new CarRentingDBContext();
                var existingContract = context.Contracts.Find(contractId);

                if (existingContract != null)
                {

                    if (dto.CarInformation != null)
                    {
                        existingContract.CarInformation = dto.CarInformation;
                    }

                    if (dto.Deposit != 0)
                    {
                        existingContract.Deposit = dto.Deposit;
                    }

                    if (dto.Note != null)
                    {
                        existingContract.Note = dto.Note;
                    }

                    if (dto.Status != 0)
                    {
                        existingContract.Status = dto.Status;
                    }

                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Contract not found for update.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update contract: {ex.Message}");
            }
        }

        public void UpdateStatus(int contractId, BookingUpdateDTO dto)
        {
            try
            {
                using var context = new CarRentingDBContext();
                var existingContract = context.Contracts.Find(contractId);

                if (existingContract != null)
                {
                    existingContract.Status = dto.Status;

                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("User not found for update.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update contract: {ex.Message}");
            }
        }
        public List<Contract> Contracts()
        {
            List<Contract> contracts;
            try
            {
                using var context = new CarRentingDBContext();
                contracts = context.Contracts.AsQueryable().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return contracts;
        }
    }
}

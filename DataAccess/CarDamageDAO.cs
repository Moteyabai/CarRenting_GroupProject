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
    public class CarDamageDAO
    {
        private static CarDamageDAO instance = null;
        private static readonly object instanceLock = new object();
        public CarDamageDAO() { }
        public static CarDamageDAO Instance
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new CarDamageDAO();
                    }
                    return instance;
                }
            }
        }

        public void Create(int detailsId)
        {
            try
            {
                using var context = new CarRentingDBContext();

                var check = context.BookingDetails.Find(detailsId);
                if (check == null)
                {
                    throw new InvalidOperationException("Booking detail not found for create.");
                }
                else
                {
                    var carDamage = new CarDamage
                    {
                        BookingDetailID = detailsId,
                        Damage = " ",
                        Fined = 0
                    };

                    context.CarDamages.Add(carDamage);
                    context.SaveChanges();
                }

            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to create car damage: {ex.Message}");
            }
        }
        public void Update(int detailsId, CarDamageDTO dto)
        {
            try
            {
                using var context = new CarRentingDBContext();
                var existingCarDamage = context.CarDamages.Find(detailsId);

                if (existingCarDamage != null)
                {
                    var config = new MapperConfiguration(cfg => cfg.AddProfile<CarDamageMapping>());
                    var mapper = new AutoMapper.Mapper(config);

                    mapper.Map(dto, existingCarDamage);

                    context.SaveChanges();
                }
                else
                {
                    throw new InvalidOperationException("Car damage not found for update.");
                }
            }
            catch (Exception ex)
            {
                throw new Exception($"Failed to update car damage: {ex.Message}");
            }
        }
        public List<CarDamage> CarDamages()
        {
            List<CarDamage> carDamages;
            try
            {
                using var context = new CarRentingDBContext();
                carDamages = context.CarDamages
                                .Include(cd => cd.BookingDetail)
                                .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return carDamages;
        }
    }
}

using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CarBrandDAO
    {
        private static CarBrandDAO instance = null;
        private static readonly object instanceLock = new object();
        public CarBrandDAO() { }
        public static CarBrandDAO Instance
        {
            get
            {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new CarBrandDAO();
                    }
                    return instance;
                }
            }
        }

        public List<CarBrand> GetCarBrand()
        {
            var list = new List<CarBrand>();
            try {
                using (var context = new CarRentingDBContext()) {
                    list = context.CarBrands.ToList();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public List<CarBrand> SearchCarBrandByName(string name)
        {
            var list = new List<CarBrand>();
            try {
                using (var context = new CarRentingDBContext()) {
                    list = context.CarBrands.Where(u => u.Name.Contains(name)).ToList();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public CarBrand GetCarBrandByID(int cID)
        {
            CarBrand car = new CarBrand();
            try {
                using (var context = new CarRentingDBContext()) {
                    car = context.CarBrands.SingleOrDefault(x => x.CarBrandID == cID);

                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return car;
        }

        public void AddCarBrand(CarBrand carBrand)
        {
            try {
                using (var context = new CarRentingDBContext()) {
                    context.CarBrands.Add(carBrand);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteCarBrand(int id)
        {
            var carBrand = GetCarBrandByID(id);
            if (carBrand != null) {
                using (var context = new CarRentingDBContext()) {
                    var carB = context.CarBrands.SingleOrDefault(x => x.CarBrandID == carBrand.CarBrandID);
                    context.CarBrands.Remove(carB);
                    context.SaveChanges();
                }
            }
        }

        public void UpdateCarBrand(CarBrand carBrand)
        {
            try {
                using (var context = new CarRentingDBContext()) {
                    context.Entry<CarBrand>(carBrand).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }
    }
}

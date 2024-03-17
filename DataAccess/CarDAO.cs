using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CarDAO
    {
        private static CarDAO instance = null;
        private static readonly object instanceLock = new object();
        public CarDAO() { }
        public static CarDAO Instance
        {
            get
            {
                lock (instanceLock) {
                    if (instance == null) {
                        instance = new CarDAO();
                    }
                    return instance;
                }
            }
        }

        public List<Car> GetCar()
        {
            var list = new List<Car>();
            try {
                using (var context = new CarRentingDBContext()) {
                    list = context.Cars.ToList();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public List<Car> SearchCarByName(string name)
        {
            var list = new List<Car>();
            try {
                using (var context = new CarRentingDBContext()) {
                    list = context.Cars.Where(u => u.CarName.Contains(name)).ToList();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public Car GetCarByID(int cID)
        {
            Car car = new Car();
            try {
                using (var context = new CarRentingDBContext()) {
                    car = context.Cars.SingleOrDefault(x => x.CarID == cID);

                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return car;
        }

        public void AddCar(Car car)
        {
            try {
                using (var context = new CarRentingDBContext()) {
                    context.Cars.Add(car);
                    context.SaveChanges();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteCar(int id)
        {         
                var car = GetCarByID(id);
                if (car != null) {
                    using (var context = new CarRentingDBContext()) {
                        var c = context.Cars.SingleOrDefault(x => x.CarID == car.CarID);
                        if (c != null) {
                            if (c.Status == 0) {                              
                                Console.WriteLine("Currently the Car's status is 0");
                            }
                            else {                              
                                c.Status = 0;
                                context.SaveChanges();
                            }
                        }
                    }
                }
        }

        public void UpdateCar(Car car)
        {
            try {
                using (var context = new CarRentingDBContext()) {
                    context.Entry<Car>(car).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
        }

        /*public List<Car> GetActiveCars()
        {
            var activeCars = new List<Car>();
            try {
                using (var context = new CarRentingDBContext()) {
                    activeCars = context.Cars.Where(car => car.Status == 1).ToList();
                }
            }
            catch (Exception ex) {
                throw new Exception(ex.Message);
            }
            return activeCars;
        }*/

    }
}

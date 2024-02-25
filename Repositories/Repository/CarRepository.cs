using BusinessObject;
using DataAccess;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class CarRepository : ICarRepository
    {
        public List<Car> GetListCar() => CarDAO.Instance.GetCar();
        public List<Car> SearchCarByName(string name) => CarDAO.Instance.SearchCarByName(name);
        public Car GetCarByID(int cID) => CarDAO.Instance.GetCarByID(cID);
        public void AddCar(Car car) => CarDAO.Instance.AddCar(car);
        public void DeleteCar(int id) => CarDAO.Instance.DeleteCar(id);
        public void UpdateCar(Car car) => CarDAO.Instance.UpdateCar(car);
    }
}

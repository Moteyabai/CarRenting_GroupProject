using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ICarRepository
    {
        public List<Car> GetListCar();
        public List<Car> SearchCarByName(string name);
        public Car GetCarByID(int cID);
        public void AddCar(Car car);
        public void DeleteCar(int id);
        public void UpdateCar(Car car);

    }
}

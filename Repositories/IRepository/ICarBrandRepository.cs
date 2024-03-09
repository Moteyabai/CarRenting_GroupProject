using BusinessObject;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ICarBrandRepository
    {
        public List<CarBrand> GetListCarBrand();
        public List<CarBrand> SearchCarBrandByName(string name);
        public CarBrand GetCarBrandByID(int cID);
        public void AddCarBrand(CarBrand carBrand);
        public void DeleteCarBrand(int id);
        public void UpdateCarBrand(CarBrand carBrand);
    }
}

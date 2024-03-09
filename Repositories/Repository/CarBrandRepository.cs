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
    public class CarBrandRepository : ICarBrandRepository
    {
        public List<CarBrand> GetListCarBrand() => CarBrandDAO.Instance.GetCarBrand();
        public List<CarBrand> SearchCarBrandByName(string name) => CarBrandDAO.Instance.SearchCarBrandByName(name);
        public CarBrand GetCarBrandByID(int cID) => CarBrandDAO.Instance.GetCarBrandByID(cID);
        public void AddCarBrand(CarBrand carBrand) => CarBrandDAO.Instance.AddCarBrand(carBrand);
        public void DeleteCarBrand(int id) => CarBrandDAO.Instance.DeleteCarBrand(id);
        public void UpdateCarBrand(CarBrand carBrand) => CarBrandDAO.Instance.UpdateCarBrand(carBrand);
    }
}

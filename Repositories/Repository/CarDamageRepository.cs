using BusinessObject;
using BusinessObject.DTO;
using DataAccess;
using Repositories.IRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.Repository
{
    public class CarDamageRepository : ICarDamageRepository
    {
        public List<CarDamage> CarDamages()
        => CarDamageDAO.Instance.CarDamages();

        public void Create(int detailsId)
        => CarDamageDAO.Instance.Create(detailsId);    

        public void Update(int detailsId, CarDamageDTO dto)
        => CarDamageDAO.Instance.Update(detailsId, dto);
    }
}

using BusinessObject;
using BusinessObject.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repositories.IRepository
{
    public interface ICarDamageRepository
    {
        void Create(int detailsId);
        void Update(int detailsId, CarDamageDTO dto);
        List<CarDamage> CarDamages();
    }
}

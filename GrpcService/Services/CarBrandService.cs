using Grpc.Core;
using Repositories.Repository;

namespace GrpcService.Services
{
    public class CarBrandService : CarBrandDetails.CarBrandDetailsBase
    {
        CarBrandRepository repository = new CarBrandRepository();

        public override Task<CarBrandListResponse> GetCarBrand(CarBrandRequest request, ServerCallContext context)
        {
            var cars = repository.GetCarBrandByID(request.ID);
            var carResponse = new List<CarBrandResponse>();

            carResponse.Add(new CarBrandResponse
            {
                CarBrandID = cars.CarBrandID,
                Name = cars.Name,
            });

            var carListResponse = new CarBrandListResponse
            {
                CarBrands = { carResponse }
            };

            return Task.FromResult(carListResponse);

        }
    }
}

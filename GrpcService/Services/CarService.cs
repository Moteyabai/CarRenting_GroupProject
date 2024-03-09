using BusinessObject;
using Grpc.Core;
using Repositories.Repository;

namespace GrpcService.Services
{
    public class CarService : CarDetails.CarDetailsBase
    {
        CarRepository repository = new CarRepository();

        public override Task<CarListResponse> GetCar(CarRequest request, ServerCallContext context)
        {
            var cars = repository.GetCarByID(request.ID);
            var carResponse = new List<CarResponse>();

            carResponse.Add(new CarResponse
            {
                CarName = cars.CarName,
                CarPlate = cars.CarPlate,
            });

            var carListResponse = new CarListResponse
            {
                Cars = { carResponse }
            };

            return Task.FromResult(carListResponse);

        }
    }
}

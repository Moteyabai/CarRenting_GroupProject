using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Models.CarModels;
using BusinessObject.Models.JwtTokenModels;
using BusinessObject.Models.UserModels;
using DataAccess;

namespace BusinessObject.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            Map_Register_User();
            Map_Update_User();
            Map_Contract();
            Map_CarDamage();
            Map_User_TokenModel();
            Map_BookingDetail();
            Map_Booking();
            Map_Car_CarViewModels();
            Map_Display_User();
            Map_Display_Car();
            Map_Update_Car();
            Map_Add_Car();
        }

        private void Map_Register_User()
        {
            CreateMap<UserRegisterDTO, User>();
        }
        private void Map_Display_User()
        {
            CreateMap<User, UserDisplayDTO>();
        }
        private void Map_Update_User()
        {
            CreateMap<UserUpdateDTO, User>();
        }

        private void Map_Contract()
        {
            CreateMap<ContractDTO, Contract>();
        }

        private void Map_CarDamage()
        {
            CreateMap<CarDamageDTO, CarDamage>();
        }

        private void Map_BookingDetail()
        {
            CreateMap<BookingDetailDTO, BookingDetail>();
        }

        private void Map_Booking()
        {
            CreateMap<BookingDTO, Booking>();
        }

        private void Map_User_TokenModel()
        {
            CreateMap<User, TokenModels>();
        }

        private void Map_Car_CarViewModels()
        {
            CreateMap<Car, CarViewModels>()
                .AfterMap<MapingAction_Car_CarViewModel>();
        }


        public class MapingAction_Car_CarViewModel : IMappingAction<Car, CarViewModels>
        {
            public void Process (Car source, CarViewModels destination, ResolutionContext context)
            {
                var db = new CarRentingDBContext();
                var brand = db.CarBrands.FirstOrDefault(x => x.CarBrandID == source.CarBrandID);

                destination.CarID = source.CarID;
                destination.CarName = source.CarName;
                destination.CarBrand = brand.Name;
                destination.CarPlate = source.CarPlate;
                destination.Deposit = source.Deposit;
                destination.PricePerDay = source.PricePerDay;
                destination.Status = source.Status;
                destination.ImageCar = source.ImageCar;
                destination.Seat = source.Seat;
                destination.Description = source.Description;
            }
        }
        private void Map_Display_Car()
        {
            CreateMap<Car, CarDisplayDTO>();
        }
        private void Map_Update_Car()
        {
            CreateMap<CarUpdateDTO, Car>();
        }
        private void Map_Add_Car()
        {
            CreateMap<CarAddDTO, Car>();
        }
        
    }
}

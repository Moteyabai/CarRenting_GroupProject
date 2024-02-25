using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Models.JwtTokenModels;

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
        }

        private void Map_Register_User()
        {
            CreateMap<UserRegisterDTO, User>();
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
    }
}

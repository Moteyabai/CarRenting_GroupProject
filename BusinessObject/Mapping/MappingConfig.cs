using AutoMapper;
using BusinessObject.DTO;

namespace BusinessObject.Mapping
{
    public class MappingConfig : Profile
    {
        public MappingConfig()
        {
            Map_Register_User();
            Map_Update_User();
        }

        private void Map_Register_User()
        {
            CreateMap<UserRegisterDTO, User>();
        }
        private void Map_Update_User()
        {
            CreateMap<UserUpdateDTO, User>();
        }

    }
}

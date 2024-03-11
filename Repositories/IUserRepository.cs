using BusinessObject;
using BusinessObject.Models.JwtTokenModels;
using BusinessObject.Models.UserModels;

namespace Repositories
{
    public interface IUserRepository
    {
        void AddNew(User user);
        void Update(User user);
        void Delete(int id);
        List<User> GetAllUsers();
        User GetUserByID(int id);
        List<UserViewModel> SearchUsersByName(string name);
        User CheckLogin(string email, string password);
        string Login(LoginModel model);
        void AdminUpdateUser(UserUpdateModel model);
    }
}

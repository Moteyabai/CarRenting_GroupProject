using BusinessObject;
using BusinessObject.Models.JwtTokenModels;
using DataAccess;

namespace Repositories
{
    public class UserRepository : IUserRepository
    {
        public void AddNew(User user) => UserDAO.Instance.SaveUser(user);

        public void Delete(int id) => UserDAO.Instance.DeleteUser(id);

        public List<User> GetAllUsers() => UserDAO.Instance.GetUsers();

        public User GetUserByID(int id) => UserDAO.Instance.GetUserByID(id);

        public List<User> SearchUsersByName(string name) => UserDAO.Instance.SearchUsersByName(name);

        public void Update(User user) => UserDAO.Instance.UpdateUser(user);

        public User CheckLogin(string email, string password) => UserDAO.Instance.CheckLogin(email, password);
        public string Login(LoginModel model) => AuthDAO.Instance.Login(model);
    }
}

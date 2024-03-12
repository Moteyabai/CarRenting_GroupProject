using AutoMapper;
using BusinessObject;
using BusinessObject.Models.UserModels;
using System.Text.Json;

namespace DataAccess
{
    public class UserDAO
    {
        //Singleton Pattern
        private readonly IMapper _mapper;
        private static UserDAO instance;
        private static readonly object instanceLock = new object();
        public static UserDAO Instance
        {
            get
            {
                if (instance == null)
                {
                    lock (instanceLock)
                    {
                        if (instance == null)
                            instance = new UserDAO();
                    }
                }
                return instance;
            }
        }

        public List<User> GetUsers()
        {
            var list = new List<User>();
            try
            {
                using (var context = new CarRentingDBContext())
                {
                    list = context.Users.ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }
        public List<User> SearchUsersByName(string name)
        {
            var list = new List<User>();
            try
            {
                using (var context = new CarRentingDBContext())
                {
                    list = context.Users.Where(u => u.UserName.Contains(name)).ToList();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return list;
        }

        public User GetUserByID(int uID)
        {
            User User = new User();
            try
            {
                using (var context = new CarRentingDBContext())
                {
                    User = context.Users.SingleOrDefault(x => x.UserID == uID);

                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
            return User;
        }

        public User CheckLogin(string email, string password)
        {
            var json = File.ReadAllText("D:\\Github Projects\\CarRenting_GroupProject\\BusinessObject\\appsettings.json");
            var admin = JsonSerializer.Deserialize<User>(json);
            List<User> users = GetUsers();
            if (admin.Email.Equals(email) && admin.Password.Equals(password))
            {
                admin.RoleID = 4;
                return admin;

            }
            if (users != null)
            {
                foreach (var user in users)
                {
                    if (user.Email.Equals(email) && user.Password.Equals(password))
                    {
                        return user;
                    }
                }
            }
            return null;
        }

        public void SaveUser(User User)
        {
            try
            {
                using (var context = new CarRentingDBContext())
                {
                    context.Users.Add(User);
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void DeleteUser(int id)
        {
            var user = GetUserByID(id);
            if (user != null)
            {
                using (var context = new CarRentingDBContext())
                {
                    var p1 = context.Users.SingleOrDefault(x => x.UserID == user.UserID);
                    context.Users.Remove(p1);
                    context.SaveChanges();
                }
            }

        }

        public void UpdateUser(User user)
        {
            try
            {
                using (var context = new CarRentingDBContext())
                {
                    context.Entry<User>(user).State =
                        Microsoft.EntityFrameworkCore.EntityState.Modified;
                    context.SaveChanges();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }

        public void AdminUpdateUser(UserUpdateModel model)
        {
            try
            {
                var db = new CarRentingDBContext();
                var user = _mapper.Map<User>(model);
                db.Users.Update(user);
                db.SaveChanges();
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}

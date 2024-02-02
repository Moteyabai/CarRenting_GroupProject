using Microsoft.AspNetCore.Mvc;
using BusinessObject;
using Repositories;
using AutoMapper;
using BusinessObject.DTO;

namespace CarRenting_API.Controllers
{
    [Route("api/[controller]")]
    /*[ApiController]*/
    public class UsersController : ControllerBase
    {
        private IUserRepository _userRepository = new UserRepository();
        private readonly IMapper _mapper;
        private string Message;

        public UsersController(IMapper mapper)
        {
            _mapper = mapper;
        }

        // GET: api/Users/UserList
        [HttpGet("UserList")]
        public ActionResult<IEnumerable<User>> GetUsers()
        {
            List<User> list = new List<User>();
            list = _userRepository.GetAllUsers();
            if (list.Count == 0)
            {
                Message = "List is empty!";

                return NotFound(Message);
            }
            return Ok(list);
        }

        [HttpGet("Search/{name}")]
        public ActionResult<IEnumerable<User>> SearchByName(string name)
        {
            List<User> list = new List<User>();
            list = _userRepository.SearchUsersByName(name);
            if (list.Count == 0)
            {
                Message = "No User Found";
                return NotFound(Message);
            }
            return Ok(list);
        }

        //Simple Login
        //GET: api/Users/Login
        [HttpGet("Login")]
        public ActionResult<User> Login(string email, string password)
        {
            var u = new User();
            u = _userRepository.CheckLogin(email, password);
            if (u == null)
            {
                Message = "Incorrect email or password!";
                return NotFound(Message);
            }
            return Ok(u);
        }

        // GET: api/Users/GetUser/5
        [HttpGet("GetUser/{id}")]
        public ActionResult<User> GetUserByID(int id)
        {
            var u = _userRepository.GetUserByID(id);
            if (u == null)
            {
                return NotFound("No user found!");
            }
            return Ok(u);
        }

        // PUT: api/Users/Update
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("Update")]
        public IActionResult UpdateUser(UserUpdateDTO userUpdateDTO)
        {
            var user = _mapper.Map<User>(userUpdateDTO);
            var u = _userRepository.GetUserByID(user.UserID);
            if (u == null)
            {
                Message = "No User Found!";
                return NotFound(Message);
            }
            user.RoleID = u.RoleID;
            user.Status = u.Status;
            _userRepository.Update(user);
            Message = "User Updated!";
            return Ok(Message);


        }

        // POST: api/Users/Register
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Register")]
        public ActionResult<User> Register(UserRegisterDTO userRegisterDTO)
        {
            var user = _mapper.Map<User>(userRegisterDTO);
            var list = _userRepository.GetAllUsers();
            foreach (User us in list)
            {
                if (us.Email.Equals(user.Email))
                {
                    Message = "Email existed!";
                    return NotFound(Message);
                }
            }
            user.RoleID = 1;
            user.Status = 1;
            _userRepository.AddNew(user);
            Message = "New User Added!";
            return Ok(Message);
        }

        // DELETE: api/Users/Delete/5
        [HttpDelete("Delete/{id}")]
        public IActionResult DeleteUser(int id)
        {
            var u = _userRepository.GetUserByID(id);
            if (u == null)
            {
                Message = "No User Found!";
                return NotFound(Message);
            }
            Message = "Deleted " + u.UserName;
            _userRepository.Delete(id);
            return Ok(Message);
        }
    }
}

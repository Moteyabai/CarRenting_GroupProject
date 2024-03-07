using Microsoft.AspNetCore.Mvc;
using BusinessObject;
using Repositories;
using AutoMapper;
using BusinessObject.DTO;
using BusinessObject.Models.JwtTokenModels;
using System.Collections.Generic;
using Microsoft.AspNetCore.Authorization;
using BusinessObject.Models.UserModels;

namespace CarRenting_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
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
        [Authorize]
        public ActionResult<IEnumerable<UserDisplayDTO>> GetUsers()
        {
            try
            {
                List<UserDisplayDTO> list = new List<UserDisplayDTO>();
                var userList = _userRepository.GetAllUsers();
                if (userList == null)
                {
                    return NotFound("No User List");
                }
                else
                {
                    list = _mapper.Map<List<UserDisplayDTO>>(userList);
                    return Ok(list);
                }
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [HttpGet("Search/{name}")]
        public ActionResult<IEnumerable<User>> SearchByName(string name)
        {           
            List<User> list = _userRepository.SearchUsersByName(name);
            var nList = _mapper.Map<List<UserDisplayDTO>>(list);

            if (nList.Count == 0)
            {
                Message = "No users found!";

                return NotFound(Message);
            }
            return Ok(nList);
        }

        //Simple Login
        //GET: api/Users/Login
        [HttpGet("Login")]
        public ActionResult<User> Login(string email, string password)
        {
            try
            {
                var model = new LoginModel()
                {
                    Email = email,
                    Password = password
                };
                var token = _userRepository.Login(model);
                return Ok(token);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        // GET: api/Users/GetUser/5
        [HttpGet("GetUser/{id}")]
        public ActionResult<User> GetUserByID(int id)
        {
            var u = _userRepository.GetUserByID(id);
            var nU = _mapper.Map<UserDisplayDTO>(u);

            if (nU == null)
            {
                Message = "No user found!";

                return NotFound(Message);
            }
            return Ok(nU);
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
        public ActionResult<UserRegisterDTO> Register(UserRegisterDTO userRegisterDTO)
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
        [HttpDelete("Delete")]
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


using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO
{
    public class UserUpdateDTO
    {
        [Required]
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Identification { get; set; }
        public string License { get; set; }
    }
}

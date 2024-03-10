
using System.ComponentModel.DataAnnotations;

namespace BusinessObject.DTO
{
    public class UserRegisterDTO
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        [Required]
        public string Email { get; set; }
        public string Phone { get; set; }
        [StringLength(12)]
        public string Identification { get; set; }
        [StringLength(10)]
        public string License { get; set; }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BusinessObject.Models.UserModels
{
    public class UserUpdateModel
    {
        public int UserID { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Identification { get; set; }
        public string License { get; set; }
        public int RoleID { get; set; }
        public int Status { get; set; }
    }
}

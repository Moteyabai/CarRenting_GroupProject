using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Transactions;

namespace BusinessObject
{
    public class User
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int UserID { get; set; }
        [Required]
        [StringLength(50)]
        public string UserName { get; set; }
        [Required]
        [StringLength(50)]
        public string Password { get; set; }
        [Required]
        [StringLength(50)]
        public string Email { get; set; }
        [Required]
        [StringLength(10)]
        public string Phone { get; set; }
        [Required]
        [StringLength(12)]
        public string Identification { get; set; }
        [Required]
        [StringLength(10)]
        public string License { get; set; }
        public int RoleID { get; set; }
        public int Status { get; set; }
        public virtual Role Roles { get; set; }
        public virtual ICollection<Transaction> Transactions { get; set; }
    }
}

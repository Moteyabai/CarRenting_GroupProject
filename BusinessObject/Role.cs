using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class Role
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RoleID { get; set; }
        [Required]
        [StringLength(10)]
        public string Name { get; set; }
        public virtual ICollection<User> Users { get; set; }
    }
}

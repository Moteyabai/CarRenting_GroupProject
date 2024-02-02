using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusinessObject
{
    public class CarBrand
    {
        [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int CarBrandID { get; set; }
        [Required]
        public string Name { get; set; }
        public virtual ICollection<Car> Cars { get; set; }
    }
}

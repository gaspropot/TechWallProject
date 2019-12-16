using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace TechWall.Entities
{
    public class Brand : BaseEntity

    {
        [Required]
        [MinLength(2,ErrorMessage ="Brand Name has a minimum lenght of 2 characters")]
        [MaxLength(15,ErrorMessage ="Brand Name has a maximum length of 15 characters")]
        [DisplayName("Brand Name")]
        public string Name { get; set; }
     
        public virtual ICollection<Product> Products { get; set; }

        public Brand()
        {
           
        }
    }
}

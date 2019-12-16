using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;

namespace TechWall.Entities { 

    public class Category : BaseEntity
    {

        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public bool isFeatured { get; set; }
        [ForeignKey("ParentCategory")]
        public int? ParentCategoryID { get; set; }
        public virtual Category ParentCategory { get; set; }
        public virtual ICollection<Category> SubCategories { get; set; }
      
        public virtual ICollection<Product> Products { get; set; }

        //public Category()
        //{
        //    this = new HashSet<Product>();

        //}

    }


}

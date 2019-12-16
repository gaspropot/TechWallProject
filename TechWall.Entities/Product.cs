using System.Collections.Generic;

namespace TechWall.Entities
{

    public class Product : BaseEntity
    {
      
        public int BrandID { get; set; }
       public int CategoryID { get; set; }

        public string Name { get; set; }
        public string Summary { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool isFeatured { get; set; }
        public int ThumbnailPictureID { get; set; }



        public virtual Brand Brand { get; set; }
        public virtual Category Category { get; set; }
        public virtual List<Picture> Pictures { get; set; }


        public Product()
        {
            
            this.Pictures = new List<Picture>();

        }
    }
}

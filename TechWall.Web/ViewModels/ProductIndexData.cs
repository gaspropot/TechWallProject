
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechWall.Entities;

namespace TechWall.ViewModels
{
    public class ProductIndexData
    {
        public IEnumerable<Product> Products { get; set; }
        public IEnumerable<Picture> Pictures { get; set; }
        public  IEnumerable<Category> Categories { get; set; }
        public IEnumerable<Brand> Brands { get; set; }
    }
}
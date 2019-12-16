using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using TechWall.ViewModels;
using TechWall.Data;

namespace TechWall.Controllers
{
    public class ProductValuesController : ApiController
    {
        TechWallDbContext db = new TechWallDbContext();

        public IQueryable<ProductDTO> GetBooks()
        {
            var products = from p in db.Products
                        select new ProductDTO()
                        {
                            Id = p.ID,
                            Name = p.Name,
                            Brand = p.Brand.Name,
                            Category=p.Category.Name,
                            Description=p.Description,
                            Price = p.Price.ToString()

                        };

            return products;
        }
    }
}

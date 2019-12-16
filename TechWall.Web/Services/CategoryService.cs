using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TechWall.Data;
using TechWall.Entities;

namespace TechWall.Services
{
    public class CategoryService
    {
        public IQueryable<Category> GetCategories()
        {

            TechWallDbContext _Context = new TechWallDbContext();
            IQueryable<Category> ListOfCategories = _Context.Categories;
            return ListOfCategories;
        }

         public IQueryable<Category> GetSubCategories(int catId)
        {

            TechWallDbContext _Context = new TechWallDbContext();
            IQueryable<Category> ListOfCategories = _Context.Categories.Where(pc=>pc.ParentCategoryID==catId);
            return ListOfCategories;
        }

        
    }
}
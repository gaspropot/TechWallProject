using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechWall.Data;

namespace TechWall.Services
{
    public class CategoryService
    {
        private readonly TechWallDbContext db;

        public CategoryService(TechWallDbContext context)
        {
            this.db = context;
        }

        
    }
}

using Microsoft.AspNet.Identity.EntityFramework;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechWall.Entities;

namespace TechWall.Data
{
    public class TechWallDbContext : IdentityDbContext<ApplicationUser>
    {

        public TechWallDbContext() : base("DefaultConnection")
        {
            Database.SetInitializer<TechWallDbContext>(new DropCreateDatabaseIfModelChanges<TechWallDbContext> ());
            //Database.SetInitializer<eShopTRTContext>(new eShopTRTDbInitializer());
        }

        public static TechWallDbContext Create()
        {
            return new TechWallDbContext();
        }



        public DbSet<Category> Categories { get; set; }
        public DbSet<Brand> Brands { get; set; }
        public DbSet<Comment> Comments { get; set; }
        public DbSet<Order> Orders { get; set; }
        public DbSet<OrderHistory> OrderHistories { get; set; }
        public DbSet<OrderItem> OrderItems { get; set; }
        public DbSet<Picture> Pictures { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Message> Messages { get; set; }
        public DbSet<UserConnection> UserConnections { get; set; }

      





        //protected override void OnModelCreating(DbModelBuilder modelBuilder)
        //{

        //    base.OnModelCreating(modelBuilder);
        //}


    }
}

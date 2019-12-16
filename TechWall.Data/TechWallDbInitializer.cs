//Use this with disabled migrations database gets created when context is accessed eg when you run the application

//using Microsoft.AspNet.Identity;
//using Microsoft.AspNet.Identity.EntityFramework;
//using System;
//using System.Collections.Generic;
//using System.Data.Entity;
//using System.Data.Entity.Migrations;
//using System.Linq;
//using System.Text;
//using System.Threading.Tasks;
//using TechWall.Entities;

//namespace TechWall.Data
//{
//    class TechWallDbInitializer : IDatabaseInitializer<TechWallDbContext>
//    {
//        public void InitializeDatabase(TechWallDbContext context)
//        {
//            context.Database.Delete();
//            context.Database.Create();
//            Seed(context);
//        }

//        protected virtual void Seed(TechWallDbContext context)
//        {
//            SeedUsersAndRoles(context);
//            SeedCategories(context);
//            context.SaveChanges();
//        }

//        public static void SeedUsersAndRoles(TechWallDbContext context)
//        {

//            string roleAdmin = "admin";
//            string roleEshopManager = "eshopmanager";
//            string roleCustomer = "customer";

//            if (!context.Roles.Any(u => u.Name == "admin"))
//            {
//                var roleStore = new RoleStore<IdentityRole>(context);
//                var roleManager = new RoleManager<IdentityRole>(roleStore);
//                var identityRole = new IdentityRole { Name = roleAdmin };
//                roleManager.Create(identityRole);
//            }

//            if (!context.Roles.Any(u => u.Name == "eshopmanager"))
//            {
//                var roleStore = new RoleStore<IdentityRole>(context);
//                var roleManager = new RoleManager<IdentityRole>(roleStore);
//                var identityRole = new IdentityRole { Name = roleEshopManager };
//                roleManager.Create(identityRole);
//            }

//            if (!context.Roles.Any(u => u.Name == "customer"))
//            {
//                var roleStore = new RoleStore<IdentityRole>(context);
//                var roleManager = new RoleManager<IdentityRole>(roleStore);
//                var identityRole = new IdentityRole { Name = roleCustomer };
//                roleManager.Create(identityRole);
//            }


//            if (!context.Users.Any(u => u.UserName == "admin@admin.com"))
//            {
//                var store = new UserStore<ApplicationUser>(context);

//                var manager = new UserManager<ApplicationUser>(store);

//                var user = new ApplicationUser { UserName = "admin@admin.com", Email = "admin@admin.com" };

//                manager.Create(user, "!Admin#1234");

//                manager.AddToRole(user.Id, roleAdmin);
//            }


//            if (!context.Users.Any(u => u.UserName == "manager@manager.com"))
//            {
//                var store = new UserStore<ApplicationUser>(context);

//                var manager = new UserManager<ApplicationUser>(store);

//                var user = new ApplicationUser { UserName = "manager@manager.com", Email = "manager@manager.com" };

//                manager.Create(user, "!Admin#1234");

//                manager.AddToRole(user.Id, roleEshopManager);
//            }



//            if (!context.Users.Any(u => u.UserName == "customer@customer.com"))
//            {
//                var store = new UserStore<ApplicationUser>(context);

//                var manager = new UserManager<ApplicationUser>(store);

//                var user = new ApplicationUser { UserName = "customer@customer.com", Email = "customer@customer.com" }; //DEN EXEI DHMIOURGITHEI

//                manager.Create(user, "!Admin#1234"); //to 2o einai to password, opote thelei Kefalaio-mikro ktl

//                manager.AddToRole(user.Id, roleCustomer);

//            }


//        }

//        public static void SeedCategories(TechWallDbContext context)
//        {

//            //,Categories = new List<Category>()
//            #region SeedBrandsTable
//            var brands = new List<Brand>
//            {
//                new Brand{Name="Apple"},
//                new Brand{Name="Sony"},
//                new Brand{Name="Huawei"},
//                new Brand{Name="Nokia"},
//                new Brand{Name="Dell"},
//                new Brand{Name="Nikkon"},
//                new Brand{Name="Canon"},
//                new Brand{Name="Asus"},
//                new Brand{Name="HP"},
//                new Brand{Name="Xiaomi"},
//                new Brand{Name="Alcatel"}

//            };
//            brands.ForEach(c => context.Brands.AddOrUpdate(p => p.Name, c));
//            context.SaveChanges();
//            #endregion

//            #region SeedCategoriesTable
//            var paraentCategories = new List<Category>();

//            //SEED CATEGORIES
//            //Parent Categories
//            var Computers = new Category { Name = "Computers" };
//            var Photography = new Category { Name = "Photography" };
//            var MobileDevices = new Category { Name = "Mobile Devices" };

//            paraentCategories.Add(Photography);
//            paraentCategories.Add(Computers);
//            paraentCategories.Add(MobileDevices);

//            paraentCategories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
//            context.SaveChanges();

//            var Laptops = new Category { Name = "Laptops", ParentCategoryID = context.Categories.Single(pc => pc.Name == "Computers").ID };
//            var Desktops = new Category { Name = "Desktops", ParentCategoryID = context.Categories.Single(pc => pc.Name == "Computers").ID };

//            var VideoCameras = new Category { Name = "Video Cameras", ParentCategoryID = context.Categories.Single(pc => pc.Name == "Photography").ID };
//            var PhotoCameras = new Category { Name = "Photo Cameras", ParentCategoryID = context.Categories.Single(pc => pc.Name == "Photography").ID };

//            var MobilePhones = new Category { Name = "Mobile Phones", ParentCategoryID = context.Categories.Single(pc => pc.Name == "Mobile Devices").ID };
//            var Tablets = new Category { Name = "Tablets", ParentCategoryID = context.Categories.Single(pc => pc.Name == "Mobile Devices").ID };

//            var firstChildCategories = new List<Category>();

//            firstChildCategories.Add(Laptops);
//            firstChildCategories.Add(Desktops);
//            firstChildCategories.Add(VideoCameras);
//            firstChildCategories.Add(PhotoCameras);
//            firstChildCategories.Add(MobilePhones);
//            firstChildCategories.Add(Tablets);

//            firstChildCategories.ForEach(c => context.Categories.AddOrUpdate(p => p.Name, c));
//            context.SaveChanges();
//            #endregion

//            #region AsscociateBrandsWithCategories

//            //AddOrUpdateBrandCategory(context, "Sony", "Computers");
//            //AddOrUpdateBrandCategory(context, "Sony", "Laptops");
//            //AddOrUpdateBrandCategory(context, "Sony", "Desktops");
//            //AddOrUpdateBrandCategory(context, "Sony", "Mobile Devices");
//            //AddOrUpdateBrandCategory(context, "Sony", "Mobile Phones");
//            //AddOrUpdateBrandCategory(context, "Sony", "Mobile Tablets");
//            //AddOrUpdateBrandCategory(context, "Sony", "Photography");
//            //AddOrUpdateBrandCategory(context, "Sony", "Photo Cameras");
//            //AddOrUpdateBrandCategory(context, "Sony", "Video Cameras");

//            //AddOrUpdateBrandCategory(context, "Xiaomi", "Computers");
//            //AddOrUpdateBrandCategory(context, "Xiaomi", "Laptops");
//            //AddOrUpdateBrandCategory(context, "Xiaomi", "Mobile Devices");

//            //AddOrUpdateBrandCategory(context, "Apple", "Computers");
//            //AddOrUpdateBrandCategory(context, "Apple", "Laptops");
//            //AddOrUpdateBrandCategory(context, "Apple", "Desktops");
//            //AddOrUpdateBrandCategory(context, "Apple", "Mobile Devices");
//            //AddOrUpdateBrandCategory(context, "Apple", "Mobile Phones");
//            //AddOrUpdateBrandCategory(context, "Apple", "Mobile Tablets");
//            //AddOrUpdateBrandCategory(context, "Apple", "Photography");


//            //AddOrUpdateBrandCategory(context, "Canon", "Photography");
//            //AddOrUpdateBrandCategory(context, "Canon", "Photo Cameras");
//            //AddOrUpdateBrandCategory(context, "Canon", "Video Cameras");

//            //AddOrUpdateBrandCategory(context, "Asus", "Computers");
//            //AddOrUpdateBrandCategory(context, "Asus", "Laptops");
//            //AddOrUpdateBrandCategory(context, "Asus", "Desktops");
//            //AddOrUpdateBrandCategory(context, "Asus", "Mobile Devices");
//            //AddOrUpdateBrandCategory(context, "Asus", "Mobile Phones");
//            //AddOrUpdateBrandCategory(context, "Asus", "Mobile Tablets");

//            context.SaveChanges();


//            #endregion




//            #region SeedProducts
//            var products = new List<Product>
//            {
//                new Product { Name = "IPhone 10", Price = 800, Summary = "It is an iphone short", Description = "It is an iphone long",

//                    BrandID = context.Brands.Single(b=>b.Name=="Apple").ID

//                },

//               new Product { Name = "IPhone", Price = 800, Summary = "It is an iphone short", Description = "It is an iphone long",

//                    BrandID = context.Brands.Single(b=>b.Name=="Apple").ID

//                }

//               };

//            products.ForEach(p => context.Products.AddOrUpdate(pp => pp.Name, p));
//            context.SaveChanges();



//            #endregion

//            #region Pictures
//            var pictures = new List<Picture>
//            {
//                new Picture(){URL=@"~\Content\img\grass_jumbotron.jpg",ProductID= context.Products.Single(p=>p.Name=="Iphone").ID},
//                new Picture(){URL=@"~\Content\img\logo.jpg",ProductID= context.Products.Single(p=>p.Name=="Iphone 10").ID},
//            };

//            pictures.ForEach(p => context.Pictures.AddOrUpdate(pp => pp.URL, p));
//            context.SaveChanges();

//            #endregion




//        }





//        //private static void AddOrUpdateBrandCategory(TechWallDbContext context, string brandName, string categoryName)
//        //{
//        //    Brand brand = null;
//        //    Category category = null;
//        //    try
//        //    {
//        //        brand = context.Brands.SingleOrDefault(b => b.Name == brandName);
//        //        category = brand.Categories.SingleOrDefault(c => c.Name == categoryName);

//        //    }
//        //    catch (Exception)
//        //    {


//        //    }
//        //    try
//        //    {
//        //        if (category == null)
//        //        {
//        //            brand.Categories.Add(context.Categories.Single(bc => bc.Name == categoryName));
//        //        }
//        //    }
//        //    catch (Exception)
//        //    {


//        //    }


//        }



//    }
//}

using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using TechWall.Data;
using TechWall.Entities;


namespace TechWall.Areas.Admin.Controllers
{
    public class CategoryController : BaseController
    {
        private TechWallDbContext db;

        public CategoryController()
        {
            this.db = new TechWallDbContext();
            
        }

        public ActionResult Index()
        {
            
            return View(db.Categories.ToList());
        }

        // GET: Admin/Category
        //public ActionResult Index(string sortOrder, string searchString, string currentFilter)
        //{
        //    ViewBag.CurrentSort = sortOrder;

        //    ViewBag.ParentNameSortParm = sortOrder == "pname_asc" ? "pname_desc" : "pname_asc";
        //    ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
        //    ViewBag.ProductNoParm = sortOrder == "pno_asc" ? "pno_desc" : "pno_asc";
           
           
        //    ViewBag.CurrentFilter = searchString;

        //    var categories = db.Categories.Select(c => c).Include(c => c.Products);

        //    if (!String.IsNullOrEmpty(searchString))
        //    {
        //        categories = categories.Where(c => c.Name.Contains(searchString)
        //                               || c.ParentCategory.Name.Contains(searchString));
        //    }

        //    switch (sortOrder)
        //    {
        //        case "name_asc":
        //            categories = categories.OrderBy(c => c.Name);
        //            break;
        //        case "name_desc":
        //            categories = categories.OrderByDescending(c => c.Name);
        //            break;
        //        case "pname_asc":
        //            categories = categories.OrderBy(c => c.ParentCategory.Name).ThenBy(c => c.Name);
        //            break;
        //        case "pname_desc":
        //            categories = categories.OrderByDescending(c => c.ParentCategory.Name).ThenBy(c => c.Name);
        //            break;
        //        case "pno_asc":
        //            categories = categories.OrderBy(c => c.Products.Count()).ThenBy(c => c.Name);
        //            break;
        //        case "pno_desc":
        //            categories = categories.OrderByDescending(c => c.Products.Count()).ThenBy(c => c.Name);
        //            break;
        //        default:
        //            categories = categories.OrderBy(c => c.Name);
        //            break;

        //    }



          
        //    return View(categories.ToList());
        //}

        // GET: Admin/Category/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(category);
        //}

        // GET: Admin/Category/Create
        public ActionResult Create()
        {
            ViewBag.ParentCategoryID = categoriesToSelect();
            return View();
        }

        // POST: Admin/Category/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,Summary,Description,isFeatured,ParentCategoryID,ModifiedOn")] Category category)
        {
            if (ModelState.IsValid)
            {
                category.ModifiedOn = DateTime.Now;
                db.Categories.Add(category);
                db.SaveChanges();
                TempData["Message"] = "You successfully added a category";
                TempData["Class"] = "bg-primary";
                return RedirectToAction("Index");
            }

            ViewBag.ParentCategoryID = new SelectList(db.Categories, "ID", "Name", category.ParentCategoryID);
            return View(category);
        }

        // GET: Admin/Category/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Category category = db.Categories.Find(id);
            if (category == null)
            {
                return HttpNotFound();
            }
            TempData["OriginalName"] = category.Name.ToLower();
            ViewBag.ParentCategoryID = categoriesToSelect();
            return View(category);
        }

        // POST: Admin/Category/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,Summary,Description,isFeatured,ParentCategoryID,ModifiedOn")] Category category)
        {
            if (!TempData["OriginalName"].Equals(category.Name.ToLower()) && NameExists(category.Name))
            {
                TempData["Message"] = "A brand with the same name already exists";
                TempData["Class"] = "bg-danger";
                TempData["OriginalName"] = TempData["OriginalName"];
                ViewBag.ParentCategoryID = categoriesToSelect();
                return View(category);
            }
            if (ModelState.IsValid)
            {
                category.ModifiedOn = DateTime.Now;
                db.Entry(category).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "You successfully edited a category";
                TempData["Class"] = "bg-primary";
                return RedirectToAction("Index");
            }
            ViewBag.ParentCategoryID = categoriesToSelect();
            return View(category);
        }

        // GET: Admin/Category/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Category category = db.Categories.Find(id);
        //    if (category == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(category);
        //}

        // POST: Admin/Category/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id)
        {
            Category category = db.Categories.Find(id);
            if (category.SubCategories.Count()>0 || category.Products.Count()>0)
            {
                TempData["Message"]= "You cannot delete a category that  has subcategories or products";
                TempData["Class"] = "bg-danger";
         
                return RedirectToAction("Index");
            }
            db.Categories.Remove(category);
            db.SaveChanges();

            TempData["Message"] = "Category has succesfully been deleted";
            TempData["Class"] = "bg-primary";

            return RedirectToAction("Index");
        }




        public static IQueryable<SelectListItem> categoriesToSelect()
        {
            TechWallDbContext db = new TechWallDbContext();
                
            var query = db.Categories
                              .Select(c => new SelectListItem
                                 {                  
                                Value = c.ID.ToString(),
                                Text = c.Name
                                });

                return query;
            

        }

        private bool NameExists(string name)
        {
            var brand = db.Categories.FirstOrDefault(p => p.Name.ToLower().Equals(name.ToLower()));

            if (brand != null)
                return true;
            return false;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}

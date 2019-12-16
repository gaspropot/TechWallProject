
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
    public class BrandController : BaseController
    {
        private TechWallDbContext db;

        public BrandController()
        {
            this.db = new TechWallDbContext();
        }

        // GET: Admin/Brand
        public ActionResult Index(string sortOrder, string searchString, string currentFilter)
        {
            //ViewBag.CurrentSort = sortOrder;
            //ViewBag.NameSortParm = sortOrder == "name_asc" ? "name_desc" : "name_asc";
            //ViewBag.ProductNoParm = sortOrder == "pno_asc" ? "pno_desc" : "pno_asc";

           
            //ViewBag.CurrentFilter = searchString;

            //var brands = db.Brands.Select(c => c);

            //if (!String.IsNullOrEmpty(searchString))
            //{
            //    brands = brands.Where(c => c.Name.Contains(searchString));
            //}

            //switch (sortOrder)
            //{
            //    case "name_asc":
            //        brands = brands.OrderBy(c => c.Name);
            //        break;
            //    case "name_desc":
            //        brands = brands.OrderByDescending(c => c.Name);
            //        break;
             
            //    default:
            //        brands = brands.OrderBy(c => c.Name);
            //        break;

            //}



            return View(db.Brands.ToList());
        }

        //// GET: Admin/Brand/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Brand brand = db.Brands.Find(id);
        //    if (brand == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(brand);
        //}

        // GET: Admin/Brand/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Brand/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "ID,Name,ModifiedOn")] Brand brand)
        {
            if (NameExists(brand.Name))
            {
                TempData["Message"] = "A brand with the same name already exists";
                TempData["Class"] = "bg-primary";
                return RedirectToAction("Create");
            }
            if (ModelState.IsValid)
            {
                brand.ModifiedOn = DateTime.Now;
                db.Brands.Add(brand);
                db.SaveChanges();
                TempData["Message"] = "You successfully created a brand";
                TempData["Class"] = "bg-primary";
                return RedirectToAction("Index");
            }

            return View(brand);
        }

        // GET: Admin/Brand/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Brand brand = db.Brands.Find(id);
            if (brand == null)
            {
                return HttpNotFound();
            }
            TempData["OriginalName"] = brand.Name.ToLower();
            return View(brand);
        }

        // POST: Admin/Brand/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "ID,Name,ModifiedOn")] Brand brand)
        {
            if (!TempData["OriginalName"].Equals(brand.Name.ToLower()) && NameExists(brand.Name))
            {
                TempData["Message"] = "A brand with the same name already exists";
                TempData["Class"] = "bg-danger";
                TempData["OriginalName"] = TempData["OriginalName"];
                return View(brand);
            }

            if (ModelState.IsValid)
            {
                brand.ModifiedOn = DateTime.Now;
                db.Entry(brand).State = EntityState.Modified;
                db.SaveChanges();
                TempData["Message"] = "You successfully edited a brand";
                TempData["Class"] = "bg-primary";
                return RedirectToAction("Index");
            }
            return View(brand);
        }

        // GET: Admin/Brand/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Brand brand = db.Brands.Find(id);
        //    if (brand == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    if (brand.Products.Count > 0)
        //    {
        //        return RedirectToAction("Index");
        //    }

        //    return View(brand);
        //}

        // POST: Admin/Brand/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Brand brand = db.Brands.Find(id);
            if (brand.Products.Count() > 0)
            {
                TempData["Message"] = "You cannot delete a brand that  has  products";
                TempData["Class"] = "bg-danger";

                return RedirectToAction("Index");
            }
            db.Brands.Remove(brand);
            db.SaveChanges();
            TempData["Message"] = "Brand has succesfully been deleted";
            TempData["Class"] = "bg-primary";
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool NameExists(string name)
        {
            var brand = db.Brands.FirstOrDefault(p => p.Name.ToLower().Equals(name.ToLower()));

            if (brand!=null)
                return true;
            return false;
        }
    }
}

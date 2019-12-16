using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using TechWall.Data;
using TechWall.Entities;

namespace TechWall.Areas.Admin.Controllers
{
    public class ProductController : BaseController
    {
        private TechWallDbContext db = new TechWallDbContext();

        // GET: Admin/Product
        public ActionResult Index()
        {
            var products = db.Products.Include(p => p.Brand).Include(p => p.Category);
            return View(products.ToList());
        }

        // GET: Admin/Product/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Product/Create
        public ActionResult Create()
        {
            ViewBag.BrandID = new SelectList(db.Brands, "ID", "Name");
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name");
            return View();
        }

        // POST: Admin/Product/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Create( Product product, HttpPostedFileBase file)
        {
          

          
            //List<Picture> pictures = new List<Picture>();
           
            //string fName;
            //if (Request.Files != null)
            //{
            //    foreach (string filename in Request.Files)
            //    {
            //        HttpPostedFileBase file = Request.Files[filename];

            //        fName = file.FileName;

            //        if (file != null && file.ContentLength > 0)
            //        {

            //            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Products", Server.MapPath(@"\")));

            //            string pathString = System.IO.Path.Combine(originalDirectory.ToString(), product.Name);

            //            var fileName1 = Path.GetFileName(file.FileName);

            //            bool isExists = System.IO.Directory.Exists(pathString);

            //            if (!isExists)
            //                System.IO.Directory.CreateDirectory(pathString);

            //            var path = string.Format("{0}\\{1}", pathString, file.FileName);
            //            file.SaveAs(path);
            //            pictures.Add(new Picture { URL="~//Images//Products//"+product.Name+"//"+file.FileName });;
            //        }
            //    }
            //}

            //try
            //{
            //    product.Pictures.AddRange(pictures);
            //}
            //catch (Exception)
            //{

                
            //}

            if (ModelState.IsValid)
            {
                Picture picture = SavePicture(file, product.Name);
                if(picture!=null)
               product.Pictures.Add(picture);
                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.BrandID = new SelectList(db.Brands, "ID", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);

            return View();
        }

      

        // GET: Admin/Product/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.BrandID = new SelectList(db.Brands, "ID", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View(product);
        }

        // POST: Admin/Product/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [ValidateInput(false)]
        public ActionResult Edit([Bind(Include = "ID,BrandID,CategoryID,Name,Summary,Description,Price,isFeatured,ThumbnailPictureID,ModifiedOn")] Product product)
        {
            if (ModelState.IsValid)
            {
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.BrandID = new SelectList(db.Brands, "ID", "Name", product.BrandID);
            ViewBag.CategoryID = new SelectList(db.Categories, "ID", "Name", product.CategoryID);
            return View(product);
        }

        //// GET: Admin/Product/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Product product = db.Products.Find(id);
        //    if (product == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(product);
        //}

        // POST: Admin/Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            DeleteDirectory(product.Name);
            return RedirectToAction("Index");
        }
        public ActionResult DeletePicture(int pictureid, int productid)
        {
            Product product = db.Products.Find(productid);
            Picture picture = db.Pictures.Find(pictureid);
            product.Pictures.Remove(picture);
            if (ModelState.IsValid)
            {
                db.Entry(picture).State = EntityState.Deleted;
                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Edit", new { id = product.ID });
               
            }
           
            return RedirectToAction("Edit", new { id = product.ID });
        }
        private void DeleteDirectory(string name)
        {
            var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Products", Server.MapPath(@"\")));

            string pathString = System.IO.Path.Combine(originalDirectory.ToString(), name);
           
            bool isExists = System.IO.Directory.Exists(pathString);

            if (isExists)
                System.IO.Directory.Delete(pathString,true);
              
        }
        private Picture SavePicture(HttpPostedFileBase file,string productName)
        {
            if (file != null && file.ContentLength > 0)
            {

                var originalDirectory = new DirectoryInfo(string.Format("{0}Images\\Products", Server.MapPath(@"\")));

                string pathString = System.IO.Path.Combine(originalDirectory.ToString(), productName);

                var extension = Path.GetExtension(file.FileName);

                var filename = Guid.NewGuid().ToString();

                bool isExists = System.IO.Directory.Exists(pathString);

                if (!isExists)
                    System.IO.Directory.CreateDirectory(pathString);

                var path = string.Format("{0}\\{1}", pathString, filename+extension);
                
                file.SaveAs(path);

               

                return new Picture { URL = "~/Images/Products/" + productName + "/" + filename+extension,ModifiedOn=DateTime.Now }; 
            }
            return null;
        }
        [HttpPost]
        public ActionResult SaveMultiple(int productId)
        {
            var product = db.Products.Find(productId);
            var pictures = new List<Picture>();

            if (product != null && Request.Files != null)
            {
                foreach (string filename in Request.Files)
                {
                    HttpPostedFileBase file = Request.Files[filename];
                    pictures.Add(SavePicture(file, product.Name));
                   
                    if (ModelState.IsValid)
                    {
                        
                        if (pictures != null)
                            product.Pictures.AddRange(pictures);

                        db.Entry(product).State = EntityState.Modified;
                        db.SaveChanges();
                        return Json(new { Message = "files saved" });
                    }
                }
            }
            return Json(new {Message="files saved" });
              
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

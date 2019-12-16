using PayPal.Api;
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
    public class OrderController : Controller
    {

        public ActionResult Index()
        {
            var apiContext = GetApiContext();

            var sales = Payment.List(apiContext);

            return View(sales);
        }

        public ActionResult Refund(string saleId)
        {
            var apiContext = GetApiContext();

            var sale = new Sale()
            {
                id = saleId
            };

            // A refund with no details refunds the entire amount.
            sale.Refund(apiContext, new Refund());

            return RedirectToAction("Index");
        }

        private APIContext GetApiContext()
        {
            // Authenticate with PayPal
            var config = ConfigManager.Instance.GetProperties();
            var accessToken = new OAuthTokenCredential(config).GetAccessToken();
            var apiContext = new APIContext(accessToken);
            return apiContext;
        }


        //private TechWallDbContext db = new TechWallDbContext();

        //// GET: Admin/Order
        //public ActionResult Index()
        //{
        //    return View(db.Orders.ToList());
        //}

        //// GET: Admin/Order/Details/5
        //public ActionResult Details(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// GET: Admin/Order/Create
        //public ActionResult Create()
        //{
        //    return View();
        //}

        //// POST: Admin/Order/Create
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Create([Bind(Include = "ID,CustomerID,CustomerName,CustomerEmail,CustomerPhone,CustomerCountry,CustomerCity,CustomerAddress,CustomerZipCode,OrderCode,PaymentMethod,TotalAmmount,Discount,DeliveryCharges,FinalAmmount,PlacedOn,TransactionID,ModifiedOn")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Orders.Add(order);
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }

        //    return View(order);
        //}

        //// GET: Admin/Order/Edit/5
        //public ActionResult Edit(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Admin/Order/Edit/5
        //// To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        //// more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public ActionResult Edit([Bind(Include = "ID,CustomerID,CustomerName,CustomerEmail,CustomerPhone,CustomerCountry,CustomerCity,CustomerAddress,CustomerZipCode,OrderCode,PaymentMethod,TotalAmmount,Discount,DeliveryCharges,FinalAmmount,PlacedOn,TransactionID,ModifiedOn")] Order order)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        db.Entry(order).State = EntityState.Modified;
        //        db.SaveChanges();
        //        return RedirectToAction("Index");
        //    }
        //    return View(order);
        //}

        //// GET: Admin/Order/Delete/5
        //public ActionResult Delete(int? id)
        //{
        //    if (id == null)
        //    {
        //        return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        //    }
        //    Order order = db.Orders.Find(id);
        //    if (order == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    return View(order);
        //}

        //// POST: Admin/Order/Delete/5
        //[HttpPost, ActionName("Delete")]
        //[ValidateAntiForgeryToken]
        //public ActionResult DeleteConfirmed(int id)
        //{
        //    Order order = db.Orders.Find(id);
        //    db.Orders.Remove(order);
        //    db.SaveChanges();
        //    return RedirectToAction("Index");
        //}

        //protected override void Dispose(bool disposing)
        //{
        //    if (disposing)
        //    {
        //        db.Dispose();
        //    }
        //    base.Dispose(disposing);
        //}

    }
}

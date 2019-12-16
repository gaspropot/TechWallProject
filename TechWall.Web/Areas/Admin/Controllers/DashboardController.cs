using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechWall.Data;

namespace TechWall.Areas.Admin.Controllers
{
    public class DashboardController : BaseController
    {
        // GET: Admin/Dashboard
        public ActionResult Index()
        {
            //using (TechWallDbContext db = new TechWallDbContext())
            //{
            //    var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
            //    if (user != null)
            //    {
            //        Session["Picture"] = user.Picture.URL;
            //    }
            //}
            return View();
        }
    }
}
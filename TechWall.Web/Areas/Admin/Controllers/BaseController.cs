using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Web;
using System.Web.Mvc;
using TechWall.Data;

namespace TechWall.Areas.Admin.Controllers
{
    [Authorize(Roles ="admin, eshopmanager")]
    public class BaseController : Controller
    {
       

        public BaseController()
        {
         
            
        }

        //private void UserInfo()
        //{
        //    using (TechWallDbContext db = new TechWallDbContext())
        //    {
        //        var user = db.Users.FirstOrDefault(u => u.UserName == User.Identity.Name);
        //        if (user != null)
        //        {
                    
        //            Session["Picture"] = user.Picture.URL;
        //        }
        //    }
        //}


    }
}
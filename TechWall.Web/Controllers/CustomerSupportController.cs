using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using TechWall.Services;
using TechWall.ViewModels;

namespace TechWall.Controllers
{
    public class CustomerSupportController : Controller
    {
        [Authorize(Roles = "eshopmanager, admin")]
        public ActionResult LiveChat()
        {
            return View();
        }

        [HttpPost]
        public ActionResult GetChatBox(string toUserId)
        {
            ChatBoxModel chatBoxModel = new AppService().GetChatBox(toUserId);
            return PartialView("~/Views/Shared/_Partials/_ChatBox.cshtml", chatBoxModel);
        }

        [HttpPost]
        public ActionResult GetManagerChatBox(string toUserId)
        {
            ChatBoxModel chatBoxModel = new AppService().GetManagerChatBox(toUserId);
            return PartialView("~/Views/Shared/_Partials/_ManagerChatBox.cshtml", chatBoxModel);
        }

        [HttpPost]
        public ActionResult SendMessage(string toUserId, string message)
        {
            return Json(new AppService().SendMessage(toUserId, message));
        }

        [HttpPost]
        public ActionResult LoadMessages(string toUserId)
        {
            return Json(new AppService().LoadMessages(toUserId));
        }
    }
}
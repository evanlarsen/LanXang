using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;

namespace LanXang.Web.Controllers
{
    public class AdminController : Controller
    {
        //
        // GET: /Admin/

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Index(string password)
        {
            if (password == "letmein")
            {
                FormsAuthentication.SetAuthCookie("admin", false);
                return RedirectToAction("MainMenu");
            }
            return View();
        }

        [Authorize]
        public ActionResult MainMenu()
        {
            return View();
        }

        [Authorize]
        public ActionResult DinnerMenu()
        {
            return View();
        }

        [Authorize]
        public ActionResult SushiMenu()
        {
            return View();
        }

        [Authorize]
        public ActionResult StoreHours()
        {
            return View();
        }

        [Authorize]
        public ActionResult Gallery()
        {
            return View();
        }

        [Authorize]
        public ActionResult Email()
        {
            return View();
        }
    }
}

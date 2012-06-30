using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LanXang.Web.Viewmodels;

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
            StoreHoursVM vm = new StoreHoursVM()
            {
                Line1 = "Mon-Thurs: 11am-10pm",
                Line2 = "Friday: 11am-10:30pm",
                Line3 = "Saturday: 12-10:30pm"
            };
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult StoreHours(StoreHoursVM vm)
        {
            return View(vm);
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

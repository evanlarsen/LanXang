using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LanXang.Web.Viewmodels;
using LanXang.Web.Core.Data;

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
            MenuVM vm = new MenuVM();
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DinnerMenu(MenuVM vm)
        {
            return View(vm);
        }

        [Authorize]
        public ActionResult SushiMenu()
        {
            return View();
        }

        [Authorize]
        public ActionResult StoreHours()
        {
            using (var r = new Repository())
            {
                var entity = r.StoreHours.First();

                StoreHoursVM vm = new StoreHoursVM()
                {
                    Line1 = entity.Line1,
                    Line2 = entity.Line2,
                    Line3 = entity.Line3
                };
                return View(vm);
            }
        }

        [Authorize]
        [HttpPost]
        public ActionResult StoreHours(StoreHoursVM vm)
        {
            using (var r = new Repository())
            {
                var entity = r.StoreHours.First();
                entity.Line1 = vm.Line1;
                entity.Line2 = vm.Line2;
                entity.Line3 = vm.Line3;
                r.SaveChanges();
            }
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

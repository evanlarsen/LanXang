using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LanXang.Web.Core.Data;
using LanXang.Web.Core.Entities;

namespace LanXang.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
            //using (var r = new Repository())
            //{
            //    r.StoreHours.Add(new StoreHoursEntity() { ID = 1, Line1 = "Test", Line2 = "Test 2", Line3 = "Test 3" });
            //    r.SaveChanges();
            //    var data = from x in r.StoreHours
            //               select x;
            //}
            return View();
        }

        public ActionResult DinnerMenu()
        {
            return View();
        }

        public ActionResult LunchMenu()
        {
            return View();
        }

        public ActionResult SushiMenu()
        {
            return View();
        }

        public ActionResult FeaturedSushi()
        {
            return View();
        }

        public ActionResult Location()
        {
            return View();
        }

        public ActionResult Delivery()
        {
            return View();
        }
    }
}

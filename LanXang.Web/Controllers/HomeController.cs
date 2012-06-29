using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace LanXang.Web.Controllers
{
    public class HomeController : Controller
    {
        //
        // GET: /Home/

        public ActionResult Index()
        {
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

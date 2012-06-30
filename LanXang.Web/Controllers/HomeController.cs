using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using LanXang.Web.Core.Data;
using LanXang.Web.Core.Entities;
using LanXang.Web.Viewmodels;

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
            using (var r = new Repository())
            {
                MenuVM vm = new MenuVM();
                vm.Categories = new List<Category>();

                foreach (var c in r.MenuCategories.Include("MenuItems").Where(c => c.CategoryType == "Dinner"))
                {
                    vm.Categories.Add(new Category()
                    {
                        Sequence = c.Sequence,
                        Name = c.Name,
                        MenuItems = c.MenuItems.Select(
                                        i => new MenuItem()
                                        {
                                            Sequence = i.Sequence,
                                            Name = i.Name,
                                            Description = i.Description,
                                            Price = i.Price
                                        }).ToList()
                    });
                }

                return View(vm);
            }
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

        [ChildActionOnly]
        public ActionResult _StoreHours()
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
    }
}

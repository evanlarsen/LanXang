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
            MenuVM vm = new MenuVM();
            using (var r = new Repository())
            {
                vm.Categories = new List<Category>();

                foreach (var c in r.MenuCategories.Where(c => c.CategoryType == "Dinner"))
                {
                    vm.Categories.Add(new Category()
                    {
                        Sequence = c.Sequence,
                        Name = c.Name
                    });
                }
            }
            return View(vm);
        }

        public ActionResult DinnerMenu()
        {
            MenuVM vm = GetMenuFromStore("Dinner");
            return View(vm);
        }

        public ActionResult LunchMenu()
        {
            MenuVM vm = GetMenuFromStore("Lunch");
            return View(vm);
        }

        public ActionResult SushiMenu()
        {
            MenuVM vm = GetMenuFromStore("Sushi");
            return View(vm);
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

        private MenuVM GetMenuFromStore(string menuType)
        {
            MenuVM vm = new MenuVM();
            using (var r = new Repository())
            {
                vm.Categories = new List<Category>();

                foreach (var c in r.MenuCategories.Include("MenuItems").Where(c => c.CategoryType == menuType))
                {
                    vm.Categories.Add(new Category()
                    {
                        Sequence = c.Sequence,
                        Name = c.Name,
                        CategoryID = c.ID,
                        Description = c.Description,
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
            }
            return vm;
        }
    }
}

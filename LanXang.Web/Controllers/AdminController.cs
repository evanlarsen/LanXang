using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LanXang.Web.Viewmodels;
using LanXang.Web.Core.Data;
using LanXang.Web.Core.Entities;

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
            MenuVM vm = GetMenuFromStore("Dinner");
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult DinnerMenu(MenuVM vm)
        {
            vm = SaveMenu(vm, "Dinner");
            return View(vm);
        }

        [Authorize]
        public ActionResult SushiMenu()
        {
            MenuVM vm = GetMenuFromStore("Sushi");
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult SushiMenu(MenuVM vm)
        {
            vm = SaveMenu(vm, "Sushi");
            return View(vm);
        }

        [Authorize]
        public ActionResult LunchMenu()
        {
            MenuVM vm = GetMenuFromStore("Lunch");
            return View(vm);
        }

        [Authorize]
        [HttpPost]
        public ActionResult LunchMenu(MenuVM vm)
        {
            vm = SaveMenu(vm, "Lunch");
            return View(vm);
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

        private MenuVM SaveMenu(MenuVM vm, string menuType)
        {
            using (var r = new Repository())
            {
                foreach (var c in r.MenuCategories.Include("MenuItems").Where(c => c.CategoryType == menuType))
                {
                    while (c.MenuItems.Any())
                    {
                        r.MenuItems.Remove(c.MenuItems.First());
                    }

                    r.MenuCategories.Remove(c);
                }

                foreach (var c in vm.Categories)
                {
                    r.MenuCategories.Add(new MenuCategoryEntity()
                    {
                        Sequence = c.Sequence,
                        CategoryType = menuType,
                        Name = c.Name,
                        MenuItems = c.MenuItems.ConvertAll(i => new MenuItemEntity()
                        {
                            Sequence = i.Sequence,
                            Name = i.Name,
                            Description = i.Description,
                            Price = i.Price
                        })
                    });
                }

                r.SaveChanges();
            }
            return vm;
        }
    }
}

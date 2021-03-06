﻿using System;
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

                foreach (var c in r.MenuCategories.Where(c => c.CategoryType == "Dinner").Take(7))
                {
                    vm.Categories.Add(new Category()
                    {
                        CategoryID = c.ID,
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
            return View(GetGalleryVM());
        }

        public ActionResult Location()
        {
            return View();
        }

        public ActionResult Delivery()
        {
            return View();
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [HttpGet]
        public ActionResult DownloadFile(Guid id)
        {
            using (Repository r = new Repository())
            {
                FileUploadEntity file = r.Files.FirstOrDefault(f => f.ID == id);

                if (file == null)
                {
                    throw new HttpException(404, "HTTP/1.1 404 Not Found");
                }

                return File(file.FileContents, file.ContentType);
            }
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
                        CategoryID = c.ID,
                        Sequence = c.Sequence,
                        Name = c.Name,
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

        private GalleryVM GetGalleryVM()
        {
            GalleryVM vm = new GalleryVM();
            vm.Images = new List<GalleryImageVM>();
            using (Repository r = new Repository())
            {
                foreach (var f in r.Files)
                {
                    vm.Images.Add(
                        new GalleryImageVM()
                        {
                            ID = f.ID.ToString(),
                            Url = Url.Action("DownloadFile", new { id = f.ID }),
                            DeleteUrl = Url.Action("DeleteFile", new { id = f.ID }),
                            Name = f.Name,
                            Description = f.Description
                        });
                }
            }
            return vm;
        }
    }
}

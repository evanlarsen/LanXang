using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using LanXang.Web.Viewmodels;
using LanXang.Web.Core.Data;
using LanXang.Web.Core.Entities;
using System.IO;
using System.Drawing;
using System.Drawing.Drawing2D;

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
            return View(GetGalleryVM());
        }

        [Authorize]
        [HttpPost]
        public ActionResult Gallery(GalleryVM gallery)
        {
            if (gallery != null && gallery.Images != null)
            {
                using (Repository r = new Repository())
                {
                    foreach (var image in r.Files)
                    {
                        var i = gallery.Images.Find(x => x.ID == image.ID.ToString());

                        if (i == null)
                        {
                            r.Files.Remove(image);
                        }
                        else
                        {
                            image.Description = i.Description;
                        }
                    }

                    r.SaveChanges();
                }
            }
            return View(GetGalleryVM());
        }

        [Authorize]
        public ActionResult Email()
        {
            return View();
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        [Authorize]
        [HttpGet]
        public ActionResult DeleteFile(Guid id)
        {
            using (Repository r = new Repository())
            {
                FileUploadEntity f = r.Files.FirstOrDefault(x => x.ID == id);
                if (f != null)
                {
                    r.Files.Remove(f);
                    r.SaveChanges();
                }
            }

            return RedirectToAction("Gallery");
        }

        [Authorize]
        [HttpPost]
        public ActionResult UploadFiles()
        {
            var headers = Request.Headers;

            if (string.IsNullOrEmpty(headers["X-File-Name"]))
            {
                UploadWholeFile(Request);
            }
            else
            {
                UploadPartialFile(headers["X-File-Name"], Request);
            }

            return RedirectToAction("Gallery");
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadPartialFile(string fileName, HttpRequestBase request)
        {
            if (request.Files.Count != 1) throw new HttpRequestValidationException("Attempt to upload chunked file containing more than one fragment per request");
            var file = request.Files[0];
            UpdateFile(fileName, file);
        }

        //DONT USE THIS IF YOU NEED TO ALLOW LARGE FILES UPLOADS
        //Credit to i-e-b and his ASP.Net uploader for the bulk of the upload helper methods - https://github.com/i-e-b/jQueryFileUpload.Net
        private void UploadWholeFile(HttpRequestBase request)
        {
            for (int i = 0; i < request.Files.Count; i++)
            {
                var file = request.Files[i];
                UpdateFile(file.FileName, file);
            }
        }

        private void UpdateFile(string fileName, HttpPostedFileBase file)
        {
            var inputStream = file.InputStream;

            FileUploadEntity fileEntity = new FileUploadEntity();
            fileEntity.ID = Guid.NewGuid();
            fileEntity.Name = Path.GetFileName(fileName);
            fileEntity.ContentType = file.ContentType;

            byte[] fileContents = new byte[file.ContentLength];
            inputStream.Read(fileContents, 0, file.ContentLength);
            fileEntity.FileContents = ResizeImage(fileContents, 400, 250);

            using (Repository r = new Repository())
            {
                r.Files.Add(fileEntity);
                r.SaveChanges();
            }
        }

        private string EncodeFile(string fileName)
        {
            return Convert.ToBase64String(System.IO.File.ReadAllBytes(fileName));
        }

        private byte[] ResizeImage(byte[] file, int maxWidth, int maxHeight)
        {
            using (MemoryStream ms = new MemoryStream(file))
            {
                using (Image img = Image.FromStream(ms))
                {
                    if (maxWidth >= img.Width && maxHeight >= img.Height)
                    {
                        return file;
                    }

                    double widthRatio = (double)maxWidth / (double)img.Width;
                    double heightRatio = (double)maxHeight / (double)img.Height;

                    double ratio = Math.Min(widthRatio, heightRatio);

                    int newWidth = (int)(img.Width * ratio);
                    int newHeight = (int)(img.Height * ratio);

                    Image thumbNail = new Bitmap(newWidth, newHeight, img.PixelFormat);
                    Graphics g = Graphics.FromImage(thumbNail);
                    g.CompositingQuality = CompositingQuality.HighQuality;
                    g.SmoothingMode = SmoothingMode.HighQuality;
                    g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                    Rectangle rect = new Rectangle(0, 0, newWidth, newHeight);
                    g.DrawImage(img, rect);

                    using (MemoryStream result = new MemoryStream())
                    {
                        thumbNail.Save(result, img.RawFormat);
                        return result.ToArray();
                    }
                }
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
                            Url = Url.Action("DownloadFile", "Home", new { id = f.ID }),
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

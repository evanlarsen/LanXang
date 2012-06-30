using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace LanXang.Web.Viewmodels
{
    public class MenuVM
    {
        public List<Category> Categories { get; set; }
    }

    public class Category
    {
        public int Sequence { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public List<MenuItem> MenuItems { get; set; }
    }

    public class MenuItem
    {
        public int Sequence { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string Price { get; set; }
    }
}
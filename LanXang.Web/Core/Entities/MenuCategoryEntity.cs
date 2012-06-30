using System.Collections.Generic;

namespace LanXang.Web.Core.Entities
{
    public class MenuCategoryEntity
    {
        public int ID { get; set; }
        public string CategoryType { get; set; }
        public string Name { get; set; }
        public IEnumerable<MenuItemEntity> MenuItems { get; set; }
    }
}

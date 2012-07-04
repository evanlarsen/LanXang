using System.Collections.Generic;

namespace LanXang.Web.Viewmodels
{
    public class GalleryVM
    {
        public List<GalleryImageVM> Images { get; set; }
    }

    public class GalleryImageVM
    {
        public string ID { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Url { get; set; }
    }
}
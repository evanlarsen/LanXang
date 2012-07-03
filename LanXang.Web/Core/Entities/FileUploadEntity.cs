using System;

namespace LanXang.Web.Core.Entities
{
    public class FileUploadEntity
    {
        public Guid ID { get; set; }
        public string Name { get; set; }
        public byte[] FileContents { get; set; }
        public string ContentType { get; set; }
    }
}

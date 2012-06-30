using System.ComponentModel.DataAnnotations;

namespace LanXang.Web.Core.Entities
{
    [Table("StoreHours", Schema = "LanXang")]
    public class StoreHoursEntity
    {
        [Key]
        public int ID { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
    }
}
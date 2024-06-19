using QuickTie.Data.Models;
using QuickTie.Cloud.Helpers;

namespace QuickTie.Portal.Models
{
    public class ProductTypeInfo
    {
        public ProductType Type { get; set; }
        public string DisplayName { get; set; }
        public long Count { get; set; }
        public string ImageUrl { get; set; }
    }
}

using SNKRS.Models;
using System.Collections.Generic;

namespace SNKRS.ViewModels
{
    public class ProductViewModel
    {
        public Product Product { get; set; }
        public IEnumerable<Product> RelatedProducts { get; set; }
        public IEnumerable<Review> Reviews { get; set; }
        public IEnumerable<ProductSize> ProductSizes { get; set; }
    }
}
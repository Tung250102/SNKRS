using SNKRS.Models;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace SNKRS.Areas.Admin.ViewModels
{
    public class ProductViewModel
    {
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public decimal Price { get; set; }
        public decimal SalePrice { get; set; }

        [AllowHtml]
        public string Description { get; set; }

        [Required]
        public string Image { get; set; }

        [Display(Name = "Product Visibility")]
        public bool isVisible { get; set; }

        public IEnumerable<Category> Categories { get; set; }
        public IEnumerable<int> ProductCategories { get; set; }

        public IEnumerable<ProductGallery> ProductGalleries { get; set; }

        public IEnumerable<ProductSize> ProductSizes { get; set; }
    }
}
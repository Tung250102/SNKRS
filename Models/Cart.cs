using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;
using System.ComponentModel.DataAnnotations;

namespace SNKRS.Models
{
    public class Cart
    {
        ApplicationDbContext db = new ApplicationDbContext();
        public Cart(int Id, int Quantity)
        {
            var productSize = db.ProductSizes.Include(x => x.Product).FirstOrDefault(x => x.Id == Id);
            ProductName = productSize.Product.Name;
            ProductImage = productSize.Product.Image;
            ProductPrice = productSize.Product.Price;
            ProductSalePrice = productSize.Product.SalePrice;
            ProductSize = productSize.Size;
            ProductSizeId = Id;
            this.Quantity = Quantity;
        }
        [Display(Name = "Name")]
        public string ProductName { get; set; }
        [Display(Name = "Image")]
        public string ProductImage { get; set; }
        public int ProductSizeId { get; set; }
        [Display(Name = "Size")]
        public string ProductSize { get; set; }
        [Display(Name = "Quantity")]
        public int Quantity { get; set; }
        [Display(Name = "Price")]
        public decimal ProductPrice { get; set; }
        public decimal ProductSalePrice { get; set; }
        public decimal Amount
        {
            get { return ProductSalePrice > 0 ? Quantity * ProductSalePrice : Quantity * ProductPrice; }
        }

        public IEnumerable<object> Items { get; internal set; }
    }
}
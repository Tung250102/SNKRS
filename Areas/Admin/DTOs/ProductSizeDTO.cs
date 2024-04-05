using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNKRS.Areas.Admin.DTOs
{
    public class ProductSizeDTO
    {
        public int productId { get; set; }
        public string size { get; set; }
        public int quantity { get; set; }
    }
}
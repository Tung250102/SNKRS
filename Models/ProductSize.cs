using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SNKRS.Models
{
    public class ProductSize
    {
        public ProductSize()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }
        public int Id { get; set; }

        public int ProductId { get; set; }

        public string Size { get; set; }

        public int Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
    }
}
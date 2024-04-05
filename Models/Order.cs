using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace SNKRS.Models
{
    public class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
        }

        public int Id { get; set; }

        public string CustomerId { get; set; }

        [Required]
        [Display(Name = "Customer")]
        public string Name { get; set; }

        [Required]
        [Column(TypeName = "money")]
        public decimal Amount { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        [Display(Name = "Created At")]
        public DateTime Created_At { get; set; }

        public int StatusId { get; set; }

        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

        [Display(Name = "Order Status")]
        public virtual OrderStatus OrderStatus { get; set; }

        public virtual ApplicationUser Customer { get; set; }
        
    }
}
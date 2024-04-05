using SNKRS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace SNKRS.Areas.Admin.ViewModels
{
    public class OrderViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Customer")]
        public string Name { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }
        [Display(Name = "Created At")]
        public DateTime Created_At { get; set; }
        public decimal Amount { get; set; }
        public int StatusId { get; set; }
        [Display(Name = "Order Details")]
        public IEnumerable<OrderDetail> OrderDetails { get; set; }
        [Display(Name = "Order Status")]
        public IEnumerable<OrderStatus> OrderStatuses { get; set; }
    }
}
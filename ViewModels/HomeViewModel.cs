using SNKRS.Models;
using System.Collections.Generic;

namespace SNKRS.ViewModels
{
    public class HomeViewModel
    {
        public IEnumerable<Product> Upcoming { get; set; }
        public IEnumerable<Product> Best_selling { get; set; }
        public IEnumerable<Product> OnSale { get; set; }
    }
}
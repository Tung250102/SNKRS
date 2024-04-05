using SNKRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using PagedList;

namespace SNKRS.ViewModels
{
    public class ShopViewModel
    {
        public IPagedList<Product> Products { get; set; }
        public IEnumerable<string> Categories { get; set; }
        public IEnumerable<string> Sizes { get; set; }
        public IEnumerable<string> AllCategories { get; set; }
        public IEnumerable<string> AllSizes { get; set; }
    }
}
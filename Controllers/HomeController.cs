using SNKRS.Models;
using SNKRS.ViewModels;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Data.Entity;

namespace SNKRS.Controllers
{
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext db;

        public HomeController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            List<Product> upcoming = db.Products.Where(x => x.isVisible).OrderBy(p => p.Id).OrderByDescending(p => p.Id).Take(4).ToList();
            var best_selling = db.Products.SqlQuery("select P.* from Products P inner join ( select P.Id, Count(P.Id) as Count from OrderDetails OD inner join ProductSizes PS on OD.ProductSizeId = PS.Id inner join Products P on PS.ProductId = P.Id where P.isVisible = 'true' group by P.Id ) A on P.Id = A.Id order by A.Count desc").Take(4).ToList<Product>();
            HomeViewModel model = new HomeViewModel();
            model.Upcoming = upcoming;
            model.Best_selling = best_selling;
            model.OnSale = db.Products.Where(x => x.SalePrice > 0).Take(8).ToList();
            return View(model);
        }
    }
}
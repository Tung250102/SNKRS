using SNKRS.Models;
using SNKRS.ViewModels;
using System.Linq;
using System.Web.Mvc;

namespace SNKRS.Controllers
{
    public class ProductController : Controller
    {
        private readonly ApplicationDbContext db;

        public ProductController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Details(int? Id)
        {
            if (Id == null) return HttpNotFound();
            var product = db.Products.FirstOrDefault(p => p.Id == Id && p.isVisible);
            if (product == null) return HttpNotFound();
            var productSizes = db.ProductSizes.Where(p => p.ProductId == Id).ToList();
            var relatedProducts = db.Products.SqlQuery($"select * from Products P where P.Id in ( select ProductId from ProductCategory PC where PC.CategoryId in ( select PC.CategoryId from ProductCategory PC where PC.ProductId = {product.Id} ) and PC.ProductId <> {product.Id} )").Take(4).ToList();
            if (!relatedProducts.Any())
            {
                relatedProducts = db.Products.Take(4).ToList();
            }
            var model = new ProductViewModel();
            model.Product = product;
            model.ProductSizes = productSizes;
            model.RelatedProducts = relatedProducts;
            model.Reviews = db.Reviews.Where(x => x.ProductId == Id).OrderByDescending(x => x.Id).Take(5).ToList();
            return View(model);
        }

        public void AddReview(int Id, string Content, int Rating)
        {
            if (!string.IsNullOrEmpty(Content))
            {
                var review = new Review()
                {
                    ProductId = Id,
                    Content = Content,
                    Rating = Rating,
                    DateTime = System.DateTime.Now,
                };
                db.Reviews.Add(review);
                db.SaveChanges();
            }
        }
    }
}
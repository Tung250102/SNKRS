using SNKRS.Areas.Admin.DTOs;
using SNKRS.Areas.Admin.ViewModels;
using SNKRS.Models;
using System.Linq;
using System.Web.Mvc;

namespace SNKRS.Areas.Admin.Controllers
{
    [Authorize]
    public class ProductController : AdminController
    {
        private readonly ApplicationDbContext db;

        public ProductController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        public ActionResult Create()
        {
            ProductViewModel viewModel = new ProductViewModel
            {
                Categories = db.Categories.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(ProductViewModel viewModel)
        {
            if (!ModelState.IsValid)
            {
                viewModel.Categories = db.Categories.ToList();
                return View("Create", viewModel);
            }
            var product = new Product()
            {
                Name = viewModel.Name,
                Price = viewModel.Price,
                SalePrice = viewModel.SalePrice,
                Image = viewModel.Image,
                Description = viewModel.Description,
                isVisible = viewModel.isVisible
            };
            if (viewModel.ProductCategories != null)
            {
                foreach (var item in viewModel.ProductCategories)
                {
                    product.Categories.Add(db.Categories.Single(x => x.Id == item));
                }
            }
            db.Products.Add(product);
            db.SaveChanges();
            return RedirectToAction("Edit", new { Id = product.Id });
        }

        public void Delete(int Id)
        {
            var product = db.Products.SingleOrDefault(p => p.Id == Id);
            db.Products.Remove(product);
            db.SaveChanges();
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null) return HttpNotFound();
            var product = db.Products.SingleOrDefault(p => p.Id == Id);
            if (product == null) return HttpNotFound();
            var viewmodel = new ProductViewModel
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price,
                SalePrice = product.SalePrice,
                Description = product.Description,
                Image = product.Image,
                ProductCategories = product.Categories.Select(x => x.Id).ToList(),
                Categories = db.Categories.ToList(),
                ProductSizes = db.ProductSizes.Where(p => p.ProductId == Id).ToList(),
                ProductGalleries = product.ProductGalleries,
                isVisible = product.isVisible,
            };
            return View(viewmodel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(ProductViewModel viewModel)
        {
            var product = db.Products.First(p => p.Id == viewModel.Id);
            product.Name = viewModel.Name;
            product.Description = viewModel.Description;
            product.Image = viewModel.Image;
            product.Price = viewModel.Price;
            product.SalePrice = viewModel.SalePrice;
            product.isVisible = viewModel.isVisible;
            product.Categories.Clear();
            if (viewModel.ProductCategories != null)
            {
                foreach (var item in viewModel.ProductCategories)
                {
                    product.Categories.Add(db.Categories.Single(x => x.Id == item));
                }
            }
            db.SaveChanges();
            return RedirectToAction("Edit", new { Id = viewModel.Id });
        }

        [HttpPost]
        public ActionResult AddSize(ProductSizeDTO productSizeDTO)
        {
            var productSize = new ProductSize()
            {
                ProductId = productSizeDTO.productId,
                Size = productSizeDTO.size,
                Quantity = productSizeDTO.quantity,
            };
            db.ProductSizes.Add(productSize);
            db.SaveChanges();
            return Json(productSize);
        }

        public void DeleteSize(int Id)
        {
            var productSize = db.ProductSizes.FirstOrDefault(p => p.Id == Id);
            db.ProductSizes.Remove(productSize);
            db.SaveChanges();
        }

        public void AddGallery(int Id, string Src)
        {
            var product = db.Products.FirstOrDefault(x => x.Id == Id);
            if (product != null)
            {
                var productGallery = new ProductGallery()
                {
                    ProductId = product.Id,
                    Src = Src,
                };
                db.ProductGalleries.Add(productGallery);
                db.SaveChanges();
            }
        }
    }
}
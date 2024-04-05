using SNKRS.Models;
using System.Web.Mvc;
using System.Linq;
using System;
using SNKRS.Areas.Admin.ViewModels;

namespace SNKRS.Areas.Admin.Controllers
{
    [Authorize]
    public class DashboardController : AdminController
    {
        private readonly ApplicationDbContext db;
        public DashboardController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            decimal total = db.Orders.Where(x => x.StatusId == 3).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            decimal thisMonth = db.Orders.Where(x => x.StatusId == 3 && x.Created_At.Month == DateTime.Now.Month && x.Created_At.Year == DateTime.Now.Year).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            decimal pending = db.Orders.Where(x => x.StatusId == 1 || x.StatusId == 2).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            decimal cancelled = db.Orders.Where(x => x.StatusId == 4).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            var dayTotal = db.Orders.Where(x => x.Created_At.Day == DateTime.Now.Day && x.Created_At.Month == DateTime.Now.Month && x.Created_At.Year == DateTime.Now.Year).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            var dayOrder = db.Orders.Where(x => x.Created_At.Day == DateTime.Now.Day && x.Created_At.Month == DateTime.Now.Month && x.Created_At.Year == DateTime.Now.Year).Count();
            var dayProducts = db.Products.SqlQuery($"select P.* from ( select PS.ProductId, Count(PS.ProductId) as Count from Orders O inner join OrderDetails OD on O.Id = OD.OrderId inner join ProductSizes PS on OD.ProductSizeId = PS.Id where Day(O.Created_At) = {DateTime.Now.Day} and Month(O.Created_At) = {DateTime.Now.Month} and Year(O.Created_At) = {DateTime.Now.Year} group by PS.ProductId ) A inner join Products P on P.Id = A.ProductId order by A.Count desc").ToList();
            var viewModel = new DashboardViewModel()
            {
                Total = total,
                Pending = pending,
                ThisMonth = thisMonth,
                Cancelled = cancelled,
                DayTotal = dayTotal,
                DayOrder = dayOrder,
                DayProducts = dayProducts,
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Index(FormCollection form)
        {
            var date = Convert.ToDateTime(form["datePicker"]);
            decimal total = db.Orders.Where(x => x.StatusId == 3).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            decimal thisMonth = db.Orders.Where(x => x.StatusId == 3 && x.Created_At.Month == DateTime.Now.Month && x.Created_At.Year == DateTime.Now.Year).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            decimal pending = db.Orders.Where(x => x.StatusId == 1 || x.StatusId == 2).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            decimal cancelled = db.Orders.Where(x => x.StatusId == 4).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            var dayTotal = db.Orders.Where(x => x.Created_At.Day == date.Day && x.Created_At.Month == date.Month && x.Created_At.Year == date.Year && x.StatusId != 4).Select(x => x.Amount).DefaultIfEmpty(0).Sum();
            var dayOrder = db.Orders.Where(x => x.Created_At.Day == date.Day && x.Created_At.Month == date.Month && x.Created_At.Year == date.Year && x.StatusId != 4).Count();
            var dayProducts = db.Products.SqlQuery($"select P.* from ( select PS.ProductId, Count(PS.ProductId) as Count from Orders O inner join OrderDetails OD on O.Id = OD.OrderId inner join ProductSizes PS on OD.ProductSizeId = PS.Id where Day(O.Created_At) = {date.Day} and Month(O.Created_At) = {date.Month} and Year(O.Created_At) = {date.Year} group by PS.ProductId ) A inner join Products P on P.Id = A.ProductId order by A.Count desc").ToList<Product>();
            var viewModel = new DashboardViewModel()
            {
                Total = total,
                Pending = pending,
                ThisMonth = thisMonth,
                Cancelled = cancelled,
                DayTotal = dayTotal,
                DayOrder = dayOrder,
                DayProducts = dayProducts,
            };
            return View(viewModel);
        }
    }
}
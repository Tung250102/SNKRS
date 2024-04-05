using SNKRS.Areas.Admin.ViewModels;
using SNKRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNKRS.Areas.Admin.Controllers
{
    public class OrderController : AdminController
    {
        private readonly ApplicationDbContext db;

        public OrderController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            var orders = db.Orders.ToList();
            return View(orders);
        }

        public ActionResult Edit(int? Id)
        {
            if (Id == null) return HttpNotFound();
            var order = db.Orders.FirstOrDefault(o => o.Id == Id);
            if (order == null) return HttpNotFound();
            var viewModel = new OrderViewModel()
            {
                Id = order.Id,
                Name = order.Name,
                Phone = order.Phone,
                Address = order.Address,
                Created_At = order.Created_At,
                StatusId = order.StatusId,
                Amount = order.Amount,
                OrderDetails = order.OrderDetails,
                OrderStatuses = db.OrderStatuses.ToList()
            };
            return View(viewModel);
        }

        [HttpPost]
        public ActionResult Edit(OrderViewModel viewModel)
        {
            var order = db.Orders.FirstOrDefault(o => o.Id == viewModel.Id);
            order.StatusId = viewModel.StatusId;
            db.SaveChanges();
            return RedirectToAction("Index");
        }
    }
}
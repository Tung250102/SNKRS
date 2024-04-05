using SNKRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SNKRS.Controllers
{
    public class TrackingController : Controller
    {
        
        private readonly ApplicationDbContext db;

        public TrackingController()
        {
            db = new ApplicationDbContext();
        }

        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken] //Cross-Site Request Forgery
        public ActionResult Index(FormCollection formCollection)
        {
            ViewBag.Search = true;
            int orderId = Convert.ToInt32(formCollection["Id"]);
            var order = db.Orders.FirstOrDefault(x => x.Id == orderId);
            return View(order);
        }
    }
}
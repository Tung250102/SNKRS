using SNKRS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using PayPal.Api;

namespace SNKRS.Controllers
{
    public class CartController : Controller
    {
        private readonly ApplicationDbContext db;
        public CartController()
        {
            db = new ApplicationDbContext();
        }
        public List<Cart> Get()
        {
            var cart = Session["Cart"] as List<Cart>;
            if (cart == null)
            {
                cart = new List<Cart>();
                Session["Cart"] = cart;
            }
            return cart;
        }

        public void Add(int Id, int Quantity)
        {
            var cart = Get();
            Cart item = cart.FirstOrDefault(c => c.ProductSizeId == Id);
            if (item == null)
            {
                item = new Cart(Id, Quantity);
                cart.Add(item);
            }
            else
            {
                item.Quantity += Quantity;
            }
        }

        public int GetCartAmount()
        {
            var cart = Get();
            return cart.Count;
        }

        public decimal Amount()
        {
            decimal amount = 0;
            var cart = Get();
            if (cart != null)
            {
                amount = cart.Sum(c => c.Amount);
            }
            return amount;
        }

        public int IncreaseQuantity(int Id)
        {
            var cart = Get();
            var item = cart.First(x => x.ProductSizeId == Id);
            if (item != null)
            {
                item.Quantity = item.Quantity+1;
            }
            return item.Quantity;
        }

        public int DecreaseQuantity(int Id)
        {
            var cart = Get();
            var item = cart.First(x => x.ProductSizeId == Id);
            if (item != null && item.Quantity > 0)
            {
                item.Quantity = item.Quantity - 1;
            }
            return item.Quantity;
        }

        public ActionResult Index()
        {
            var cart = Get();
            ViewBag.Amount = Amount();
            return View(cart);
        }

        public void Delete(int Id)
        {
            var cart = Get();
            var item = cart.First(x => x.ProductSizeId == Id);
            if (item != null)
            {
                cart.Remove(item);
            }
        }

        public ActionResult Checkout()
        {
            var cart = Get();
            if (cart.Count == 0)
            {
                return RedirectToAction("Index");
            }
            if (User.Identity.IsAuthenticated)
            {
                var userId = User.Identity.GetUserId();
                var user = db.Users.FirstOrDefault(x => x.Id == userId);
                ViewBag.User = user;
            }
            ViewBag.Cart = cart;
            ViewBag.Amount = Amount();
            return View();
        }

        [HttpPost]
        public ActionResult Checkout(Models.Order order)
        {
            var cart = Get();

            if (!ModelState.IsValid)
            {
                if (cart.Count == 0)
                {
                    return RedirectToAction("Index");
                }
                ViewBag.Cart = cart;
                ViewBag.Amount = Amount();
                return View(order);
            }

            order.Created_At = DateTime.Now;
            order.Amount = Amount();
            order.StatusId = 1;

            if (User.Identity.IsAuthenticated)
            {
                order.CustomerId = User.Identity.GetUserId();
            }

            db.Orders.Add(order);

            cart = Get();

            foreach (var item in cart)
            {
                var orderDetail = new OrderDetail()
                {
                    OrderId = order.Id,
                    ProductSizeId = item.ProductSizeId,
                    Quantity = item.Quantity,
                };

                var productSize = db.ProductSizes.First(x => x.Id == item.ProductSizeId);
                productSize.Quantity -= item.Quantity;

                db.OrderDetails.Add(orderDetail);
            }

            db.SaveChanges();

            // Clear the cart after the order is successfully placed
            ClearCart();

            return RedirectToAction("Success", new { order.Id });
        }

        private void ClearCart()
        {
            
            Session["Cart"] = new List<CartItem>();
        }
        public ActionResult FailureView()
        {
            return View();
        }
        public ActionResult Success(int Id)
        {
            ViewBag.Id = Id;
            return View();
        }
        public ActionResult SuccessView()
        {
            ClearCart();
            return View();
        }


        public ActionResult PaymentWithPaypal(string Cancel = null)
        {
            APIContext apiContext = PaypalConfiguration.GetAPIContext();
            try
            {
                string payerId = Request.Params["PayerID"];
                if (string.IsNullOrEmpty(payerId))
                {
                    string baseURI = Request.Url.Scheme + "://" + Request.Url.Authority + "/cart/PaymentWithPayPal?";
                    var guid = Convert.ToString((new Random()).Next(100000));
                    var createdPayment = this.CreatePayment(apiContext, baseURI + "guid=" + guid);
                    var links = createdPayment.links.GetEnumerator();
                    string paypalRedirectUrl = null;
                    while (links.MoveNext())
                    {
                        Links lnk = links.Current;
                        if (lnk.rel.ToLower().Trim().Equals("approval_url"))
                        {
                            paypalRedirectUrl = lnk.href;
                        }
                    }
                    Session.Add(guid, createdPayment.id);
                    return Redirect(paypalRedirectUrl);
                }
                else
                {
                    var guid = Request.Params["guid"];
                    var executedPayment = ExecutePayment(apiContext, payerId, Session[guid] as string);
                    if (executedPayment.state.ToLower() != "approved")
                    {
                        return View("FailureView");
                    }
                    string orderId = executedPayment.id;
                    ClearCart();
                    ViewBag.OrderId = orderId;

                    return View("SuccessView");
                }
            }
            catch (Exception ex)
            {
                return View("FailureView");
            }
        }
        private PayPal.Api.Payment payment;
        private Payment ExecutePayment(APIContext apiContext, string payerId, string paymentId)
        {
            var paymentExecution = new PaymentExecution()
            {
                payer_id = payerId
            };
            this.payment = new Payment()
            {
                id = paymentId
            };
            return this.payment.Execute(apiContext, paymentExecution);
        }
        private Payment CreatePayment(APIContext apiContext, string redirectUrl)
        {
            
           
            var itemList = new ItemList()
            {
                items = new List<Item>()
            };
           
            
            {

            }
            itemList.items.Add(new Item()
            {
                name = "Item Name comes here",
                currency = "USD",
                price = "1",
                quantity = "1",
                sku = "sku"
            });
            var payer = new Payer()
            {
                payment_method = "paypal"
            };
            
            var redirUrls = new RedirectUrls()
            {
                cancel_url = redirectUrl + "&Cancel=true",
                return_url = redirectUrl
            };
            
            var details = new Details()
            {
                tax = "1",
                shipping = "1",
                subtotal = "1"
            };
           
            var amount = new Amount()
            {
                currency = "USD",
                total = "3",  
                details = details
            };
            var transactionList = new List<Transaction>();
            
            var paypalOrderId = DateTime.Now.Ticks;
            transactionList.Add(new Transaction()
            {
                description = $"Invoice #{paypalOrderId}",
                invoice_number = paypalOrderId.ToString(),   
                amount = amount,
                item_list = itemList
            });
            this.payment = new Payment()
            {
                intent = "sale",
                payer = payer,
                transactions = transactionList,
                redirect_urls = redirUrls
            };
            
            return this.payment.Create(apiContext);
        }
        



    }
}
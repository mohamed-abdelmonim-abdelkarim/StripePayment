using Microsoft.AspNetCore.Mvc;
using PaymentWebApp.Models;
using Stripe;
using Stripe.Checkout;
using System.Diagnostics;

namespace PaymentWebApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        [HttpPost]
        public IActionResult CreateCheckoutSession()
        {
            var options = new SessionCreateOptions
            {
                PaymentMethodTypes = new List<string> { "card" },
                LineItems = new List<SessionLineItemOptions>
            {
                new SessionLineItemOptions
                {
                    PriceData = new SessionLineItemPriceDataOptions
                    {
                        Currency = "usd",
                        ProductData = new SessionLineItemPriceDataProductDataOptions
                        {
                            Name = "Sample Product",
                        },
                        UnitAmount = 500, 
                    },
                    Quantity = 1,
                },
            },
                Mode = "payment",
                SuccessUrl = Url.Action("PaymentSuccess", "Home", null, Request.Scheme),
                CancelUrl = Url.Action("PaymentFailed", "Home", null, Request.Scheme),
            };

            var service = new SessionService();
            var session = service.Create(options);

            return Json(new { id = session.Id });
        }

        public IActionResult PaymentSuccess()
        {
            ViewBag.Message = "Payment succeeded!";
            return View();
        }

        public IActionResult PaymentFailed()
        {
            ViewBag.Message = "Payment failed or cancelled.";
            return View();
        }
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}

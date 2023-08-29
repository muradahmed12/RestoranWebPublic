using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using RestoranWeb.Data;
using RestoranWeb.Models;

namespace RestoranWeb.Controllers
{
    public class CheckoutController : Controller
    {
        private AppDbContext _context;

        public CheckoutController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
         }

        [HttpPost]
        public IActionResult Index(Order model)
        {
            var user = _context.GetLoggedInUser().Id;
            model.UserId = user;
            var existing = _context.ShoppingCart.Where(m => m.UserId == user).Include(m => m.CartItems).FirstOrDefault();
            model.Details = existing.CartItems.Select(m => new OrderDetail
            {
                FoodItemId = m.FoodItemId,
                QuantityDemanded = m.Quantity
            }).ToList();

            model.Details.ForEach(m => m.OrderId = model.Id);
            _context.Add(model);
            _context.RemoveRange(existing.CartItems);
            _context.Remove(existing);

            int r = _context.SaveChanges();

            return RedirectToAction("Index", "Home");
          
        }
    }
}

using Microsoft.AspNetCore.Mvc;
using RestoranWeb.Handlers;

namespace RestoranWeb.Controllers
{
    public class HomeController : Controller
    {
        [Authorized]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult FoodItem()
        {
            return View();
        } 
        public IActionResult Cart()
        {
            return View();
        }
    }
}

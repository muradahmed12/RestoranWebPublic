using Microsoft.AspNetCore.Mvc;
using RestoranWeb.Data;
using System.Drawing.Text;
using RestoranWeb.Models;
using RestoranWeb.Models;

namespace RestoranWeb.Controllers
{
    public class RemoteValidationsController : Controller
    {
            private readonly AppDbContext _context;

        public RemoteValidationsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult FoodTypeNameCheck(FoodType foodType)
        {
            var existing = _context.FoodType.Where(m => m.Id != foodType.Id && m.Name == foodType.Name).Any();

            return Json(!existing);
        }
    }
}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestoranWeb.Data;
using RestoranWeb.Models;

namespace RestoranWeb.Controllers
{
    public class FoodTypesController : Controller
    {
        private readonly AppDbContext _context;

        public FoodTypesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FoodTypes
        public async Task<IActionResult> Index(string k , CategoryType? type)
        {
            if (type == null)
            {
                type = CategoryType.Lunch;
            }
            ViewBag.type = type;

            var categoryQuery = _context.FoodType.Where(m => m.Type == type);
            if (!string.IsNullOrWhiteSpace(k))
            {
                categoryQuery = categoryQuery.Where(m => m.Name.StartsWith(k) || m.Description.Contains(k));
            }
            ViewBag.searchUrl = "/FoodType";
            //ViewBag.searchKeyword = k;

            var data = categoryQuery.Select(m => new FoodTypeViewModel
            {
               
                CategoryWiseFood = m.CategoryWiseFood.Count(),
                Id = m.Id,
                LogoUrl = m.LogoUrl,
                Name = m.Name,
               
                Type = m.Type
            }).ToList();
            return View(await _context.FoodType.ToListAsync());
        }

        // GET: FoodTypes/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.FoodType == null)
            {
                return NotFound();
            }

            var foodType = await _context.FoodType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodType == null)
            {
                return NotFound();
            }

            return View(foodType);
        }

        // GET: FoodTypes/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: FoodTypes/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Description,LogoUrl,Logo,Type")] FoodType model)
        {
            if (ModelState.IsValid)
            {
                if (model.Logo != null && model.Logo.Length > 0)
                {
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string appPath = Path.Combine("images", "FoodType");
                    string fileName = Path.GetRandomFileName().Replace(".", "") + Path.GetExtension(model.Logo.FileName);
                    string directryPath = Path.Combine(basePath, appPath);
                    Directory.CreateDirectory(directryPath);

                    using var stream = new FileStream(Path.Combine(directryPath, fileName), FileMode.Create);
                    model.Logo.CopyTo(stream);
                    model.LogoUrl = Path.Combine(appPath, fileName).Replace("\\", "/");
                }
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: FoodTypes/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.FoodType == null)
            {
                return NotFound();
            }

            var foodType = await _context.FoodType.FindAsync(id);
            if (foodType == null)
            {
                return NotFound();
            }
            return View(foodType);
        }

        // POST: FoodTypes/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Description,LogoUrl,Type")] FoodType foodType)
        {
            if (id != foodType.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foodType);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodTypeExists(foodType.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(foodType);
        }

        // GET: FoodTypes/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.FoodType == null)
            {
                return NotFound();
            }

            var foodType = await _context.FoodType
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodType == null)
            {
                return NotFound();
            }

            return View(foodType);
        }

        // POST: FoodTypes/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.FoodType == null)
            {
                return Problem("Entity set 'AppDbContext.FoodType'  is null.");
            }
            var foodType = await _context.FoodType.FindAsync(id);
            if (foodType != null)
            {
                _context.FoodType.Remove(foodType);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodTypeExists(string id)
        {
          return _context.FoodType.Any(e => e.Id == id);
        }
    }
}

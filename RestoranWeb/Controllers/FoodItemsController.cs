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
    public class FoodItemsController : Controller
    {
        private readonly AppDbContext _context;

        public FoodItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: FoodItems
        public async Task<IActionResult> Index(string categoryId)
        {
            var products = _context.FoodItem
              .Where(m => string.IsNullOrEmpty(categoryId) || m.TypeId == categoryId)
              .Select(m => new FoodItemViewModel
              {
                
                  Name = m.Name,
                  Category = m.Type.Name,
                  Description = m.Description,
                  Price = m.Price,
                  Slug = m.Slug,
                  ReleaseDate = m.ReleaseDate,
                  Id = m.Id,
                
                  ImageUrl = m.ImageUrl,
              }).ToList();
            
            return View(products);
        }

        // GET: FoodItems/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (id == null || _context.FoodItem == null)
            {
                return NotFound();
            }

            var foodItem = await _context.FoodItem
                .Include(f => f.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }

        // GET: FoodItems/Create
        public IActionResult Create()
        {
            ViewData["TypeId"] = new SelectList(_context.FoodType, "Id", "Name");
            return View();
        }

        // POST: FoodItems/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Slug,Name,Description,Price,Uploads,ReleaseDate,TypeId")] FoodItem model)
        {
            if (ModelState.IsValid)
            {
                if (model.Uploads != null && model.Uploads.Length > 0)
                {
                    string basePath = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
                    string appPath = Path.Combine("images", "FoodType");
                    string fileName = Path.GetRandomFileName().Replace(".", "") + Path.GetExtension(model.Uploads.FileName);
                    string directryPath = Path.Combine(basePath, appPath);
                    Directory.CreateDirectory(directryPath);

                    using var stream = new FileStream(Path.Combine(directryPath, fileName), FileMode.Create);
                    model.Uploads.CopyTo(stream);
                    model.ImageUrl = Path.Combine(appPath, fileName).Replace("\\", "/");
                }
                _context.Add(model);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["TypeId"] = new SelectList(_context.FoodType, "Id", "Name", model.TypeId);
            return View(model);
        }

        // GET: FoodItems/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.FoodItem == null)
            {
                return NotFound();
            }

            var foodItem = await _context.FoodItem.FindAsync(id);
            if (foodItem == null)
            {
                return NotFound();
            }
            ViewData["TypeId"] = new SelectList(_context.FoodType, "Id", "Name", foodItem.TypeId);
            return View(foodItem);
        }

        // POST: FoodItems/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Slug,Name,Description,Price,ReleaseDate,TypeId")] FoodItem foodItem)
        {
            if (id != foodItem.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(foodItem);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FoodItemExists(foodItem.Id))
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
            ViewData["TypeId"] = new SelectList(_context.FoodType, "Id", "Name", foodItem.TypeId);
            return View(foodItem);
        }

        // GET: FoodItems/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.FoodItem == null)
            {
                return NotFound();
            }

            var foodItem = await _context.FoodItem
                .Include(f => f.Type)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (foodItem == null)
            {
                return NotFound();
            }

            return View(foodItem);
        }

        // POST: FoodItems/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.FoodItem == null)
            {
                return Problem("Entity set 'AppDbContext.FoodItem'  is null.");
            }
            var foodItem = await _context.FoodItem.FindAsync(id);
            if (foodItem != null)
            {
                _context.FoodItem.Remove(foodItem);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool FoodItemExists(string id)
        {
          return _context.FoodItem.Any(e => e.Id == id);
        }
    }
}

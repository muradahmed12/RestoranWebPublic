using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using RestoranWeb.Data;
using RestoranWeb.Handlers;
using RestoranWeb.Models;

namespace RestoranWeb.Controllers
{
    public class AppUsersController : Controller
    {
        private readonly AppDbContext _context;

        public AppUsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: AppUsers
        public async Task<IActionResult> Index()
        {
            var appDbContext = _context.AppUser.Include(a => a.AppRoles);
            return View(await appDbContext.ToListAsync());
        }



        // GET: AppUsers/Create
        public IActionResult Create()
        {
            //ViewData["AppRoleId"] = new SelectList(_context.AppRole, "Id", "Id");
            return View();
        }

        // POST: AppUsers/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        //[ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email,PhoneNumber,Password,ConfirmPassword,AppRoleId")] AppUser appUser)
        {
            if (ModelState.IsValid)
            {
                appUser.HashPassword =(appUser.Id + appUser.Password).Encrypt();
                _context.Add(appUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            return View(appUser);
        }
        [Authorized]
        public IActionResult ChangePassword()
        {
         
            return View();
        }
        [HttpPost]
        [Authorized]
        public IActionResult ChangePassword(AppUser model)
        {
            if (model.Password == model.ConfirmPassword)
            {
                var userId = _context.GetLoggedInUser().Id;
                var user = _context.AppUser.Where(m => m.Id ==userId ).FirstOrDefault();
                user.HashPassword = (userId + model.Password).Encrypt();
                _context.SaveChanges();
                _context.httpContextAccessor.HttpContext.Response.Cookies.Delete(Global.LoginCookie, new CookieOptions
                {
                    IsEssential = true
                });
                return RedirectToAction("Index", "Login");
            }
            ModelState.AddModelError(" ", "Both Passwords not matched");
            return View();
        }
        // GET: AppUsers/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.AppUser == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUser.FindAsync(id);
            if (appUser == null)
            {
                return NotFound();
            }
         
            return View(appUser);
        }

        // POST: AppUsers/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Name,Email,PhoneNumber,Password,AppRoleId")] AppUser appUser)
        {
            if (id != appUser.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(appUser);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AppUserExists(appUser.Id))
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
          
            return View(appUser);
        }

        // GET: AppUsers/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.AppUser == null)
            {
                return NotFound();
            }

            var appUser = await _context.AppUser
                .Include(a => a.AppRoles)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (appUser == null)
            {
                return NotFound();
            }

            return View(appUser);
        }

        // POST: AppUsers/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.AppUser == null)
            {
                return Problem("Entity set 'AppDbContext.User'  is null.");
            }
            var appUser = await _context.AppUser.FindAsync(id);
            if (appUser != null)
            {
                _context.AppUser.Remove(appUser);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AppUserExists(string id)
        {
          return _context.AppUser.Any(e => e.Id == id);
        }

    }
}

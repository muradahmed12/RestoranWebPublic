using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RestoranWeb.Data;
using RestoranWeb.Handlers;
using RestoranWeb.Models;

namespace RestoranWeb.Controllers
{
    public class LoginController : Controller
    {
        private readonly AppDbContext _context;

        public LoginController(AppDbContext context)
        {
            _context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
     
        [HttpPost]
        public IActionResult Index(LoginViewModel model)
        {
           var user = _context.AppUser.Where(m => m.Email == model.Email).FirstOrDefault();
            if(user == null)
            { 
                ModelState.AddModelError("Email", "Email not exists");
                return View(model);
            }

            if((user.Id + model.Password).Encrypt() == user.HashPassword)
            {
                var userAgent = _context.httpContextAccessor.HttpContext.Request.Headers["User-Agent"].ToString();
                // HttpContext.Session.SetString(Global.LoginSession, user.Id);
                LoginHistory LoginHistory = new()
                {
                    ClientInfo = userAgent,
                    IPAddress = _context.httpContextAccessor.HttpContext.Connection.RemoteIpAddress.ToString(),
                    UserId = user.Id,
                    ValidTill = model.RememberMe ? DateTime.Now.AddDays(7) : DateTime.Now.AddMinutes(20)


                };
                _context.Add(LoginHistory);
                _context.SaveChanges();
                HttpContext.Response.Cookies.Append(Global.LoginCookie, LoginHistory.Token , new CookieOptions {
                    IsEssential = true,
                    Expires = LoginHistory.ValidTill,
                    //HttpOnly = true
                });
                return RedirectToAction("Index", "Home");
            }
            ModelState.AddModelError("Password", "Invalid Password");
            return View(model);
        }
        public IActionResult Logout()
        {
            HttpContext.Response.Cookies.Delete(Global.LoginCookie);
            return RedirectToAction("Index", "Home  ");

        }

    }
}

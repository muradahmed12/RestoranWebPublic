using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RestoranWeb.Models;

namespace RestoranWeb.Data
{
    public class AppDbContext : DbContext
    {
        //private  IHttpContextAccessor _contextAccessor;
        public IHttpContextAccessor httpContextAccessor { get; }
        private  AppUserViewModel LoggedInUser { get; set; }
        public AppDbContext (DbContextOptions<AppDbContext> options, IHttpContextAccessor contextAccessor)
            : base(options)
        {
            httpContextAccessor = contextAccessor;
           

        }
        public DbSet<RestoranWeb.Models.LoginHistory> LoginHistory { get; set; }
        public DbSet<RestoranWeb.Models.AppUser> AppUser { get; set; }
        public DbSet<RestoranWeb.Models.AppRole> AppRole { get; set; }
        public DbSet<RestoranWeb.Models.CartItem> CartItem { get; set; }
        public DbSet<RestoranWeb.Models.FoodItem> FoodItem { get; set; }
        public DbSet<RestoranWeb.Models.FoodType> FoodType { get; set; }
        public DbSet<RestoranWeb.Models.Order> Order { get; set; }
        public DbSet<RestoranWeb.Models.OrderDetail> OrderDetail { get; set; }
        public DbSet<RestoranWeb.Models.ShoppingCart> ShoppingCart { get; set; }
       
       
       
       
       
        public AppUserViewModel GetLoggedInUser(/*IHttpContextAccessor httpContextAccessor = null*/)
        {
            if (LoggedInUser != null) return LoggedInUser;
                  //  _contextAccessor ?? = httpContextAccessor;
            //var userId = _contextAccessor.HttpContext.Session.GetString(Global.LoginSession);
           var token = httpContextAccessor.HttpContext.Request.Cookies[Global.LoginCookie]?.ToString();
            if (string.IsNullOrEmpty(token))
            {
                 token = httpContextAccessor.HttpContext.Request.Headers[Global.LoginCookie].ToString();

            }
            if (!string.IsNullOrEmpty(token))
            {
                var loginHistory = LoginHistory.Where(m => m.Token == token).FirstOrDefault();
                if(loginHistory == null || loginHistory.ValidTill < DateTime.Now)
                {
                   httpContextAccessor.HttpContext.Response.Cookies.Delete(Global.LoginCookie, new CookieOptions { IsEssential = true });
                    return null;
                }
            var user = LoginHistory.Where(m => m.Token == token).Select(n => n.User)
                  .Select(n => new AppUserViewModel
                  {
                      Id = n.Id,
                      Name = n.Name,
                      Email = n.Email,
                      DbEntryTime = n.DbEntryTime,
                     // AppRole = n.AppRoles.Select(c => c.Name).ToList()
                  }).FirstOrDefault();
            LoggedInUser = user;
                return LoggedInUser;
            }
            return null;
        }
        // protected override void OnModelCreating(ModelBuilder modelBuilder)
        //{
            
        //    modelBuilder.Entity<FoodType>()
        //        .HasMany(m => m.ImageList)
        //        .WithOne(m => m.FoodType)
        //        .OnDelete(DeleteBehavior.Cascade); // Set the delete behavior to Cascade
        //    base.OnModelCreating(modelBuilder);
        //   // modelBuilder.Entity<FoodItem>()
        //   //.HasMany(ds => ds.FoodItemImages)
        //   ////.WithOne(d => d.FoodItem)
        //   //.HasForeignKey(ds => ds.TypeId);
        //}

    }   
}

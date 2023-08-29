using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using RestoranWeb.Data;
namespace RestoranWeb
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();
            builder.Services.AddTransient<IHttpContextAccessor, HttpContextAccessor>();
            string connectionString = builder.Configuration.GetConnectionString("SqlConnection");

            //builder.Services.AddHttpContextAccessor();
            builder.Services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(connectionString ?? throw new InvalidOperationException("Connection string 'SqlConnection' not found.")));
            // Add services to the container.
            builder.Services.AddDistributedSqlServerCache(m =>
            {
                m.ConnectionString = connectionString;
                m.SchemaName = "dbo";
                m.TableName = "SessionData";
            });
            builder.Services.AddSession(m =>
            {
                m.IdleTimeout = TimeSpan.FromMinutes(30);
            });
         
        //singleton 
        //scope 
        //transiet
            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }
            using var scope = app.Services.CreateScope();
           var _context =  scope.ServiceProvider.GetRequiredService<AppDbContext>();
            _context.Database.Migrate();
            app.UseSession();
            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
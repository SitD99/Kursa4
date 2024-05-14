using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApp.Models;
using Microsoft.Extensions.Options;
using System.Security.Claims;

namespace WebApp
{
    public class Startup
    {
        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {

            services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.Cookie.Name = "MYCookie"; // Установите имя куки
                    options.Cookie.HttpOnly = true;
                    options.ExpireTimeSpan = TimeSpan.FromMinutes(30); // Установите время жизни куки
                    options.LoginPath = "/User/Login"; // Установите путь для перенаправления на страницу входа
                    options.AccessDeniedPath = "/User/AccessDenied"; // Установите путь для перенаправления на страницу доступа запрещен
                    options.SlidingExpiration = true; // Реализация скользящего времени сеанса
                });
            services.AddDbContext<AppDbContext>(options =>
                options.UseSqlServer(Configuration.GetConnectionString("DefaultConnection")));

            services.AddControllersWithViews().AddNewtonsoftJson();
            services.AddRazorPages().AddRazorRuntimeCompilation();
        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();
            app.UseExceptionHandler("/Home/Error");
            app.UseStatusCodePagesWithReExecute("/Home/Error/{0}");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "login",
                    pattern: "login",
                    defaults: new { controller = "User", action = "Login" });

                endpoints.MapControllerRoute(
                    name: "register",
                    pattern: "register",
                    defaults: new { controller = "User", action = "Register" });

                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                /*endpoints.MapControllerRoute(
                        name: "CardSetCreate",
                        pattern: "CardSet/Create",
                        defaults: new { controller = "CardSet", action = "Create" });*/

                /*endpoints.MapControllerRoute(
                    name: "cardset_details",
                    pattern: "CardSet/Details/{id}",
                    defaults: new { controller = "CardSet", action = "Details" });*/

                /*endpoints.MapControllerRoute(
                    name: "cardset_default",
                    pattern: "CardSet/{action=Index}",
                    defaults: new { controller = "CardSet" });*/

                /*endpoints.MapControllerRoute(
                    name: "card_details",
                    pattern: "Card/Details/{id}",
                    defaults: new { controller = "Card", action = "Details" });

                endpoints.MapControllerRoute(
                    name: "card_create",
                    pattern: "Card/Create",
                    defaults: new { controller = "Card", action = "Create" });

                endpoints.MapControllerRoute(
                    name: "card_edit",
                    pattern: "Card/Edit/{id}",
                    defaults: new { controller = "Card", action = "Edit" });

                endpoints.MapControllerRoute(
                    name: "card_delete",
                    pattern: "Card/Delete/{id}",
                    defaults: new { controller = "Card", action = "Delete" });

                endpoints.MapControllerRoute(
                    name: "card_default",
                    pattern: "Card/{action=Index}",
                    defaults: new { controller = "Card" });*/




            });
        }
    }
}
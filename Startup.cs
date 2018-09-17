using GroceryStore2.Models;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace GroceryStore2 {
  public class Startup {
    public Startup (IConfiguration configuration) {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    public void ConfigureServices (IServiceCollection services) {
      services.AddDbContext<AppDbContext> (options =>
        options.UseNpgsql (Configuration.GetConnectionString ("DefaultConnection")));

      services.AddIdentity<AppUser, IdentityRole> ()
        .AddEntityFrameworkStores<AppDbContext> ()
        .AddDefaultTokenProviders ();

      services.AddTransient<ICategoryRepository, CategoryRepository> ();
      services.AddTransient<IProductRepository, ProductRepository> ();
      services.AddSingleton<IHttpContextAccessor, HttpContextAccessor> ();
      services.AddScoped<ShoppingCart> (sp => ShoppingCart.GetCart (sp));
      services.AddTransient<IOrderRepository, OrderRespository> ();

      services.AddMvc ().SetCompatibilityVersion (CompatibilityVersion.Version_2_1);

      services.AddMemoryCache ();
      services.AddSession ();

      services.AddAuthentication (options => {
          options.DefaultScheme = CookieAuthenticationDefaults.AuthenticationScheme;
        })
        .AddCookie ();
    }

    public void Configure (IApplicationBuilder app, IHostingEnvironment env) {
      if (env.IsDevelopment ()) {
        app.UseDeveloperExceptionPage ();
        app.UseDatabaseErrorPage ();
      } else {
        app.UseExceptionHandler ("/Home/Error");
        app.UseHsts ();
      }

      app.UsePathBase("/gs");
      app.UseSession ();
      app.UseHttpsRedirection ();
      app.UseStatusCodePages ();
      app.UseStaticFiles ();
      // app.UseCookiePolicy();

      app.UseForwardedHeaders (new ForwardedHeadersOptions {
        ForwardedHeaders = ForwardedHeaders.XForwardedFor | ForwardedHeaders.XForwardedProto
      });

      app.UseAuthentication ();
      app.UseMvc (routes => {
        routes.MapRoute (
          name: "categoryFilter",
          template: "Product/{action}/{category?}",
          defaults : new { Controller = "Product", action = "List" });

        routes.MapRoute (
          name: "default",
          template: "{controller=Home}/{action=Index}/{id?}");
      });
    }
  }
}

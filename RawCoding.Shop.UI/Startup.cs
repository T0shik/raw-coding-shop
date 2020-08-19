using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawCoding.Shop.Database;
using Stripe;
using System;
using Microsoft.Extensions.Hosting;

namespace RawCoding.Shop.UI
{
    public class Startup
    {
        private readonly IConfiguration _config;
        private readonly IWebHostEnvironment _env;

        public Startup(
            IConfiguration configuration,
            IWebHostEnvironment env)
        {
            _config = configuration;
            _env = env;
        }


        public void ConfigureServices(IServiceCollection services)
        {
            services.Configure<CookiePolicyOptions>(options =>
            {
                options.CheckConsentNeeded = context => true;
                options.MinimumSameSitePolicy = SameSiteMode.None;
            });

            // services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(_config["DefaultConnection"]));

            services.AddDbContext<ApplicationDbContext>(options => options.UseInMemoryDatabase("Dev"));
            // todo configure for prod
            services.AddIdentity<IdentityUser, IdentityRole>(options =>
                {
                    options.Password.RequireDigit = false;
                    options.Password.RequiredLength = 6;
                    options.Password.RequireNonAlphanumeric = false;
                    options.Password.RequireUppercase = false;
                })
                .AddEntityFrameworkStores<ApplicationDbContext>();

            services.ConfigureApplicationCookie(options => { options.LoginPath = "/Admin/Login"; });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
            });

            StripeConfiguration.ApiKey = _config.GetSection("Stripe")["SecretKey"];

            services.AddApplicationServices();

            services.AddControllers()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin", "Admin");
                options.Conventions.AllowAnonymousToPage("/Admin/Login");
            });
        }

        public void Configure(IApplicationBuilder app)
        {
            if (_env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseCookiePolicy();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
                endpoints.MapRazorPages();
            });
        }
    }
}
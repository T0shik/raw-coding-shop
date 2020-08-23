using System;
using System.Security.Claims;
using FluentValidation.AspNetCore;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using RawCoding.Shop.Database;
using Stripe;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Routing;
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

            services.AddAuthentication()
                .AddCookie(ShopConstants.Schemas.Guest,
                    config =>
                    {
                        config.Cookie.Name = ShopConstants.Schemas.Guest;
                        config.ExpireTimeSpan = TimeSpan.FromDays(365);
                    });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(ShopConstants.Policies.Customer, x =>
                {
                    x.AddAuthenticationSchemes(ShopConstants.Schemas.Guest);
                    x.RequireAuthenticatedUser();
                });
            });

            services.AddAuthorization(options =>
            {
                options.AddPolicy("Admin", policy => policy.RequireClaim("Role", "Admin"));
            });

            StripeConfiguration.ApiKey = _config.GetSection("Stripe")["SecretKey"];

            services.AddApplicationServices();

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = true;
            });

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

            app.UseStatusCodePages(context =>
            {
                var pathBase = context.HttpContext.Request.PathBase;
                var endpoint = StatusCodeEndpoint(context.HttpContext.Response.StatusCode);
                context.HttpContext.Response.Redirect(pathBase + endpoint);
                return Task.CompletedTask;
            });

            //todo move the user generation to authentication policy
            app.Use(async (context, next) =>
            {
                if (!context.User.Identity.IsAuthenticated)
                {
                    var identity = new ClaimsIdentity(new[]
                    {
                        new Claim(ClaimTypes.Role, ShopConstants.Roles.Guest),
                        new Claim(ClaimTypes.NameIdentifier, Guid.NewGuid().ToString()),
                    }, ShopConstants.Schemas.Guest);

                    var claimsPrinciple = new ClaimsPrincipal(identity);

                    await context.SignInAsync(
                        ShopConstants.Schemas.Guest,
                        claimsPrinciple,
                        new AuthenticationProperties
                        {
                            IsPersistent = true
                        });
                }

                await next();
            });

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                    .RequireAuthorization(ShopConstants.Policies.Customer);
                endpoints.MapRazorPages();
            });
        }

        private static string StatusCodeEndpoint(int code) =>
            code switch
            {
                StatusCodes.Status404NotFound => "/not-found",
                _ => "/not-found"
            };
    }
}
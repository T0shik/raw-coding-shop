using System;
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
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Hosting;
using RawCoding.S3;
using RawCoding.Shop.UI.Workers.Email;

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

            services.Configure<StripeSettings>(_config.GetSection(nameof(StripeSettings)));

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
                        config.LoginPath = "/api/cart/guest-auth";
                    });

            services.AddAuthorization(config =>
            {
                config.AddPolicy(ShopConstants.Policies.Customer, policy => policy
                    .AddAuthenticationSchemes(ShopConstants.Schemas.Guest)
                    .AddRequirements(new ShopRequirement())
                    .RequireAuthenticatedUser());

                config.AddPolicy(ShopConstants.Policies.Admin, policy => policy
                    .AddAuthenticationSchemes(IdentityConstants.ApplicationScheme)
                    .RequireClaim(ShopConstants.Claims.Role, ShopConstants.Roles.Admin)
                    .RequireAuthenticatedUser());
            });

            StripeConfiguration.ApiKey = _config.GetSection("Stripe")["SecretKey"];

            services.Configure<RouteOptions>(options =>
            {
                options.LowercaseUrls = true;
                options.LowercaseQueryStrings = false;
            });

            services.AddControllers()
                .AddFluentValidation(x => x.RegisterValidatorsFromAssembly(typeof(Startup).Assembly));

            services.AddRazorPages(options =>
            {
                options.Conventions.AuthorizeFolder("/Admin", ShopConstants.Policies.Admin);
                options.Conventions.AllowAnonymousToPage("/Admin/Login");
            });

            services.AddApplicationServices()
                .AddEmailService(_config)
                .AddRawCodingS3Client(() => _config.GetSection(nameof(S3StorageSettings)).Get<S3StorageSettings>())
                .AddScoped<PaymentIntentService>();

            services.AddHttpClient();
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

            app.UseAuthentication();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute()
                    .RequireAuthorization(ShopConstants.Policies.Customer);

                endpoints.MapRazorPages()
                    .RequireAuthorization(ShopConstants.Policies.Customer);
            });
        }

        private static string StatusCodeEndpoint(int code) =>
            code switch
            {
                StatusCodes.Status404NotFound => "/not-found",
                _ => "/",
            };

        public class ShopRequirement : AuthorizationHandler<ShopRequirement>, IAuthorizationRequirement
        {
            protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ShopRequirement requirement)
            {
                if (context.User != null)
                {
                    if (context.User.HasClaim(ShopConstants.Claims.Role, ShopConstants.Roles.Admin)
                        || context.User.HasClaim(ShopConstants.Claims.Role, ShopConstants.Roles.Guest))
                    {
                        context.Succeed(requirement);
                    }
                }

                return Task.CompletedTask;
            }
        }
    }
}
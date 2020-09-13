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

            services.AddAuthentication(options =>
                {
                    // options.DefaultChallengeScheme = ShopConstants.Schemas.Guest;
                })
                .AddCookie(ShopConstants.Schemas.Guest,
                    config =>
                    {
                        config.Cookie.Name = ShopConstants.Schemas.Guest;
                        config.ExpireTimeSpan = TimeSpan.FromDays(365);
                        config.LoginPath = "/api/cart/guest-auth";
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
                options.LowercaseQueryStrings = false;
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
                _ => "/not-found"
            };

        // public class CustomerSchemeOptions : AuthenticationSchemeOptions
        // {
        // }
        //
        // public class CustomerAuthenticationHandler : AuthenticationHandler<CustomerSchemeOptions>
        // {
        //     public CustomerAuthenticationHandler(IOptionsMonitor<CustomerSchemeOptions> options, ILoggerFactory logger,
        //         UrlEncoder encoder, ISystemClock clock) : base(options, logger, encoder, clock)
        //     {
        //     }
        //
        //     protected override Task<AuthenticateResult> HandleAuthenticateAsync()
        //     {
        //         return Task.CompletedTask(AuthenticateResult.Success(new AuthenticationTicket()));
        //     }
        // }
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using RawCoding.Shop.Database;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.UI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var host = CreateWebHostBuilder(args).Build();

            try
            {
                using (var scope = host.Services.CreateScope())
                {
                    var env = scope.ServiceProvider.GetRequiredService<IWebHostEnvironment>();
                    var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                    var userManger = scope.ServiceProvider.GetRequiredService<UserManager<IdentityUser>>();

                    if (env.IsDevelopment())
                    {
                        context.Add(new Product
                        {
                            Name = "Test",
                            Description = "Test Product",
                            Series = "Original",
                            Slug = "original-test",
                            StockDescription = "Size",
                            Stock = new List<Stock>
                            {
                                new Stock {Description = "Small", Value = 1000, Qty = 100,},
                                new Stock {Description = "Medium", Value = 1000, Qty = 100,},
                                new Stock {Description = "Large", Value = 1000, Qty = 1,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "book.jpg"},
                                new Image {Index = 1, Path = "book3.jpg"},
                                new Image {Index = 2, Path = "pen.jpg"},
                                new Image {Index = 3, Path = "shirt.jpg"},
                            }
                        });

                        context.Add(new Product
                        {
                            Name = "Out Of Stock",
                            Description = "Test Out Of Stock Product",
                            Series = "Original",
                            Slug = "original-out-of-stock",
                            StockDescription = "Size",
                            Stock = new List<Stock>
                            {
                                new Stock {Description = "Small", Value = 1000, Qty = 0,},
                                new Stock {Description = "Medium", Value = 1000, Qty = 0,},
                                new Stock {Description = "Large", Value = 1000, Qty = 0,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "book.jpg"},
                                new Image {Index = 1, Path = "pen.jpg"},
                                new Image {Index = 2, Path = "shirt.jpg"},
                            }
                        });


                        context.Add(new Product
                        {
                            Name = "Limited",
                            Description = "Test Limited Product",
                            Series = "Original",
                            Slug = "original-limited",
                            StockDescription = "Size",
                            Stock = new List<Stock>
                            {
                                new Stock {Description = "Small", Value = 1000, Qty = 10,},
                                new Stock {Description = "Medium", Value = 1000, Qty = 0,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "book.jpg"},
                                new Image {Index = 1, Path = "shirt.jpg"},
                            }
                        });

                        context.Add(new Product
                        {
                            Name = "Test2",
                            Description = "Test Product 22",

                            Series = "Original",
                            Slug = "original-test2",
                            Stock = new List<Stock>
                            {
                                new Stock {Value = 2220, Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "pen.jpg"},
                                new Image {Index = 1, Path = "shirt3.jpg"},
                            }
                        });

                        context.Add(new Product
                        {
                            Name = "Test 33",
                            Description = "Test Product 313",
                            Series = "Original",
                            Slug = "original-test-33",
                            Stock = new List<Stock>
                            {
                                new Stock {Value = 333, Description = "Default", Qty = 100,},
                            },
                            Images = new List<Image>
                            {
                                new Image {Index = 0, Path = "shirt.jpg"},
                            }
                        });


                        context.SaveChanges();
                    }

                    if (!context.Users.Any())
                    {
                        var adminUser = new IdentityUser()
                        {
                            UserName = "Admin"
                        };

                        userManger.CreateAsync(adminUser, "password").GetAwaiter().GetResult();

                        var adminClaim = new Claim(ClaimTypes.Role, ShopConstants.Roles.Admin);

                        userManger.AddClaimAsync(adminUser, adminClaim).GetAwaiter().GetResult();
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }

            host.Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
}
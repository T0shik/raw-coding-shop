using System.Linq;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using RawCoding.Shop.Application;
using RawCoding.Shop.Database;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.UI
{
    public static class ServiceRegister
    {
        public static IServiceCollection AddApplicationServices(
            this IServiceCollection @this)
        {
            var serviceType = typeof(Service);
            var definedTypes = serviceType.Assembly.DefinedTypes;

            var services = definedTypes
                .Where(x => x.GetTypeInfo().GetCustomAttribute<Service>() != null);

            foreach (var service in services)
            {
                @this.AddTransient(service);
            }

            @this.AddTransient<ICartManager, CartManager>();
            @this.AddTransient<IStockManager, StockManager>();
            @this.AddTransient<IProductManager, ProductManager>();
            @this.AddTransient<IOrderManager, OrderManager>();

            return @this;
        }
    }
}

using Microsoft.EntityFrameworkCore;
using RawCoding.Shop.Domain.Enums;
using RawCoding.Shop.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Interfaces;

namespace RawCoding.Shop.Database
{
    public class OrderManager : IOrderManager
    {
        private readonly ApplicationDbContext _ctx;

        public OrderManager(ApplicationDbContext ctx)
        {
            _ctx = ctx;
        }

        public bool OrderReferenceExists(string reference)
        {
            return _ctx.Orders.Any(x => x.Id == reference);
        }

        public IEnumerable<Order> OrdersForAdminPanel(OrderStatus status)
        {
            return _ctx.Orders
                .Where(x => x.Status == status)
                .ToList();
        }

        public Order GetOrderById(string id)
        {
            return _ctx.Orders
                .AsNoTracking()
                .Include(x => x.Cart)
                .ThenInclude(x => x.Products)
                .ThenInclude(x => x.Stock)
                .ThenInclude(x => x.Product)
                .ThenInclude(x => x.Images)
                .FirstOrDefault(order => order.Id == id);
        }

        public Task<int> CreateOrder(Order order)
        {
            _ctx.Orders.Add(order);

            return _ctx.SaveChangesAsync();
        }

        public Task<int> AdvanceOrder(string id)
        {
            _ctx.Orders.FirstOrDefault(x => x.Id == id).Status++;

            return _ctx.SaveChangesAsync();
        }
    }
}
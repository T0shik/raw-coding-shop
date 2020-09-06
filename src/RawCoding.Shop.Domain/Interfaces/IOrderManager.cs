using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;
using RawCoding.Shop.Domain.Enums;
using RawCoding.Shop.Domain.Models;

namespace RawCoding.Shop.Domain.Interfaces
{
    public interface IOrderManager
    {
        bool OrderReferenceExists(string reference);

        IEnumerable<TResult> GetOrdersByStatus<TResult>(OrderStatus status, Expression<Func<Order, TResult>> selector);
        Order GetOrderById(string id);

        Task<int> CreateOrder(Order order);
        Task<int> AdvanceOrder(string id);
    }
}
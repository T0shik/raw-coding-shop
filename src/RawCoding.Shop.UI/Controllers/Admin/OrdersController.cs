﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using RawCoding.Shop.Application.Admin.Orders;
using RawCoding.Shop.Application.Orders;
using RawCoding.Shop.Domain.Enums;

namespace RawCoding.Shop.UI.Controllers.Admin
{
    public class OrdersController : AdminBaseController
    {
        [HttpGet]
        public IActionResult GetOrders(OrderStatus status, [FromServices] GetOrders getOrders) =>
            Ok(getOrders.ForStatus(status));

        [HttpGet("{id}")]
        public IActionResult GetOrder(string id, [FromServices] GetOrder getOrder) =>
            Ok(getOrder.ForAdminById(id));

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrder(string id, [FromServices] UpdateOrder updateOrder)
        {
            var success = await updateOrder.DoAsync(id) > 0;
            if (success)
            {
                return Ok();
            }

            return BadRequest();
        }
    }
}
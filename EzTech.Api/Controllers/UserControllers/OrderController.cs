using AutoMapper;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.ApiModels.UserApiModels.OrderApiModels;
using EzTech.Data.DtoModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.UserControllers;

/// <summary>
/// Source: https://stripe.com/docs/api
/// Right now the process is done in frontend, but we will need to do it in backend later
/// if there is time
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class OrderController : BaseUserController
{
    [HttpPost]
    [Route("{orderId:int}/change-status/{status}")]
    public async Task<IActionResult> ChangeOrderStatus(int orderId, string status)
    {
        var order = await DbContext.Orders.FindAsync(orderId);
        if (order == null)
        {
            return NotFound("Order not found");
        }

        if (order.Status.ToString() == status)
        {
            return BadRequest("Order already has this status");
        }

        var newStatus = Enum.Parse<OrderStatus>(status);

        if (order.Status == newStatus)
        {
            return BadRequest("Order already has this status");
        }

        order.Status = Enum.Parse<OrderStatus>(status);

        await DbContext.SaveChangesAsync();

        return Ok("Order status changed");
    }

    [HttpGet]
    [Route("{orderNumber}")]
    public async Task<ActionResult> GetOrder(string orderNumber)
    {
        var order = await DbContext.Orders
            .OrderBy(o => orderNumber)
            .Include(o => o.Items)
            .FirstOrDefaultAsync(order => order.OrderNumber.ToString() == orderNumber);
        if (order == null) return NotFound("Order not found");

        var response = Mapper.Map<OrderDto>(order);
        return Ok(response);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrder(CreateOrderRequest request)
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .Include(u => u.Orders)
            .ThenInclude(order => order.Items)
            .Include(u => u.Cart)
            .ThenInclude(cart => cart.CartItems)
            .FirstOrDefaultAsync(user => user.Id == UserPrincipal.Id);
        if (user == null) return Unauthorized("User not found");

        // Creating order items
        var orderItems = request.Products.Select(
            product => new OrderItem
            {
                ProductId = product.ProductId,
                ProductName = product.ProductName,
                ProductDescription = product.ProductDescription,
                Price = product.Price,
                Quantity = product.Quantity
            }).ToList();

        // Creating order
        var order = new Order
        {
            Total = request.Total,
            Items = orderItems,
            CreatedAt = DateTime.Parse(request.Date)
        };

        user.Orders.Add(order);
        user.Cart.ClearCart();
        foreach (var orderItem in orderItems)
        {
            var product = await DbContext.Products
                .OrderBy(u => u.Id)
                .FirstOrDefaultAsync(product => product.Id == orderItem.ProductId);
            product!.Sell(orderItem.Quantity);
        }

        await DbContext.SaveChangesAsync();
        var response = Mapper.Map<OrderDto>(order);
        await EmailManager.SendEmail(user.Email, "Order created",
            $"Your order has been created\nLink: http://localhost:3000/order/{order.OrderNumber}");
        return Ok(response);
    }

    public OrderController(IMapper mapper, EzTechDbContext dbContext, IEmailManager emailManager) : base(mapper,
        dbContext, emailManager)
    {
    }
}
using AutoMapper;
using EzTech.Data;
using EzTech.Data.ApiModels.AdminApiModels.DashboardApiModels;
using EzTech.Data.DtoModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class DashboardController : BaseAdminController
{
    [HttpGet]
    public async Task<IActionResult> GetDashboard()
    {
        var orders = await DbContext.Orders
            .Include(x => x.Items)
            .ToListAsync();

        var completedOrders = orders.Where(x => x.Status == OrderStatus.Completed).Select(order =>
        {
            var newOrder = new DashboardOrder
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                CompletedAt = order.CompletedAt,
                IdListOfItems = order.Items.Select(x => x.ProductId).ToList(),
                Total = order.Total,
            };
            return newOrder;
        }).ToList();
        var activeOrders = orders.Where(x => x.Status != OrderStatus.Completed).Select(order =>
        {
            var newOrder = new DashboardOrder
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Status = order.Status.ToString(),
                CreatedAt = order.CreatedAt,
                CompletedAt = order.CompletedAt,
                IdListOfItems = order.Items.Select(x => x.ProductId).ToList(),
                Total = order.Total,
            };
            return newOrder;
        }).ToList();
        
        var incomeToday = completedOrders.Where(x => x.CreatedAt.Date == DateTime.Now.Date).Sum(x => x.Total);
        var incomeThisMonth = completedOrders.Where(x => x.CreatedAt.Month == DateTime.Now.Month).Sum(x => x.Total);
        var incomeThisYear = completedOrders.Where(x => x.CreatedAt.Year == DateTime.Now.Year).Sum(x => x.Total);
        var incomeAllTime = completedOrders.Sum(x => x.Total);

        var response = new GetDashboardResponse
        {
            // Count of Orders
            TotalActiveOrders = activeOrders.Count,
            PendingOrders = orders.Count(x => x.Status == OrderStatus.Pending),
            ProcessingOrders = orders.Count(x => x.Status == OrderStatus.Processing),
            ShippedOrders = orders.Count(x => x.Status == OrderStatus.Shipped),
            // Created Orders
            CreatedOrdersToday = orders.Count(x => x.CreatedAt.Date == DateTime.Now.Date),
            CreatedOrdersThisMonth = orders.Count(x => x.CreatedAt.Month == DateTime.Now.Month),
            CreatedOrdersThisYear = orders.Count(x => x.CreatedAt.Year == DateTime.Now.Year),
            // Completed Orders
            CompletedOrdersAllTime = completedOrders,
            CompletedOrdersToday = completedOrders.Where(x => x.CompletedAt!.Value.Date == DateTime.Now.Date).ToList(),
            CompletedOrdersThisMonth =
                completedOrders.Where(x => x.CompletedAt!.Value.Month == DateTime.Now.Month).ToList(),
            CompletedOrdersThisYear =
                completedOrders.Where(x => x.CompletedAt!.Value.Year == DateTime.Now.Year).ToList(),
            IncomeToday = incomeToday,
            IncomeThisMonth = incomeThisMonth,
            IncomeThisYear = incomeThisYear,
            IncomeAllTime = incomeAllTime,
            // Latest Orders
            LatestFiveActiveOrders = Mapper.Map<List<OrderDto>>(orders.OrderByDescending(x => x.CreatedAt).Take(5)),
            OldestFiveActiveOrders = Mapper.Map<List<OrderDto>>(orders.OrderBy(x => x.CreatedAt).Take(5)),
        };
        return Ok(response);
    }

    public DashboardController(IMapper mapper, EzTechDbContext dbContext) : base(mapper, dbContext)
    {
    }
}
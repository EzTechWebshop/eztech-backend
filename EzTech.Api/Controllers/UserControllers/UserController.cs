using AutoMapper;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.ApiModels.UserApiModels;
using EzTech.Data.ApiModels.UserApiModels.OrderApiModels;
using EzTech.Data.ApiModels.UserApiModels.RatingApiModels;
using EzTech.Data.DtoModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.UserControllers;

[ApiController]
[Route("api/[controller]")]
public class UserController : BaseUserController
{
    
    [HttpPost]
    [Route("add-rating/{productId:int}")]
    public async Task<IActionResult> AddRating(int productId, [FromBody] AddRatingRequest request)
    {
        var user = await DbContext.Users.FindAsync(UserPrincipal.Id);
        var product = await DbContext.Products.FindAsync(productId);
        var rating = new Rating
        {
            Rate = request.Rate,
            Comment = request.Comment,
            Product = product,
            User = user,
        };
        await DbContext.Ratings.AddAsync(rating);
        await DbContext.SaveChangesAsync();
        return Ok("Rating added");
    }
    
    [HttpGet]
    [Route("get-details")]
    public async Task<IActionResult> GetDetails()
    {
        var user = await DbContext.Users.FindAsync(UserPrincipal.Id);
        var response = Mapper.Map<UserDto>(user);
        return Ok(response);
    }
    
    [HttpGet]
    [Route("get-orders")]
    public async Task<IActionResult> GetOrders(
        [FromQuery] string? status
        )
    {
        var orders = DbContext.Orders
            .Include(x => x.Items)
            .ThenInclude(x => x.Product)
            .Where(x => x.UserId == UserPrincipal.Id)
            .AsQueryable();
        var totalOrders = orders.Count();
        var pendingOrders = orders.Count(x => x.Status == OrderStatus.Pending);
        var processingOrders = orders.Count(x => x.Status == OrderStatus.Processing);
        var completedOrders = orders.Count(x => x.Status == OrderStatus.Completed);
        if (status != null)
        {
            var orderStatus = Enum.Parse<OrderStatus>(status);
            orders = orders.Where(x => x.Status == orderStatus);
        }
        var orderList = await orders.ToListAsync();
        var response = new GetUserOrdersResponse
        {
            Status = status ?? "All",
            TotalOrders = totalOrders,
            PendingOrders = pendingOrders,
            ProcessingOrders = processingOrders,
            CompletedOrders = completedOrders,
            Orders = Mapper.Map<List<OrderDto>>(orderList),
        };
        
        return Ok(response);
    }
    
    [HttpPatch]
    [Route("update-details")]
    public async Task<IActionResult> UpdateDetails([FromBody] UpdateUserDetailsRequest request)
    {
        var user = await DbContext.Users.FindAsync(UserPrincipal.Id);
        if (user == null) return Unauthorized("User not found");
        if (user.PhoneNumber != request.PhoneNumber)
        {
            var phoneNumberExists = await DbContext.Users.AnyAsync(x => x.PhoneNumber == request.PhoneNumber);
            if (phoneNumberExists)
            {
                return Conflict("Phone number already exists");
            }
        }
        if (user.Email != request.Email)
        {
            var emailExists = await DbContext.Users.AnyAsync(x => x.Email == request.Email);
            if (emailExists)
            {
                return Conflict("Email already exists");
            }
        }
        user.UpdateDetails(request);
        await DbContext.SaveChangesAsync();
        var response = Mapper.Map<UserDto>(user);
        return Ok(response);
    }


    public UserController(IMapper mapper, EzTechDbContext dbContext, IEmailManager emailManager) : base(mapper, dbContext, emailManager)
    {
    }
}
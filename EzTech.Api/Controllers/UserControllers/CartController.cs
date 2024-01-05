using AutoMapper;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.ApiModels.UserApiModels.CartApiModels;
using EzTech.Data.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.UserControllers;

[ApiController]
[Route("api/[controller]")]
public class CartController : BaseUserController
{
    [HttpPost]
    [Route("change-quantity")]
    public async Task<ActionResult> ChangeQuantity([FromBody] ChangeQuantityRequest request)
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .Include(u => u.Cart)
            .ThenInclude(cart => cart.CartItems)
            .ThenInclude(cartItem => cartItem.Product)
            .FirstOrDefaultAsync(user => user.Id == UserPrincipal.Id);
        if (user == null) return Unauthorized("User not found");

        user.Cart.ChangeQuantity(request.CartItemId, request.Quantity);
        await DbContext.SaveChangesAsync();

        return Ok("Quantity changed");
    }

    [HttpGet]
    [Route("get-cart")]
    public async Task<IActionResult> GetCart()
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .Include(u => u.Cart)
            .ThenInclude(cart => cart.CartItems)
            .ThenInclude(item => item.Product)
            .ThenInclude(product => product.Images)
            .FirstOrDefaultAsync(user => user.Id == UserPrincipal.Id);
        if (user == null) return Unauthorized("User not found");

        var response = Mapper.Map<CartDto>(user.Cart);

        return Ok(response);
    }

    [HttpPost]
    [Route("add-to-cart/{productId:int}")]
    public async Task<ActionResult> AddToCart(int productId)
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .Include(u => u.Cart)
            .ThenInclude(cart => cart.CartItems)
            .FirstOrDefaultAsync(user => user.Id == UserPrincipal.Id);
        if (user == null) return Unauthorized("User not found");

        var product = await DbContext.Products.FindAsync(productId);
        if (product == null) return NotFound("Product not found");

        user.Cart.AddItem(product);
        await DbContext.SaveChangesAsync();

        return Ok("Added to cart");
    }

    [HttpDelete]
    [Route("remove-from-cart/{cartItemId:int}")]
    public async Task<ActionResult> RemoveFromCart(int cartItemId)
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .Include(u => u.Cart)
            .ThenInclude(cart => cart.CartItems)
            .FirstOrDefaultAsync(user => user.Id == UserPrincipal.Id);
        if (user == null) return Unauthorized("User not found");

        user.Cart.RemoveItem(cartItemId);
        await DbContext.SaveChangesAsync();

        return Ok("Removed from cart");
    }

    public CartController(IMapper mapper, EzTechDbContext dbContext, IEmailManager emailManager) : base(mapper,
        dbContext, emailManager)
    {
    }
}
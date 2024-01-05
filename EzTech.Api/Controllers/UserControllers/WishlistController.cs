using AutoMapper;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.UserControllers;

[ApiController]
[Route("api/[controller]")]
public class WishlistController : BaseUserController
{
    [HttpGet]
    [Route("get-wishlist")]
    public async Task<ActionResult<WishlistDto>> GetWishlist()
    {
        var user = await DbContext.Users
            .OrderBy(x => x.Id)
            .Include(user => user.Wishlist)
            .ThenInclude(wishlist => wishlist.Products)
            .FirstOrDefaultAsync(x => x.Id == UserPrincipal.Id);
        if (user == null) return Unauthorized("User not found");

        var wishlistDto = Mapper.Map<WishlistDto>(user.Wishlist);

        return Ok(wishlistDto);
    }

    [HttpPost]
    [Route("add-item/{productId:int}")]
    public async Task<IActionResult> AddToWishlist(int productId)
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .Include(user => user.Wishlist)
            .ThenInclude(wishlist => wishlist.Products)
            .FirstOrDefaultAsync(x => x.Id == UserPrincipal.Id);
        if (user == null) return Unauthorized("User not found");

        var product = await DbContext.Products.FindAsync(productId);
        if (product == null) return BadRequest("Product not found");

        if (user.Wishlist.Products.Any(x => x.Id == productId))
        {
            return BadRequest("Product already in wishlist");
        }

        user.Wishlist.Products.Add(product);
        await DbContext.SaveChangesAsync();

        return Ok("Product added to wishlist");
    }

    [HttpDelete]
    [Route("remove-item/{productId:int}")]
    public async Task<IActionResult> RemoveFromWishlist(int productId)
    {
        var user = await DbContext.Users
            .OrderBy(u => u.Id)
            .Include(user => user.Wishlist)
            .ThenInclude(wishlist => wishlist.Products)
            .FirstOrDefaultAsync(x => x.Id == UserPrincipal.Id);
        if (user == null) return Unauthorized("User not found");

        var product = await DbContext.Products.FindAsync(productId);
        if (product == null) return BadRequest("Product not found");

        if (user.Wishlist.Products.All(x => x.Id != productId))
        {
            return BadRequest("Product not in wishlist");
        }

        user.Wishlist.Products.Remove(product);
        await DbContext.SaveChangesAsync();

        return Ok();
    }

    public WishlistController(IMapper mapper, EzTechDbContext dbContext, IEmailManager emailManager) : base(mapper,
        dbContext, emailManager)
    {
    }
}
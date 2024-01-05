using AutoMapper;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.ApiModels.UserApiModels.RatingApiModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;

namespace EzTech.Api.Controllers.UserControllers;

[ApiController]
[Route("api/[controller]")]
public class RatingController : BaseUserController
{
    [HttpPost]
    [Route("{productId:int}")]
    public async Task<IActionResult> AddRating(int productId, [FromBody] AddRatingRequest request)
    {
        var user = await DbContext.Users.FindAsync(UserPrincipal.Id);
        var product = await DbContext.Products.FindAsync(productId);
        if (user == null) return NotFound("User not found");
        if (product == null) return NotFound("Product not found");
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

    [HttpPatch]
    [Route("{ratingId:int}")]
    public async Task<IActionResult> UpdateRating(int ratingId, [FromBody] UpdateRatingRequest request)
    {
        var rating = await DbContext.Ratings.FindAsync(ratingId);
        if (rating == null) return NotFound("Rating not found");
        rating.Rate = request.Rate ?? rating.Rate;
        rating.Comment = request.Comment ?? rating.Comment;
        await DbContext.SaveChangesAsync();
        return Ok("Rating updated");
    }

    public RatingController(IMapper mapper, EzTechDbContext dbContext, IEmailManager emailManager) : base(mapper,
        dbContext, emailManager)
    {
    }
}
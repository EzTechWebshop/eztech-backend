using AutoMapper;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.ApiModels.AdminApiModels.PromotionApiModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class PromotionController : BaseAdminController
{
    private readonly IBlobService _blobService;

    [HttpPost]
    public async Task<IActionResult> CreatePromotion([FromBody] CreatePromotionRequest request)
    {
        var existingPromotion = await DbContext.Promotions.FirstOrDefaultAsync(e => e.Title == request.Title);
        if (existingPromotion != null)
        {
            return Conflict("Promotion with this title already exists");
        }

        var promotion = new Promotion
        {
            Title = request.Title,
            Description = request.Description,
        };
        if (request.StartDate != null)
        {
            promotion.StartDate = request.StartDate.Value;
        }

        if (request.EndDate != null)
        {
            promotion.EndDate = request.EndDate.Value;
        }

        await DbContext.Promotions.AddAsync(promotion);
        await DbContext.SaveChangesAsync();

        var response = Mapper.Map<Promotion>(promotion);

        return Ok(response);
    }

    [HttpPost]
    [Route("{promotionId:int}/add-image")]
    public async Task<IActionResult> AddImage(int promotionId, IFormFile file)
    {
        var promotion = await DbContext.Promotions.FindAsync(promotionId);
        if (promotion == null) return NotFound("Promotion not found");
        var newFileName = $"{Guid.NewGuid()}_{Path.GetExtension(file.FileName)}_";
        if (promotion.ImageUrl != null)
        {
            await _blobService.DeleteImage(promotion.ImageUrl);
        }

        var (status, message) = await _blobService.UploadImage(newFileName, file);
        if (status != 1) return BadRequest(message);
        promotion.ImageUrl = newFileName;
        await DbContext.SaveChangesAsync();
        return Ok("Image added to promotion");
    }

    [HttpPatch]
    [Route("{promotionId:int}")]
    public async Task<IActionResult> UpdatePromotion(int promotionId, [FromBody] UpdatePromotionRequest request)
    {
        var promotion = await DbContext.Promotions.FindAsync(promotionId);
        if (promotion == null) return NotFound("Promotion not found");
        promotion.Title = request.Title ?? promotion.Title;
        promotion.Description = request.Description ?? promotion.Description;
        promotion.StartDate = request.StartDate ?? promotion.StartDate;
        promotion.EndDate = request.EndDate ?? promotion.EndDate;
        await DbContext.SaveChangesAsync();
        return Ok("Promotion updated");
    }

    [HttpDelete]
    [Route("{promotionId:int}")]
    public async Task<IActionResult> DeletePromotion(int promotionId)
    {
        var promotion = await DbContext.Promotions.FindAsync(promotionId);
        if (promotion == null) return NotFound("Promotion not found");
        if (promotion.ImageUrl != null)
        {
            await _blobService.DeleteImage(promotion.ImageUrl);
        }
        DbContext.Promotions.Remove(promotion);
        await DbContext.SaveChangesAsync();
        return Ok("Promotion deleted");
    }

    public PromotionController(IMapper mapper, EzTechDbContext dbContext, IBlobService blobService) : base(mapper,
        dbContext)
    {
        _blobService = blobService;
    }
}
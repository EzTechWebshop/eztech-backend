using AutoMapper;
using EzTech.Data;
using EzTech.Data.ApiModels.AdminApiModels.CategoryApiModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class CategoryController : BaseAdminController
{
    [HttpGet]
    public async Task<IActionResult> GetCategories()
    {
        var categories = await DbContext.Categories.ToListAsync();
        if (!categories.Any())
        {
            return NotFound("No categories found");
        }

        return Ok(categories);
    }

    [HttpGet]
    [Route("{id:int}")]
    public async Task<IActionResult> GetCategory(int id)
    {
        var category = await DbContext.Categories.FindAsync(id);
        if (category == null)
        {
            return NotFound();
        }

        return Ok(category);
    }

    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] AddCategoryRequest request)
    {
        var categoryExists = await DbContext.Categories.AnyAsync(x => x.Name == request.Name);
        if (categoryExists)
        {
            return Conflict("Category already exists");
        }

        var category = new Category
        {
            Name = request.Name,
            Description = request.Description
        };

        await DbContext.Categories.AddAsync(category);
        await DbContext.SaveChangesAsync();

        return Ok(category);
    }

    [HttpPatch]
    [Route("{categoryId:int}")]
    public async Task<IActionResult> UpdateCategory(int categoryId, [FromBody] UpdateCategoryRequest request)
    {
        var category = await DbContext.Categories.FindAsync(categoryId);
        if (category == null)
        {
            return NotFound("Category not found");
        }

        category.Name = request.Name ?? category.Name;
        category.Description = request.Description ?? category.Description;

        await DbContext.SaveChangesAsync();

        return Ok(category);
    }

    [HttpDelete]
    [Route("{categoryId:int}")]
    public async Task<IActionResult> DeleteCategory(int categoryId)
    {
        var category = await DbContext.Categories.FindAsync(categoryId);
        if (category == null)
        {
            return NotFound("Category not found");
        }

        DbContext.Categories.Remove(category);
        await DbContext.SaveChangesAsync();

        return Ok("Category deleted");
    }


    public CategoryController(IMapper mapper, EzTechDbContext dbContext) : base(mapper, dbContext)
    {
    }
}
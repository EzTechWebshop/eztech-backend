using AutoMapper;
using EzTech.Api.Services;
using EzTech.Data;
using EzTech.Data.ApiModels.AdminApiModels.ProductApiModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class ProductController : BaseAdminController
{
    private readonly IBlobService _fileService;

    [HttpPost]
    [Route("{productId:int}/add-image")]
    public async Task<IActionResult> AddImage(int productId, IFormFile file)
    {
        var product = await DbContext.Products.FindAsync(productId);
        if (product == null) return NotFound("Product not found");
        var newFileName = $"{Guid.NewGuid()}_{Path.GetExtension(file.FileName)}_";
        while (await _fileService.ImageExists(newFileName))
        {
            newFileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
        }

        var (status, message) = await _fileService.UploadImage(newFileName, file);
        if (status != 1) return BadRequest(message);
        // Filename minus file extension
        var image = new Image
        {
            Name = Path.GetFileNameWithoutExtension(file.FileName),
            FileName = message,
            Alt = "image",
        };
        product.Images.Add(image);
        await DbContext.SaveChangesAsync();
        return Ok("Image added to product");
    }

    [HttpDelete]
    [Route("{productId:int}/remove-image/{imageId:int}")]
    public async Task<IActionResult> RemoveImage(int productId, int imageId)
    {
        var product = await DbContext.Products
            .OrderBy(u => u.Id)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == productId);

        if (product == null) return NotFound();
        var image = product.Images.FirstOrDefault(x => x.Id == imageId);
        if (image == null) return NotFound();
        if (product.ThumbnailId == image.Id)
        {
            product.ThumbnailId = null;
        }

        var (status, message) = await _fileService.DeleteImage(image.FileName);
        if (status == 1)
        {
            product.Images.Remove(image);
            await DbContext.SaveChangesAsync();
            return Ok("Image removed from product");
        }

        return BadRequest(message);
    }

    [HttpPost]
    [Route("{productId:int}/set-thumbnail/{imageId:int}")]
    public async Task<IActionResult> SetThumbnail(int productId, int imageId)
    {
        var product = await DbContext.Products
            .OrderBy(x => x.Id)
            .Include(x => x.Images)
            .FirstOrDefaultAsync(x => x.Id == productId);
        if (product == null) return NotFound();
        var image = product.Images.FirstOrDefault(x => x.Id == imageId);
        if (image == null) return NotFound();
        product.ThumbnailId = image.Id;
        await DbContext.SaveChangesAsync();
        return Ok("Thumbnail set");
    }


    [HttpPost]
    [Route("{productId:int}/add-category/{categoryId:int}")]
    public async Task<IActionResult> AddCategory(int productId, int categoryId)
    {
        var product = await DbContext.Products.FindAsync(productId);
        if (product == null) return NotFound();
        var category = await DbContext.Categories.FindAsync(categoryId);
        if (category == null) return NotFound();
        product.Categories.Add(category);
        await DbContext.SaveChangesAsync();
        return Ok("Category added to product");
    }

    [HttpPost]
    [Route("{productId:int}/remove-category/{categoryId:int}")]
    public async Task<IActionResult> RemoveCategory(int productId, int categoryId)
    {
        var product = await DbContext.Products
            .OrderBy(u => u.Id)
            .Include(x => x.Categories)
            .FirstOrDefaultAsync(x => x.Id == productId);
        if (product == null) return NotFound();
        var category = await DbContext.Categories.FindAsync(categoryId);
        if (category == null) return NotFound();
        product.Categories.Remove(category);
        await DbContext.SaveChangesAsync();
        return Ok("Category removed from product");
    }

    [HttpDelete]
    [Route("remove-product/{id:int}")]
    public async Task<IActionResult> RemoveProduct(int id)
    {
        var product = await DbContext.Products.FindAsync(id);
        if (product == null) return NotFound();
        product.IsDeleted = true;
        await DbContext.SaveChangesAsync();
        return Ok("Product removed");
    }

    [HttpPost]
    [Route("restore-product/{id:int}")]
    public async Task<IActionResult> RestoreProduct(int id)
    {
        var product = await DbContext.Products.FindAsync(id);
        if (product == null) return NotFound();
        product.IsDeleted = false;
        await DbContext.SaveChangesAsync();
        return Ok("Product restored");
    }

    [HttpGet]
    [Route("id:int")]
    public async Task<IActionResult> GetProduct(int id)
    {
        var product = await DbContext.Products
            .OrderBy(u => u.Id)
            .Include(x => x.Categories)
            .Include(x => x.Images)
            .Include(x => x.Ratings)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
        if (product == null) return NotFound();
        return Ok(product);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProduct([FromBody] AddProductRequest request)
    {
        var existingProduct = await DbContext.Products
            .OrderBy(u => u.Id)
            .FirstOrDefaultAsync(x => x.Name == request.Name);
        if (existingProduct != null) return Conflict("Product already exists");
        var product = new Product
        {
            Name = request.Name,
            Description = request.Description,
            Price = request.Price,
            Stock = request.Stock,
            Sold = 0,
        };

        await DbContext.Products.AddAsync(product);
        await DbContext.SaveChangesAsync();
        return Ok(product);
    }

    [HttpPatch]
    [Route("{id:int}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductRequest request)
    {
        var product = await DbContext.Products.FindAsync(id);
        if (product == null) return NotFound();
        product.Update(request);
        await DbContext.SaveChangesAsync();
        return Ok(product);
    }

    [HttpDelete]
    [Route("delete-product-from-database/{id:int}/name/{name}")]
    public async Task<IActionResult> DeleteProduct(int id, string name)
    {
        var product = await DbContext.Products.FindAsync(id);
        if (product == null) return NotFound();
        if (product.Name != name) return BadRequest("Product name does not match");
        foreach (var image in product.Images)
        {
            await _fileService.DeleteImage(image.FileName);
        }

        DbContext.Products.Remove(product);
        await DbContext.SaveChangesAsync();
        return Ok("Product deleted");
    }

    public ProductController(IMapper mapper, EzTechDbContext dbContext, IBlobService fileService) : base(mapper,
        dbContext)
    {
        _fileService = fileService;
    }
}
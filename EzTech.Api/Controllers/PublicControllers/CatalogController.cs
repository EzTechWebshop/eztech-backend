using AutoMapper;
using EzTech.Data;
using EzTech.Data.ApiModels.CatalogApiModels;
using EzTech.Data.DtoModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.PublicControllers;

/// <summary>
/// Catalog that is public to all people who visit the website, and is used to display products and categories
/// TODO: Maybe add a way to limit the amount of calls to the database to avoid DDOS attacks or something
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class CatalogController : BaseController
{
    private readonly EzTechDbContext _ezTechDbContext;
    private readonly IMapper _mapper;
    

    /// <summary>
    /// This is the universal method for getting products in the catalog.
    /// Pagination: https://learn.microsoft.com/en-us/ef/core/querying/pagination
    /// </summary>
    [HttpGet]
    [Route("get-products")]
    public async Task<ActionResult> GetProducts(
        [FromQuery] string? includeSoldOut,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] string? search,
        [FromQuery] string? sort,
        [FromQuery] int? categoryId)
    {
        // TODO: Maybe this should be moved to a config file or something
        const int defaultPage = 1;
        const int defaultPageSize = 25;
        page ??= defaultPage;
        pageSize ??= defaultPageSize;
        if (page < 1) page = defaultPage;
        if (pageSize < 1) pageSize = defaultPageSize;
        var products = _ezTechDbContext.Products
            .Include(x => x.Categories)
            .Include(x => x.Images)
            .Include(x => x.Ratings)
            .Where(x => !x.IsDeleted)
            .AsSplitQuery()
            .AsQueryable();
        if (search != null)
        {
            products = products.Where(x => x.Name.Contains(search));
        }

        // Sorting based off
        products = sort switch
        {
            "popularity_asc" => products.OrderBy(p => p.Sold),
            "popularity_desc" => products.OrderByDescending(p => p.Sold),
            "name_asc" => products.OrderBy(p => p.Name),
            "name_desc" => products.OrderByDescending(p => p.Name),
            "date_asc" => products.OrderBy(p => p.CreatedAt),
            "date_desc" => products.OrderByDescending(p => p.CreatedAt),
            "price_asc" => products.OrderBy(p => p.Price),
            "price_desc" => products.OrderByDescending(p => p.Price),
            "stock_asc" => products.OrderBy(p => p.Stock),
            "stock_desc" => products.OrderByDescending(p => p.Stock),
            "rating_asc" => products.OrderBy(p => p.AverageRating),
            "rating_desc" => products.OrderByDescending(p => p.AverageRating),
            // Default Sorting
            _ => products.OrderBy(p => p.Name)
        };
        if (includeSoldOut != "true")
        {
            products = products.Where(x => x.Stock > 0);
        }

        var totalProducts = products.Count();
        // If there are no products, return an empty list
        if (totalProducts == 0)
        {
            var response2 = new ProductListResponse
            {
                TotalProducts = totalProducts,
                TotalPages = 0,
                Page = page.Value,
                PageSize = pageSize.Value,
                Search = search ?? "",
                Sort = sort ?? "",
                Products = new List<ProductDto>(),
            };
            return Ok(response2);
        }

        var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize.Value);
        if (page > totalPages) page = totalPages;
        // Pagination
        products = products.Skip((int)((page - 1) * pageSize)!).Take((int)pageSize);
        var productList = await products.ToListAsync();
        var response = new ProductListResponse
        {
            TotalProducts = totalProducts,
            TotalPages = totalPages,
            Page = page.Value,
            PageSize = pageSize.Value,
            Search = search ?? "",
            Sort = sort ?? "",
            Products = _mapper.Map<List<ProductDto>>(productList),
        };
        return Ok(response);
    }

    [HttpGet]
    [Route("get-product-by-id/{id:int}")]
    public async Task<ActionResult> GetProduct(int id)
    {
        var product = await _ezTechDbContext.Products
            .OrderBy(x => x.Name)
            .Include(x => x.Categories)
            .Include(x => x.Images)
            .Include(x => x.Ratings)
            .AsSplitQuery()
            .FirstOrDefaultAsync(x => x.Id == id);
        if (product == null)
        {
            return NotFound("Product not found");
        }

        var response = _mapper.Map<ProductDto>(product);
        return Ok(response);
    }

    [HttpGet]
    [Route("get-categories")]
    public async Task<ActionResult> GetCategories()
    {
        var categories = await _ezTechDbContext.Categories
            .OrderBy(x => x.Name)
            .Include(x => x.Products)
            .AsSplitQuery()
            .ToListAsync();
        if (!categories.Any())
        {
            return NotFound("No categories found");
        }

        var response = new CategoryListResponse
        {
            Categories = _mapper.Map<List<CategoryDto>>(categories),
            TotalCategories = categories.Count
        };
        return Ok(response);
    }

    [HttpGet]
    [Route("get-category-by-id/{id:int}")]
    public async Task<ActionResult> GetCategory(int id)
    {
        var category = await _ezTechDbContext.Categories
            .OrderBy(x => x.Name)
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == id);
        if (category == null)
        {
            return NotFound("Category not found");
        }

        var response = _mapper.Map<CategoryDto>(category);

        return Ok(response);
    }

    [HttpGet]
    [Route("get-faq")]
    public async Task<ActionResult> GetFaq()
    {
        var faq = await _ezTechDbContext.Faqs.OrderBy(x => x.Question).ToListAsync();
        if (!faq.Any())
        {
            var noFaqResponse = new FaqListResponse
            {
                Faqs = new List<FaqDto>(),
                TotalFaqs = 0
            };
            return Ok(noFaqResponse);
        }

        var response = new FaqListResponse
        {
            Faqs = _mapper.Map<List<FaqDto>>(faq),
            TotalFaqs = faq.Count
        };

        return Ok(response);
    }

    // Website Info related endpoints
    [HttpGet]
    [Route("get-website-info")]
    public async Task<ActionResult> GetWebsiteInfo()
    {
        var websiteInfo = await _ezTechDbContext.WebsiteInfos
            .OrderBy(x => x.Id)
            .Include(x => x.WeeklyOpeningHours.OrderBy(day => day.DayOfWeek))
            .Include(x => x.SpecialOpeningHours)
            .Include(x => x.WebsiteInfoFields.OrderBy(text => text.Title))
            .FirstOrDefaultAsync();
        
        if (websiteInfo == null)
        {
            return NotFound("No website info found");
        }
        var response = _mapper.Map<WebsiteInfoDto>(websiteInfo);
        return Ok(response);
    }


    // Promotion
    [HttpGet]
    [Route("get-promotions")]
    public async Task<ActionResult> GetPromotions()
    {
        var promotions = await _ezTechDbContext.Promotions
            .OrderBy(x => x.Id)
            .Include(x => x.Products)
            .AsSplitQuery()
            .ToListAsync();
        if (!promotions.Any())
        {
            return NotFound("No promotions found");
        }
        var response = new PromotionListResponse
        {
            Promotions = _mapper.Map<List<PromotionDto>>(promotions),
            PromotionCount = promotions.Count
        };
        
        return Ok(response);
    }
    
    public CatalogController(IMapper mapper, EzTechDbContext dbContext, EzTechDbContext ezTechDbContext) : base(mapper, dbContext)
    {
        _mapper = mapper;
        _ezTechDbContext = ezTechDbContext;
    }
}
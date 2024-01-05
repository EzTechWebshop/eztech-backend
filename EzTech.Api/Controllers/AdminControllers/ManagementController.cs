using AutoMapper;
using EzTech.Data;
using EzTech.Data.ApiModels.AdminApiModels.DashboardApiModels;
using EzTech.Data.ApiModels.AdminApiModels.ManagementApiModels;
using EzTech.Data.DtoModels;
using EzTech.Data.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Api.Controllers.AdminControllers;

[ApiController]
[Route("api/admin/[controller]")]
public class ManagementController : BaseAdminController
{
    [HttpGet]
    [Route("get-users")]
    public async Task<ActionResult> GetUsers(
        [FromQuery] int? userId,
        [FromQuery] string? search,
        [FromQuery] string? sort,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        const int defaultPage = 1;
        const int defaultPageSize = 25;
        page ??= defaultPage;
        pageSize ??= defaultPageSize;
        if (page < 1) page = defaultPage;
        var users = DbContext.Users
            .OrderBy(x => x.Id)
            .Include(x => x.Orders)
            .Include(x => x.Ratings)
            .AsSplitQuery()
            .AsQueryable();
        if (search != null)
        {
            users = users.Where(x => x.FirstName.Contains(search) || x.LastName.Contains(search));
        }

        if (sort != null)
        {
            users = sort switch
            {
                "firstName_asc" => users.OrderBy(p => p.FirstName),
                "firstName_desc" => users.OrderByDescending(p => p.FirstName),
                "lastName_asc" => users.OrderBy(p => p.LastName),
                "lastName_desc" => users.OrderByDescending(p => p.LastName),
                "activeOrders_asc" => users.OrderBy(p => p.Orders.Count(x => x.Status != OrderStatus.Completed)),
                "activeOrders_desc" => users.OrderByDescending(p =>
                    p.Orders.Count(x => x.Status != OrderStatus.Completed)),
                "completedOrders_asc" => users.OrderBy(p => p.Orders.Count(x => x.Status == OrderStatus.Completed)),
                "completedOrders_desc" => users.OrderByDescending(p =>
                    p.Orders.Count(x => x.Status == OrderStatus.Completed)),
                "ratings_asc" => users.OrderBy(p => p.Ratings.Count),
                "ratings_desc" => users.OrderByDescending(p => p.Ratings.Count),

                _ => users.OrderByDescending(p => p.FirstName)
            };
        }
        else
        {
            users = users.OrderByDescending(p => p.FirstName);
        }

        var totalUsers = users.Count();
        // Pagination
        var totalPages = (int)Math.Ceiling((double)totalUsers / pageSize.Value);
        if (page > totalPages) page = totalPages;
        // Pagination
        users = users.Skip((int)((page - 1) * pageSize)!).Take((int)pageSize);
        var userList = await users.ToListAsync();
        var user = await DbContext.Promotions
            .OrderBy(u => u.Id)
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == userId);

        var response = new GetUsersApiResponse()
        {
            TotalUsers = totalUsers,
            TotalPages = totalPages,
            Page = page.Value,
            PageSize = pageSize.Value,
            Search = search ?? "",
            Sort = sort ?? "",
            Users = Mapper.Map<List<UserDto>>(userList),
            User = Mapper.Map<UserDto>(user),
        };
        return Ok();
    }

    [HttpGet]
    [Route("get-promotions")]
    public async Task<ActionResult> GetPromotions(
        [FromQuery] int? promotionId,
        [FromQuery] string? search,
        [FromQuery] string? sort,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        const int defaultPage = 1;
        const int defaultPageSize = 25;
        page ??= defaultPage;
        pageSize ??= defaultPageSize;
        if (page < 1) page = defaultPage;

        var promotions = DbContext.Promotions
            .OrderBy(x => x.Id)
            .Include(x => x.Products)
            .AsSplitQuery()
            .AsQueryable();

        if (search != null)
        {
            promotions = promotions.Where(x => x.Title == search);
        }

        if (sort != null)
        {
            promotions = sort switch
            {
                "name_asc" => promotions.OrderBy(p => p.Title),
                "name_desc" => promotions.OrderByDescending(p => p.Title),
                "products_asc" => promotions.OrderBy(p => p.Products.Count),
                "products_desc" => promotions.OrderByDescending(p => p.Products.Count),
                "startDate_asc" => promotions.OrderBy(p => p.StartDate),
                "startDate_desc" => promotions.OrderByDescending(p => p.StartDate),
                "endDate_asc" => promotions.OrderBy(p => p.EndDate),
                "endDate_desc" => promotions.OrderByDescending(p => p.EndDate),
                // Default Sorting
                _ => promotions.OrderByDescending(p => p.Title)
            };
        }
        else
        {
            promotions = promotions.OrderByDescending(p => p.Title);
        }

        var totalPromotions = promotions.Count();

        if (totalPromotions == 0)
        {
            var emptyResponse = new GetPromotionsApiResponse
            {
                TotalPromotions = totalPromotions,
                TotalPages = 0,
                Page = page.Value,
                PageSize = pageSize.Value,
                Search = search ?? "",
                Sort = sort ?? "",
                Promotions = new List<PromotionDto>(),
            };
            return Ok(emptyResponse);
        }

        // Pagination
        var totalPages = (int)Math.Ceiling((double)totalPromotions / pageSize.Value);
        if (page > totalPages) page = totalPages;
        // Pagination
        promotions = promotions.Skip((int)((page - 1) * pageSize)!).Take((int)pageSize);
        var promotionList = await promotions.ToListAsync();

        var promotion = await DbContext.Promotions
            .OrderBy(u => u.Id)
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == promotionId);

        var response = new GetPromotionsApiResponse
        {
            TotalPromotions = totalPromotions,
            TotalPages = totalPages,
            Page = page.Value,
            PageSize = pageSize.Value,
            Search = search ?? "",
            Sort = sort ?? "",
            Promotions = Mapper.Map<List<PromotionDto>>(promotionList),
            Promotion = Mapper.Map<PromotionDto>(promotion),
            PromotionProducts = Mapper.Map<List<ProductDto>>(promotion?.Products),
        };
        return Ok(response);
    }


    [HttpGet]
    [Route("get-categories")]
    public async Task<IActionResult> GetCategories(
        [FromQuery] int? categoryId,
        [FromQuery] string? search,
        [FromQuery] string? sort,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        const int defaultPage = 1;
        const int defaultPageSize = 25;
        page ??= defaultPage;
        pageSize ??= defaultPageSize;
        if (page < 1) page = defaultPage;
        var categories = DbContext.Categories
            .Include(x => x.Products)
            .AsSplitQuery()
            .AsQueryable();

        if (search != null)
        {
            categories = categories.Where(x => x.Name.Contains(search));
        }

        if (sort != null)
        {
            categories = sort switch
            {
                "name_asc" => categories.OrderBy(p => p.Name),
                "name_desc" => categories.OrderByDescending(p => p.Name),
                "products_asc" => categories.OrderBy(p => p.Products.Count),
                "products_desc" => categories.OrderByDescending(p => p.Products.Count),
                // Default Sorting
                _ => categories.OrderByDescending(p => p.Name)
            };
        }
        else
        {
            categories = categories.OrderByDescending(p => p.Name);
        }

        // Find Product, maybe put this in a separate method
        var category = await DbContext.Categories
            .OrderBy(u => u.Id)
            .Include(x => x.Products)
            .FirstOrDefaultAsync(x => x.Id == categoryId);
        var totalCategories = categories.Count();
        if (totalCategories == 0)
        {
            var emptyResponse = new GetCategoriesApiResponse
            {
                TotalCategories = totalCategories,
                TotalPages = 0,
                Page = page.Value,
                PageSize = pageSize.Value,
                Search = search ?? "",
                Sort = sort ?? "",
                Categories = new List<CategoryDto>(),
                Category = Mapper.Map<CategoryDto>(category),
            };
            return Ok(emptyResponse);
        }

        // Pagination
        var totalPages = (int)Math.Ceiling((double)totalCategories / pageSize.Value);
        if (page > totalPages) page = totalPages;
        // Pagination
        categories = categories.Skip((int)((page - 1) * pageSize)!).Take((int)pageSize);
        var categoryList = await categories.ToListAsync();

        var response = new GetCategoriesApiResponse
        {
            TotalCategories = totalCategories,
            TotalPages = totalPages,
            Page = page.Value,
            PageSize = pageSize.Value,
            Search = search,
            Sort = sort,
            Categories = Mapper.Map<List<CategoryDto>>(categoryList),
            Category = Mapper.Map<CategoryDto>(category),
            CategoryProducts = Mapper.Map<List<ProductDto>>(category?.Products),
        };
        return Ok(response);
    }

    [HttpGet]
    [Route("get-products")]
    public async Task<IActionResult> GetProducts(
        [FromQuery] int? productId,
        [FromQuery] int? categoryId,
        [FromQuery] int? minPrice,
        [FromQuery] int? maxPrice,
        [FromQuery] string? search,
        [FromQuery] DateTime? startDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? sort,
        [FromQuery] int? page,
        [FromQuery] int? pageSize
    )
    {
        const int defaultPage = 1;
        const int defaultPageSize = 25;
        page ??= defaultPage;
        pageSize ??= defaultPageSize;
        if (page < 1) page = defaultPage;
        if (pageSize < 1) pageSize = defaultPageSize;
        var products = DbContext.Products
            .Include(x => x.Categories)
            .Include(x => x.Images)
            .Include(x => x.Ratings)
            .AsSplitQuery()
            .AsQueryable();
        if (categoryId != null)
        {
            products = products.Where(x => x.Categories.Any(c => c.Id == categoryId));
        }

        if (minPrice != null)
        {
            products = products.Where(x => x.Price >= minPrice);
        }

        if (maxPrice != null)
        {
            products = products.Where(x => x.Price <= maxPrice);
        }

        if (search != null)
        {
            products = products.Where(x => x.Name.Contains(search));
        }

        if (startDate != null)
        {
            products = products.Where(x => x.CreatedAt >= startDate);
        }

        if (endDate != null)
        {
            products = products.Where(x => x.CreatedAt <= endDate);
        }

        if (sort != null)
        {
            products = sort switch
            {
                "name_asc" => products.OrderBy(p => p.Name),
                "name_desc" => products.OrderByDescending(p => p.Name),
                "price_asc" => products.OrderBy(p => p.Price),
                "price_desc" => products.OrderByDescending(p => p.Price),
                "stock_asc" => products.OrderBy(p => p.Stock),
                "stock_desc" => products.OrderByDescending(p => p.Stock),
                "sold_asc" => products.OrderBy(p => p.Sold),
                "sold_desc" => products.OrderByDescending(p => p.Sold),
                "rating_asc" => products.OrderBy(p => p.AverageRating),
                "rating_desc" => products.OrderByDescending(p => p.AverageRating),
                "removed_asc" => products.OrderBy(p => p.IsDeleted),
                "removed_desc" => products.OrderByDescending(p => p.IsDeleted),
                "created_asc" => products.OrderBy(p => p.CreatedAt),
                "created_desc" => products.OrderByDescending(p => p.CreatedAt),
                "discount_asc" => products.OrderBy(p => p.Discount),
                "discount_desc" => products.OrderByDescending(p => p.Discount),
                // Default Sorting
                _ => products.OrderBy(p => p.Name)
            };
        }
        else
        {
            products = products.OrderBy(p => p.Name);
        }

        var totalProducts = products.Count();
        if (totalProducts == 0)
        {
            var emptyResponse = new GetProductsApiResponse
            {
                TotalProducts = totalProducts,
                TotalPages = 0,
                Page = page.Value,
                PageSize = pageSize.Value,
                MinPrice = minPrice,
                MaxPrice = maxPrice,
                MinDate = startDate,
                MaxDate = endDate,
                Search = search ?? "",
                Sort = sort ?? "",
                Products = new List<ProductDto>(),
            };
            return Ok(emptyResponse);
        }

        var totalPages = (int)Math.Ceiling((double)totalProducts / pageSize.Value);
        if (page > totalPages) page = totalPages;
        // Pagination
        products = products.Skip((int)((page - 1) * pageSize)!).Take((int)pageSize);
        var productList = await products.ToListAsync();
        // Find Product, maybe put this in a separate method
        var product = await DbContext.Products
            .OrderBy(u => u.Id)
            .Include(x => x.Categories)
            .Include(x => x.Images)
            .Include(x => x.Ratings)
            .FirstOrDefaultAsync(x => x.Id == productId);

        var category = await DbContext.Categories.FindAsync(categoryId);
        var response = new GetProductsApiResponse
        {
            TotalProducts = totalProducts,
            TotalPages = totalPages,
            Page = page.Value,
            PageSize = pageSize.Value,
            MinPrice = minPrice,
            MaxPrice = maxPrice,
            MinDate = startDate,
            MaxDate = endDate,
            Search = search,
            Sort = sort,
            Products = Mapper.Map<List<ProductDto>>(productList),
            Product = Mapper.Map<ProductDto>(product),
            Category = Mapper.Map<CategoryDto>(category),
        };
        return Ok(response);
    }

    [HttpGet]
    [Route("get-orders")]
    public async Task<ActionResult> GetOrders(
        [FromQuery] bool? archive,
        [FromQuery] int? orderId,
        [FromQuery] int? userId,
        [FromQuery] DateTime? minDate,
        [FromQuery] DateTime? endDate,
        [FromQuery] string? sort,
        [FromQuery] int? page,
        [FromQuery] int? pageSize,
        [FromQuery] OrderStatus? status
    )
    {
        const int defaultPage = 1;
        const int defaultPageSize = 25;
        page ??= defaultPage;
        pageSize ??= defaultPageSize;
        if (page < 1) page = defaultPage;

        var orders = DbContext.Orders
            .OrderBy(o => o.CreatedAt)
            .Include(o => o.Items)
            .Include(o => o.User)
            .AsSplitQuery()
            .AsQueryable();
        if (userId != null)
        {
            orders = orders.Where(o => o.UserId == userId);
        }

        if (status != null)
        {
            orders = orders.Where(o => o.Status == status);
        }

        if (minDate != null)
        {
            orders = orders.Where(o => o.CreatedAt >= minDate);
        }

        if (endDate != null)
        {
            orders = orders.Where(o => o.CreatedAt <= endDate);
        }

        if (sort != null)
        {
            orders = sort switch
            {
                "id_asc" => orders.OrderBy(o => o.Id),
                "id_desc" => orders.OrderByDescending(o => o.Id),
                "date_asc" => orders.OrderBy(o => o.CreatedAt),
                "date_desc" => orders.OrderByDescending(o => o.CreatedAt),
                "total_asc" => orders.OrderBy(o => o.Total),
                "total_desc" => orders.OrderByDescending(o => o.Total),
                "status_asc" => orders.OrderBy(o => o.Status),
                "status_desc" => orders.OrderByDescending(o => o.Status),
                "created_asc" => orders.OrderBy(o => o.CreatedAt),
                "created_desc" => orders.OrderByDescending(o => o.CreatedAt),
                // Default Sorting
                _ => orders.OrderByDescending(o => o.CreatedAt)
            };
        }
        else
        {
            orders = orders.OrderByDescending(p => p.CreatedAt);
        }

        var totalOrders = orders.Count();
        if (totalOrders == 0)
        {
            var emptyResponse = new GetOrdersApiResponse
            {
                TotalOrders = totalOrders,
                TotalPages = 0,
                Page = page.Value,
                PageSize = pageSize.Value,
                MinDate = minDate,
                MaxDate = endDate,
                Sort = sort ?? "",
                Orders = new List<OrderDto>(),
            };
            return Ok(emptyResponse);
        }

        // Pagination
        var totalPages = (int)Math.Ceiling((double)totalOrders / pageSize.Value);
        if (page > totalPages) page = totalPages;
        // Pagination
        orders = orders.Skip((int)((page - 1) * pageSize)!).Take((int)pageSize);
        var orderList = await orders.ToListAsync();
        orderList = archive == true
            ? orderList.Where(o => o.Status
                is OrderStatus.Completed
                or OrderStatus.Cancelled).ToList()
            : orderList.Where(o => o.Status != OrderStatus
                .Completed && o.Status != OrderStatus.Cancelled).ToList();
        // Find Order, maybe put this in a separate method
        var order = await DbContext.Orders
            .OrderBy(u => u.Id)
            .Include(o => o.Items)
            .Include(o => o.User)
            .FirstOrDefaultAsync(o => o.Id == orderId);
        var user = await DbContext.Users.FindAsync(userId);
        var statusName = status?.ToString();
        var response = new GetOrdersApiResponse
        {
            Status = statusName,
            TotalOrders = totalOrders,
            TotalPages = totalPages,
            Page = page.Value,
            PageSize = pageSize.Value,
            MinDate = minDate,
            MaxDate = endDate,
            Sort = sort,
            Orders = Mapper.Map<List<OrderDto>>(orderList),
            Order = Mapper.Map<OrderDto>(order),
        };
        return Ok(response);
    }

    [HttpGet]
    [Route("get-faqs")]
    public async Task<ActionResult> GetFaqs(
        [FromQuery] int? faqId
    )
    {
        var faqs = await DbContext.Faqs.ToListAsync();
        var faq = await DbContext.Faqs.FindAsync(faqId) ?? new Faq();
        var response = new GetFaqsApiResponse
        {
            TotalFaqs = faqs.Count,
            Faqs = Mapper.Map<List<FaqDto>>(faqs),
            Faq = Mapper.Map<FaqDto>(faq),
        };
        return Ok(response);
    }

    [HttpGet]
    [Route("get-website-info")]
    public async Task<ActionResult> GetWebsiteInfo(
        [FromQuery] WebsiteInfoFieldType? topic
    )
    {
        var websiteInfo = await DbContext.WebsiteInfos
            .OrderBy(x => x.Id)
            .Include(x => x.WebsiteInfoFields.OrderBy(text => text.Title))
            .FirstOrDefaultAsync();
        if (websiteInfo == null)
        {
            return NotFound("Website Info not found");
        }

        var aboutUsCount = websiteInfo.WebsiteInfoFields.Count(x => x.Type == WebsiteInfoFieldType.AboutUs);
        var contactUsCount = websiteInfo.WebsiteInfoFields.Count(x => x.Type == WebsiteInfoFieldType.ContactUs);
        var termsAndConditionsCount =
            websiteInfo.WebsiteInfoFields.Count(x => x.Type == WebsiteInfoFieldType.TermsAndConditions);
        var privacyPolicyCount = websiteInfo.WebsiteInfoFields.Count(x => x.Type == WebsiteInfoFieldType.PrivacyPolicy);
        var shippingPolicyCount =
            websiteInfo.WebsiteInfoFields.Count(x => x.Type == WebsiteInfoFieldType.ShippingPolicy);
        var returnPolicyCount = websiteInfo.WebsiteInfoFields.Count(x => x.Type == WebsiteInfoFieldType.ReturnPolicy);
        var topicFields = websiteInfo.WebsiteInfoFields.Where(x => x.Type == topic).ToList();
        var response = new GetWebsiteInfoResponse
        {
            AboutUsCount = aboutUsCount,
            ContactUsCount = contactUsCount,
            TermsAndConditionsCount = termsAndConditionsCount,
            PrivacyPolicyCount = privacyPolicyCount,
            ShippingPolicyCount = shippingPolicyCount,
            ReturnPolicyCount = returnPolicyCount,
            WebsiteInfo = Mapper.Map<WebsiteInfoDto>(websiteInfo),
            Topic = topic.ToString() ?? "",
            WebsiteInfoFields = Mapper.Map<List<WebsiteInfoTopic>>(topicFields),
        };
        return Ok(response);
    }

    [HttpGet]
    [Route("get-dashboard")]
    public async Task<IActionResult> GetDashboard()
    {
        var orders = await DbContext.Orders
            .OrderBy(x => x.CreatedAt)
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

    public ManagementController(IMapper mapper, EzTechDbContext dbContext) : base(mapper, dbContext)
    {
    }
}
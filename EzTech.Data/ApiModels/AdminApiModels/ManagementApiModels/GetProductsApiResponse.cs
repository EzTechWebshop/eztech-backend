using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.AdminApiModels.ManagementApiModels;

public class GetProductsApiResponse
{
    public int TotalProducts { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int? MinPrice { get; set; }
    public int? MaxPrice { get; set; }
    public DateTime? MinDate { get; set; }
    public DateTime? MaxDate { get; set; }
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public List<ProductDto> Products { get; set; } = null!;
    public ProductDto Product { get; set; } = null!;
    public CategoryDto Category { get; set; } = null!;
}
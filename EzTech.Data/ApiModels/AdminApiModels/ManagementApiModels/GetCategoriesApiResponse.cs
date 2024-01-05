using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.AdminApiModels.ManagementApiModels;

public class GetCategoriesApiResponse
{
    public int TotalCategories { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public List<CategoryDto> Categories { get; set; } = null!;
    public CategoryDto Category { get; set; } = null!;
    public List<ProductDto> CategoryProducts { get; set; } = null!;
}
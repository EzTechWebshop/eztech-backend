using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.CatalogApiModels;

public class CategoryListResponse
{
    public List<CategoryDto> Categories { get; set; } = null!;
    public int TotalCategories { get; set; }
}
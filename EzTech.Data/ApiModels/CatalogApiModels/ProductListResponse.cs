using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.CatalogApiModels;

public class ProductListResponse
{
    public int TotalProducts { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public int TotalPages { get; set; }
    public string Search { get; set; } = null!;
    public string Sort { get; set; } = null!;
    public int? CategoryId { get; set; } = 0;
    public List<ProductDto> Products { get; set; } = null!;
}
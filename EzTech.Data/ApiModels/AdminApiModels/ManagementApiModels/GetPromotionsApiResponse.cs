using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.AdminApiModels.ManagementApiModels;

public class GetPromotionsApiResponse
{
    public int TotalPromotions { get; set; }
    public int TotalPages { get; set; }
    public int Page { get; set; }
    public int PageSize { get; set; }
    public string? Search { get; set; }
    public string? Sort { get; set; }
    public List<PromotionDto> Promotions { get; set; } = null!;
    public PromotionDto? Promotion { get; set; }
    public List<ProductDto>? PromotionProducts { get; set; }
}
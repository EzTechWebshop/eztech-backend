using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.CatalogApiModels;

public class PromotionListResponse
{
    public int PromotionCount { get; set; }
    public List<PromotionDto> Promotions { get; set; } = new();
}
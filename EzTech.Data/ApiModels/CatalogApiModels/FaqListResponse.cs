using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.CatalogApiModels;

public class FaqListResponse
{
    public List<FaqDto> Faqs { get; set; } = new();
    public int TotalFaqs { get; set; } = 0;
}
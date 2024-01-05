using EzTech.Data.DtoModels;

namespace EzTech.Data.ApiModels.AdminApiModels.ManagementApiModels;

public class GetFaqsApiResponse
{
    public int TotalFaqs { get; set; }
    public FaqDto Faq { get; set; } = null!;
    public List<FaqDto> Faqs { get; set; } = null!;
}
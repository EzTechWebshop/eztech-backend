namespace EzTech.Data.ApiModels.AdminApiModels.PromotionApiModels;

public class CreatePromotionRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
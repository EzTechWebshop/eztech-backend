namespace EzTech.Data.ApiModels.AdminApiModels.PromotionApiModels;

public class UpdatePromotionRequest
{
    public string? Title { get; set; }
    public string? Description { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
}
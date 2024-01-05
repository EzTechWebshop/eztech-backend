namespace EzTech.Data.ApiModels.WebsiteInfoApiModels;

public class AddWebsiteInfoTextRequest
{
    public string Title { get; set; } = null!;
    public string Description { get; set; } = null!;
    public string Topic { get; set; } = null!;
}
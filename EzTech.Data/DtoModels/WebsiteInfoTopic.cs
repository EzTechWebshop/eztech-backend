using EzTech.Data.Models;

namespace EzTech.Data.DtoModels;

public class WebsiteInfoTopic
{
    public int Id { get; set; }
    public string Type { get; set; } = "";
    public string Title { get; set; } = "";
    public string Description { get; set; } = "";
}
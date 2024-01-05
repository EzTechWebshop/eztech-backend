using EzTech.Data.Models;

namespace EzTech.Data.ApiModels.AdminApiModels.ProductApiModels;

public class UpdateProductRequest
{
    public string? Name { get; set; }
    public string? Description { get; set; }
    public int? Price { get; set; }
    public int? Sold { get; set; }
    public int? Discount { get; set; }
    public int? Stock { get; set; }
    public List<Category>? Categories { get; set; }
    
}
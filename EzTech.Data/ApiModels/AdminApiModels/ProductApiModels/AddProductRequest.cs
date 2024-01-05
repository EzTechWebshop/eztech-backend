using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.ApiModels.AdminApiModels.ProductApiModels;

public class AddProductRequest
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [Required]
    public decimal Price { get; set; }
    public int? Stock { get; set; } = 0;
}
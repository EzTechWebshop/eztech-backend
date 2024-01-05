using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.ApiModels.AdminApiModels.CategoryApiModels;

public class AddCategoryRequest
{
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
}
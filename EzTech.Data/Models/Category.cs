using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using EzTech.Data.ApiModels.AdminApiModels.CategoryApiModels;

namespace EzTech.Data.Models;

public class Category
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Name { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    [JsonIgnore]
    public List<Product> Products { get; set; } = new();
    
    public int TotalProducts => Products.Count;
}
using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.Models;

public class Promotion
{
    [Key]
    public int Id { get; set; }
    [Required]
    public string Title { get; set; } = null!;
    [Required]
    public string Description { get; set; } = null!;
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    
    public bool IsActive => DateTime.Now >= StartDate && DateTime.Now <= EndDate;
    public List<Product> Products { get; set; } = null!;
    public string ImageUrl { get; set; } = "no-image.jpg";
}
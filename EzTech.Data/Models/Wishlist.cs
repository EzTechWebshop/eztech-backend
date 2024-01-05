using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.Models;

public class Wishlist
{
    [Key]
    public int Id { get; set; }
    
    [Required]
    public List<Product> Products { get; set; } = new();
    
    [Required]
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
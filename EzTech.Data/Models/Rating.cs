using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace EzTech.Data.Models;

// Index prevents a user from putting multiple ratings on the same product
public class Rating
{
    [Key] 
    public int Id { get; set; }
    [Required] 
    [Range(1, 5)] 
    public int Rate { get; set; }
    
    [MaxLength(1000)]
    public string? Comment { get; set; }
    
    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
    
    [Required] 
    public int ProductId { get; set; }
    public Product Product { get; set; } = null!;
    
    [Required] 
    public int UserId { get; set; }
    public User User { get; set; } = null!;
}
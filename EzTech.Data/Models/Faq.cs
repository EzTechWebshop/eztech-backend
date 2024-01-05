using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.Models;

public class Faq
{
    [Key] 
    public int Id { get; set; }
    
    [Required] 
    public string Question { get; set; } = null!;
    
    [Required] 
    public string Answer { get; set; } = null!;
    
    [Required] 
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
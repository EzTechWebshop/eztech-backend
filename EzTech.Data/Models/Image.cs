using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.Models;

public class Image
{
    [Key] 
    public int Id { get; set; }
    
    [Required] 
    public string Name { get; set; } = null!;
    
    [Required] 
    public string FileName { get; set; } = null!;
    
    [Required] 
    public string Alt { get; set; } = "img";
}
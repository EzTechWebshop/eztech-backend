using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.Models;

public class OpeningHoursSpecial
{
    [Required] 
    public int Id { get; set; }

    [Required] 
    public string Title { get; set; } = null!;
    
    [Required]
    public string Description { get; set; } = null!;
    
    [Required]
    public DateTime Date { get; set; }
    
    [Required]
    public DateTime? EndDate { get; set; }

    [Required]
    public TimeSpan OpenTime { get; set; }

    [Required]
    public TimeSpan CloseTime { get; set; }

    public bool IsClosed => OpenTime == TimeSpan.Zero && CloseTime == TimeSpan.Zero;
}
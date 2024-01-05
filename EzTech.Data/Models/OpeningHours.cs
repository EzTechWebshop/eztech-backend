using System.ComponentModel.DataAnnotations;

namespace EzTech.Data.Models;

public class OpeningHours
{
    [Required] 
    public int Id { get; set; }
    
    [Required]
    public DayOfWeek DayOfWeek { get; set; }
    
    [Required] 
    public TimeSpan OpenTime { get; set; }
    
    [Required] 
    public TimeSpan CloseTime { get; set; }
    
    public bool IsClosed => OpenTime == TimeSpan.Zero && CloseTime == TimeSpan.Zero;
}
using System.ComponentModel.DataAnnotations;
using EzTech.Data.ApiModels.FaqApiModels;

namespace EzTech.Data.Models;

public class Faq
{
    public void CreateNew(CreateFaqRequest request)
    {
        Question = request.Question;
        Answer = request.Answer;
    }
    
    public void Update(UpdateFaqRequest request)
    {
        Question = request.Question;
        Answer = request.Answer;
    }
    
    [Key]
    public int Id { get; set; }
    [Required]
    public string Question { get; set; } = null!;
    [Required]
    public string Answer { get; set; } = null!;
    [Required]
    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;
}
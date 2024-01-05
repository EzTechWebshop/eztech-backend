namespace EzTech.Data.ApiModels.FaqApiModels;

public class CreateFaqRequest
{
    public string Question { get; set; } = null!;
    public string Answer { get; set; } = null!;
}